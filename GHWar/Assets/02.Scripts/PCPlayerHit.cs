using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UniRx;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using OVR.OpenVR;

// PC플레이어 최상단에 넣는다.
// 컬라이더를 넣고, 태그를 PC_Player로 바꾼다.
public class PCPlayerHit : MonoBehaviourPunCallbacks
{
    PhotonView pv;

    [SerializeField] Camera cam_this;

    [Header("Max HP")]
    public float MaxHP = 2.0f;
    [Header("HP")]
    public float HP = 2.0f;

    [Header("HP 슬라이더바")]
    [SerializeField] Slider hpBar;
    [Header("경고 이미지")]
    [SerializeField] GameObject img_warning;
    Text txt_warning;

    GameObject o_touchArea;

    private PC_Player_Move PPM;
    private PCPlayerFireArrow PPFA;
    Animator a_PCPlayer;

    float invincibilityTime = 2.0f;
    public float currentTime = 2.0f;

    [SerializeField] float f_hapticTime = 0.5f;
    [SerializeField] float f_hapticStrength = 0.8f;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        a_PCPlayer = GetComponent<PC_Player_Move>().a_player;
        PPM = GetComponent<PC_Player_Move>();
        PPFA = GetComponent<PCPlayerFireArrow>();
        img_warning.SetActive(false);
        txt_warning = img_warning.GetComponentInChildren<Text>();
        txt_warning.text = "";

        HP = MaxHP;
        hpBar.value = HP / MaxHP;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 태그가 다르면 적용하지 않는다.
        if (!gameObject.CompareTag("PC_Player")) { Debug.LogError("Need Player Tag!!"); return; }
        if (PPM.GetComponent<PC_Player_Move>().isDie == true) { return; }
        // 대포 스위치가 눌리고 올라올 때 속도가 꽤 빠르므로 피격대상에서 제외시킨다.
        if(collision.gameObject.layer == LayerMask.NameToLayer("Turret")) { return; }
        // 리지드바디가 없으면 충돌대상으로 취급하지 않는다.
        if (collision.gameObject.GetComponent<Rigidbody>() == null) { return; }

        // RPC 동기화를 위해 자기 자신에 대해서만 동작한다.
        if (pv.IsMine)
        {
            float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            //print("PC Player Hit Object Velocity : " + f_objVelocity);

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
                            pv.RPC("Hit_PCPlayer", RpcTarget.AllBuffered, 1);
                            currentTime = 0.0f;
                        }
                        // 주먹을 쥐지 않을 경우, 튕겨남 방지
                        else
                        {
                            pv.RPC("FunctionForceReducing", RpcTarget.AllBuffered);
                        }
                    }
                    // 매우 작은 진동에도 튕기므로 보험용
                    else
                    {
                        pv.RPC("FunctionForceReducing", RpcTarget.AllBuffered);
                    }
                }
                else
                {
                    // 어떠한 오브젝트로 속도가 일정이상 빠르면 플레이어 피격
                    if (f_objVelocity >= 5f)
                    {
                        //Hit_PCPlayer(1);
                        pv.RPC("Hit_PCPlayer", RpcTarget.AllBuffered, 1);
                        currentTime = 0.0f;
                    }
                }
            }
        }
    }

    // 플레이어가 바닥으로 떨어질 경우, 스폰장소로 리스폰
    // 게임 중일 경우 피격 판정
    private void OnTriggerEnter(Collider other)
    {
        if (pv.IsMine)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                print(other.name);
                o_touchArea = other.gameObject;
            }

            if (other.gameObject.CompareTag("FallingZone"))
            {
                // 싱글턴을 통한 원래 스폰 위치로 복귀
                transform.position = ConnManager.Conn.PC_Spawn;

                // 반동으로 떨어졌을때 힘 억제
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                currentTime = 0.0f;

                if (GameManager.instance.B_GameStart)
                {
                    pv.RPC("Hit_PCPlayer", RpcTarget.AllBuffered, 1);
                }
            }
        }
    }

    // 손에서 이탈하고도 너무 많이 튕기는 것을 방지
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() == null) { return; }
        if (pv.IsMine)
        {
            float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            //print("PC Player Hit Object Velocity : " + f_objVelocity);

            if (currentTime >= invincibilityTime)
            {
                if ((collision.gameObject.CompareTag("RightHand") || collision.gameObject.CompareTag("LeftHand")))
                {
                    pv.RPC("FunctionForceReducing", RpcTarget.AllBuffered);
                }
            }
        }
    }

    int count = 0;

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
        //if (Input.GetKeyDown(KeyCode.Alpha1) && !a_PCPlayer.GetBool("IsHit"))
        //{
        //    Hit_PCPlayer(1);
        //}
    }

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

    [PunRPC]
    public void Hit_PCPlayer(int damage)
    {
        cam_this.DOShakePosition(damage * f_hapticTime, f_hapticStrength);
        HP -= damage;
        Debug.Log($"PC Player {pv.Controller} is Damaged : Dmg {damage}");

        // 죽음모션, 부활
        if (HP <= 0)
        {
            print("PC 죽음");
            a_PCPlayer.SetBool("IsDie", true);
            Observable.NextFrame().Subscribe(_ => a_PCPlayer.SetBool("IsDie", false));

            PPM.GetComponent<PC_Player_Move>().isDie = true;
            PPFA.GetComponent<PCPlayerFireArrow>().isDie = true;

            if (GameManager.instance.i_PCDeathCount >= 1)
            {
                --GameManager.instance.i_PCDeathCount;
                PPM.GetComponent<PC_Player_Move>().isDie = false;
                PPFA.GetComponent<PCPlayerFireArrow>().isDie = false;
                GameManager.instance.CheckRebirthPCPlayer();
            }
        }
        // 피격모션
        else
        {
            a_PCPlayer.SetBool("IsHit", true);
            Observable.NextFrame().Subscribe(_ => a_PCPlayer.SetBool("IsHit", false));
        }
    }

    [PunRPC]
    public void FunctionForceReducing()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

}