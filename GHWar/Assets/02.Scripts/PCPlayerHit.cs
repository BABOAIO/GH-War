using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UniRx;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine;

// PC플레이어 최상단에 넣는다.
// 컬라이더를 넣고, 태그를 PC_Player로 바꾼다.
// 대미지를 받는 오브젝트는 enter를 쓴다. 대미지를 주는 오브젝트는 stay를 쓴다.(exit의 경우, contacts를 활용할 수 없는 단점이 있다.)
public class PCPlayerHit : MonoBehaviourPunCallbacks, IPunObservable
{
    AudioSource as_hitPCPlayer;
    [SerializeField] AudioClip ac_hitPCPlayer;

    [SerializeField] Camera cam_this;

    [Header("Max HP")]
    public float MaxHP = 2.0f;
    [Header("HP")]
    public float HP = 2.0f;
    float HP_other;

    [Header("HP 슬라이더바")]
    [SerializeField] Slider hpBar;
    [Header("경고 이미지")]
    [SerializeField] GameObject img_warning;
    Text txt_warning;

    GameObject o_touchArea;

    private PC_Player_Move PPM;
    private PCPlayerFireArrow PPFA;
    Animator a_PCPlayer;

    [Header("피격 후 무적이 부여될 시간")]
    public float invincibilityTime = 2.0f;
    [Header("무적 초기화 후 시간")]
    public float currentTime = 2.0f;

    [Header("피격 시 화면 진동 시간")]
    [SerializeField] float f_hapticTime = 0.5f;
    [Header("피격 시 화면 진동 세기")]
    [SerializeField] float f_hapticStrength = 0.8f;

    // 무적 시간동안 잠시 감출 스킨드메쉬렌더러
    SkinnedMeshRenderer[] all_child_skinnedMeshRenderer;
    // 무적 시간동안 잠시 감출 스킨드메쉬렌더러
    MeshRenderer[] all_child_meshRenderer;

    private void Start()
    {
        a_PCPlayer = GetComponent<PC_Player_Move>().a_player;
        PPM = GetComponent<PC_Player_Move>();
        PPFA = GetComponent<PCPlayerFireArrow>();
        as_hitPCPlayer = GetComponent<AudioSource>();
        img_warning.SetActive(false);
        txt_warning = img_warning.GetComponentInChildren<Text>();
        txt_warning.text = "";

        all_child_skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        all_child_meshRenderer = GetComponentsInChildren<MeshRenderer>();

        HP = MaxHP;
        hpBar.value = HP / MaxHP;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 태그가 다르면 적용하지 않는다.
        if (!gameObject.CompareTag("PC_Player")) { Debug.LogError("Need Player Tag!!"); return; }
        //if (PPM.isDie == true) { return; }
        // 대포 스위치가 눌리고 올라올 때 속도가 꽤 빠르므로 피격대상에서 제외시킨다.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Turret")) { return; }
        // 리지드바디가 없으면 충돌대상으로 취급하지 않는다.
        if (collision.gameObject.GetComponent<Rigidbody>() == null) { return; }

        // RPC 동기화를 위해 자기 자신에 대해서만 동작한다.
        if (photonView.IsMine)
        {
            float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            if (currentTime >= invincibilityTime)
            {
                // VR플레이어의 손위에 올라갈 때
                if ((collision.gameObject.CompareTag("RightHand") || collision.gameObject.CompareTag("LeftHand")))
                {
                    if (f_objVelocity >= 5f)
                    {
                        // 주먹을 쥐었을 경우, 피격
                        if (collision.gameObject.GetComponent<HandPresence>().gripValue >= 0.5f)
                        {
                            GameObject o_effect = PhotonNetwork.Instantiate("HitEffect2", collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].point, collision.contacts[0].normal));
                            HitPCPlayer_PhotonView(1);
                            currentTime = 0.0f;
                        }
                        // 주먹을 쥐지 않을 경우, 튕겨남 방지
                        else
                        {
                            FunctionForceReducing();
                            photonView.RPC("FunctionForceReducing", RpcTarget.AllBuffered);
                        }
                    }
                    // 매우 작은 진동에도 튕기므로 보험용
                    else
                    {
                        FunctionForceReducing();
                        photonView.RPC("FunctionForceReducing", RpcTarget.AllBuffered);
                    }
                }
                else
                {
                    // 어떠한 오브젝트로 속도가 일정이상 빠르면 플레이어 피격
                    if (f_objVelocity >= 5f)
                    {
                        //Hit_PCPlayer(1);
                        GameObject o_effect = PhotonNetwork.Instantiate("HitEffect2", collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].point, collision.contacts[0].normal));
                        HitPCPlayer_PhotonView(1);
                        FunctionForceReducing();
                        photonView.RPC("FunctionForceReducing", RpcTarget.AllBuffered);
                    }
                }
            }
        }
    }

    Vector3 v3_areaSpawnPosition = Vector3.zero;
    Quaternion q_areaSpawnRotation = Quaternion.identity;

    // 플레이어가 바닥으로 떨어질 경우, 스폰장소로 리스폰
    // 게임 중일 경우 피격 판정
    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                o_touchArea = other.gameObject;
            }

            // 6면에 외벽 설치 + 각 오브젝트가 닿으면 사라지지만 플레이어의 경우, 게임매니저를 참고하여 정해진 위치에 스폰 > 게임성을 위해서는 랜덤 스폰도 괜찮을거 같음
            if (other.gameObject.CompareTag("FallingZone"))
            {
                v3_areaSpawnPosition = Vector3.zero;
                q_areaSpawnRotation = Quaternion.identity;

                for (int i = GameManager.instance.num_destroyArea-1; i < GameManager.instance.o_PlayArea.Count; i++)
                {
                    if (GameManager.instance.o_PlayArea[i] != null)
                    {
                        GameObject la = GameManager.instance.o_PlayArea[i];
                        v3_areaSpawnPosition = (la.transform.position - transform.position);
                        q_areaSpawnRotation = la.transform.rotation;

                        // 이후 지형 추가하면 각각의 이름과 동일한 resource를 찾아서 인스턴스시키는 방향으로 가려함
                        GameObject tmp = PhotonNetwork.Instantiate("RandSet1", GameManager.instance.o_PlayArea[GameManager.instance.num_destroyArea - 1].GetComponent<FractureTest>().tr_spawnPoint.position + v3_areaSpawnPosition, q_areaSpawnRotation);
                        StartCoroutine(DelayedPhotonNetworkDestroy(tmp));
                    }
                }

                if (GameManager.instance.o_PlayArea.Count > GameManager.instance.num_destroyArea)
                {
                    // 싱글턴을 통한 원래 스폰 위치로 복귀
                    transform.position =
                        GameManager.instance.o_PlayArea[GameManager.instance.num_destroyArea - 1].GetComponent<FractureTest>().tr_spawnPoint.position;

                }
                else
                {
                    transform.position =
                        GameManager.instance.o_PlayArea[^1]. // 리스트의 마지막 인덱스는 [^1]로 표시
                        GetComponent<FractureTest>().tr_spawnPoint.position;
                }

                // 반동으로 떨어졌을때 힘 억제
                //gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, gameObject.GetComponent<Rigidbody>().velocity.y, 0);
                //gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                // 떨어질 때도 무적판정 존재, 큰의미는 없음
                if (GameManager.instance.B_GameStart && currentTime >= invincibilityTime)
                {
                    HitPCPlayer_PhotonView(1);
                }
            }
        }
    }

    IEnumerator DelayedPhotonNetworkDestroy(GameObject _o) 
    {
        yield return new WaitForSeconds(3.0f);
        PhotonNetwork.Destroy(_o);
    }

    // 손에서 이탈하고도 너무 많이 튕기는 것을 방지
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() == null) { return; }
        if (photonView.IsMine)
        {
            //float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            //print("PC Player Hit Object Velocity : " + f_objVelocity);

            if (currentTime >= invincibilityTime)
            {
                if ((collision.gameObject.CompareTag("RightHand") || collision.gameObject.CompareTag("LeftHand") || collision.gameObject.CompareTag("VRPlayerHead")))
                {
                    photonView.RPC("FunctionForceReducing", RpcTarget.All);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;

        hpBar.value = HP / MaxHP;

        if(o_touchArea!= null)
        {
            if (o_touchArea.GetComponent<FractureTest>().i_destroyTime <= 10)
            {
                DisplayWarning_On();
            }
            else if (o_touchArea.GetComponent<FractureTest>().i_destroyTime == 100)
            {
                DisplayWarning_Off();
            }
        }

        // 플레이어 피격 치트
        if (Input.GetKeyDown(KeyCode.Alpha1) && !a_PCPlayer.GetBool("IsHit"))
        {
            if (photonView.IsMine)
            {
                photonView.RPC("Hit_PCPlayer", RpcTarget.All, 1);
            }
            //OnSKinMesh();
        }
    }

    // 지형붕괴 시간을 알려주는 경고 문고 및 표지판
    void DisplayWarning_On()
    {
        int inverseCount = o_touchArea.GetComponent<FractureTest>().i_destroyTime;
        img_warning.SetActive(true);
        txt_warning.text =  $"{inverseCount}초 후 지금 있는 땅이\n" + "무너집니다...!";
    }
    void DisplayWarning_Off()
    {
        img_warning.SetActive(false);
        txt_warning.text = "";
    }

    public void HitPCPlayer_PhotonView(int damage)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("Hit_PCPlayer", RpcTarget.AllBuffered, damage);
        }
    }

    [PunRPC]
    public void Hit_PCPlayer(int damage)
    {
        as_hitPCPlayer.Stop();
        as_hitPCPlayer.PlayOneShot(ac_hitPCPlayer);

        gameObject.GetComponent<PCPlayerFireArrow>().B_isReadyToShot = false;
        OnSKinMesh();
        cam_this.DOShakePosition(damage * f_hapticTime, f_hapticStrength);
        HP -= damage;
        Debug.Log($"PC Player {photonView.Controller} is Damaged : Dmg {damage}");

        // 죽음모션, 부활
        if (HP <= 0)
        {
            print("PC 죽음");
            a_PCPlayer.SetBool("IsDie", true);
            Observable.NextFrame().Subscribe(_ => a_PCPlayer.SetBool("IsDie", false));

            PPM.GetComponent<PC_Player_Move>().isDie = true;
            PPFA.GetComponent<PCPlayerFireArrow>().isDie = true;

            if (GameManager.instance.i_PCDeathCount > 0)
            {
                --GameManager.instance.i_PCDeathCount;
                photonView.RPC("CheckRebirthPCPlayer", RpcTarget.All);
                //CheckRebirthPCPlayer();
            }
        }
        // 피격모션
        else
        {
            a_PCPlayer.SetBool("IsHit", true);
            Observable.NextFrame().Subscribe(_ => a_PCPlayer.SetBool("IsHit", false));
        }
        currentTime = 0;
    }

    // VR의 손을 이탈하고 나서 튕기는 힘을 억제
    [PunRPC]
    public void FunctionForceReducing()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    [PunRPC]
    public void OnSKinMesh()
    {
        StartCoroutine(SkinMeshFade());
    }

    // 피격 시 깜빡이는 코루틴
    IEnumerator SkinMeshFade()
    {
        // 데미지를 입으면 무적시간 만큼 깜빡이는 효과, 일단은 2초를 기준으로 짰으나,
        // 가능하면 While문으로 지속효과를 주어도 될듯
        for (int i = 0; i < 10; i++)
        {
            for(int j = 0; j < all_child_skinnedMeshRenderer.Length;j++)
            {
                all_child_skinnedMeshRenderer[j].enabled = false;
            }
            for (int j = 0; j < all_child_meshRenderer.Length; j++)
            {
                all_child_meshRenderer[j].enabled = false;
            }
            yield return new WaitForSeconds(0.1f);
            for (int j = 0; j < all_child_skinnedMeshRenderer.Length; j++)
            {
                all_child_skinnedMeshRenderer[j].enabled = true;
            }
            for (int j = 0; j < all_child_meshRenderer.Length; j++)
            {
                all_child_meshRenderer[j].enabled = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsReading)
        {
        }
        if(stream.IsWriting)
        {
        }
    }
    // PC 플레이어 2초 후 그 자리에서 부활, 애니메이션 초기화를 하지 않을 경우 꼬일 수 있으니 주의!
    // 추가로 움직임을 멈추게 하는 장치 필요
    IEnumerator RebirthPCPlayer()
    {
        // 2초 동안 움직임 방지

        yield return new WaitForSeconds(2f);

        if (!GameManager.instance.B_IsGameOver)
        {
            GetComponent<PC_Player_Move>().a_player.SetBool("Rebirth", true);
            Observable.NextFrame().Subscribe(_ => GetComponent<PC_Player_Move>().a_player.SetBool("Rebirth", false));
            yield return new WaitForSeconds(2f);
            GetComponent<PC_Player_Move>().a_player.SetBool("ReadyNextIdle", true);
            GetComponent<PC_Player_Move>().a_player.Rebind();

            // 일정 시간 무적 부여
            GetComponent<PCPlayerHit>().currentTime = 0;
            GetComponent<PCPlayerHit>().HP = GetComponent<PCPlayerHit>().MaxHP;
            GetComponent<PC_Player_Move>().isDie = false;
            GetComponent<PCPlayerFireArrow>().isDie = false;
        }
    }

    [PunRPC]
    public void CheckRebirthPCPlayer()
    {
        StartCoroutine(RebirthPCPlayer());
    }
}