using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UniRx;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using OVR.OpenVR;

// PC�÷��̾� �ֻ�ܿ� �ִ´�.
// �ö��̴��� �ְ�, �±׸� PC_Player�� �ٲ۴�.
public class PCPlayerHit : MonoBehaviourPunCallbacks
{
    PhotonView pv;

    AudioSource as_hitPCPlayer;
    [SerializeField] AudioClip ac_hitPCPlayer;

    [SerializeField] Camera cam_this;

    [Header("Max HP")]
    public float MaxHP = 2.0f;
    [Header("HP")]
    public float HP = 2.0f;

    [Header("HP �����̴���")]
    [SerializeField] Slider hpBar;
    [Header("��� �̹���")]
    [SerializeField] GameObject img_warning;
    Text txt_warning;

    GameObject o_touchArea;

    private PC_Player_Move PPM;
    private PCPlayerFireArrow PPFA;
    Animator a_PCPlayer;

    [Header("�ǰ� �� ������ �ο��� �ð�")]
    public float invincibilityTime = 2.0f;
    [Header("���� �ʱ�ȭ �� �ð�")]
    public float currentTime = 2.0f;

    [Header("�ǰ� �� ȭ�� ���� �ð�")]
    [SerializeField] float f_hapticTime = 0.5f;
    [Header("�ǰ� �� ȭ�� ���� ����")]
    [SerializeField] float f_hapticStrength = 0.8f;

    // ���� �ð����� ��� ���� ��Ų��޽�������
    SkinnedMeshRenderer[] all_child_skinnedMeshRenderer;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        a_PCPlayer = GetComponent<PC_Player_Move>().a_player;
        PPM = GetComponent<PC_Player_Move>();
        PPFA = GetComponent<PCPlayerFireArrow>();
        as_hitPCPlayer = GetComponent<AudioSource>();
        img_warning.SetActive(false);
        txt_warning = img_warning.GetComponentInChildren<Text>();
        txt_warning.text = "";

        all_child_skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();

        HP = MaxHP;
        hpBar.value = HP / MaxHP;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �±װ� �ٸ��� �������� �ʴ´�.
        if (!gameObject.CompareTag("PC_Player")) { Debug.LogError("Need Player Tag!!"); return; }
        if (PPM.GetComponent<PC_Player_Move>().isDie == true) { return; }
        // ���� ����ġ�� ������ �ö�� �� �ӵ��� �� �����Ƿ� �ǰݴ�󿡼� ���ܽ�Ų��.
        if(collision.gameObject.layer == LayerMask.NameToLayer("Turret")) { return; }
        // ������ٵ� ������ �浹������� ������� �ʴ´�.
        if (collision.gameObject.GetComponent<Rigidbody>() == null) { return; }

        // RPC ����ȭ�� ���� �ڱ� �ڽſ� ���ؼ��� �����Ѵ�.
        if (pv.IsMine)
        {
            float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            //print("PC Player Hit Object Velocity : " + f_objVelocity);

            if (currentTime >= invincibilityTime)
            {
                // VR�÷��̾��� ������ �ö� ��
                if ((collision.gameObject.CompareTag("RightHand") || collision.gameObject.CompareTag("LeftHand")))
                {
                    if (f_objVelocity >= 5f)
                    {
                        // �ָ��� ����� ���, �ǰ�
                        if (collision.gameObject.GetComponent<HandPresence>().gripValue >= 0.5f)
                        {
                            pv.RPC("Hit_PCPlayer", RpcTarget.AllBuffered, 1);
                            currentTime = 0.0f;
                        }
                        // �ָ��� ���� ���� ���, ƨ�ܳ� ����
                        else
                        {
                            pv.RPC("FunctionForceReducing", RpcTarget.AllBuffered);
                        }
                    }
                    // �ſ� ���� �������� ƨ��Ƿ� �����
                    else
                    {
                        pv.RPC("FunctionForceReducing", RpcTarget.AllBuffered);
                    }
                }
                else
                {
                    // ��� ������Ʈ�� �ӵ��� �����̻� ������ �÷��̾� �ǰ�
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

    Vector3 v3_areaSpawnPosition = Vector3.zero;
    Quaternion q_areaSpawnRotation = Quaternion.identity;

    // �÷��̾ �ٴ����� ������ ���, ������ҷ� ������
    // ���� ���� ��� �ǰ� ����
    private void OnTriggerEnter(Collider other)
    {
        if (pv.IsMine)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                o_touchArea = other.gameObject;
            }

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

                        GameObject tmp = PhotonNetwork.Instantiate("RandSet", GameManager.instance.o_PlayArea[GameManager.instance.num_destroyArea - 1].GetComponent<FractureTest>().tr_spawnPoint.position + v3_areaSpawnPosition, q_areaSpawnRotation);
                        Destroy(tmp, 3.0f);
                    }
                }

                if (GameManager.instance.o_PlayArea.Count > GameManager.instance.num_destroyArea)
                {
                    // �̱����� ���� ���� ���� ��ġ�� ����
                    transform.position =
                        GameManager.instance.o_PlayArea[GameManager.instance.num_destroyArea - 1].GetComponent<FractureTest>().tr_spawnPoint.position;

                }
                else
                {
                    transform.position =
                        GameManager.instance.o_PlayArea[^1]. // ����Ʈ�� ������ �ε����� [^1]�� ǥ��
                        GetComponent<FractureTest>().tr_spawnPoint.position;
                }

                // �ݵ����� ���������� �� ����
                gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, gameObject.GetComponent<Rigidbody>().velocity.y, 0);

                // ������ ���� �������� ����, ū�ǹ̴� ����
                if (GameManager.instance.B_GameStart && currentTime >= invincibilityTime)
                {
                    pv.RPC("Hit_PCPlayer", RpcTarget.AllBuffered, 1);
                }
            }
        }
    }

    // �տ��� ��Ż�ϰ� �ʹ� ���� ƨ��� ���� ����
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

        // �÷��̾� �ǰ� ġƮ
        if (Input.GetKeyDown(KeyCode.Alpha1) && !a_PCPlayer.GetBool("IsHit"))
        {
            Hit_PCPlayer(1);
            //OnSKinMesh();
        }
    }

    // �����ر� �ð��� �˷��ִ� ��� ���� �� ǥ����
    void DisplayWarning_On()
    {
        int inverseCount = o_touchArea.GetComponent<FractureTest>().i_destroyTime;
        img_warning.SetActive(true);
        txt_warning.text =  $"{inverseCount}�� �� ���� �ִ� ����\n" + "�������ϴ�...!";
    }
    void DisplayWarning_Off()
    {
        img_warning.SetActive(false);
        txt_warning.text = "";
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
        Debug.Log($"PC Player {pv.Controller} is Damaged : Dmg {damage}");

        // �������, ��Ȱ
        if (HP <= 0)
        {
            print("PC ����");
            a_PCPlayer.SetBool("IsDie", true);
            Observable.NextFrame().Subscribe(_ => a_PCPlayer.SetBool("IsDie", false));

            PPM.GetComponent<PC_Player_Move>().isDie = true;
            PPFA.GetComponent<PCPlayerFireArrow>().isDie = true;

            if (GameManager.instance.i_PCDeathCount > 0)
            {
                --GameManager.instance.i_PCDeathCount;
                PPM.GetComponent<PC_Player_Move>().isDie = false;
                PPFA.GetComponent<PCPlayerFireArrow>().isDie = false;
                GameManager.instance.CheckRebirthPCPlayer();
            }
        }
        // �ǰݸ��
        else
        {
            a_PCPlayer.SetBool("IsHit", true);
            Observable.NextFrame().Subscribe(_ => a_PCPlayer.SetBool("IsHit", false));
        }
    }

    // VR�� ���� ��Ż�ϰ� ���� ƨ��� ���� ����
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

    // �ǰ� �� �����̴� �ڷ�ƾ
    IEnumerator SkinMeshFade()
    {
        // �������� ������ �����ð� ��ŭ �����̴� ȿ��, �ϴ��� 2�ʸ� �������� ®����,
        // �����ϸ� While������ ����ȿ���� �־ �ɵ�
        for (int i = 0; i < 10; i++)
        {
            for(int j = 0; j < all_child_skinnedMeshRenderer.Length;j++)
            {
                all_child_skinnedMeshRenderer[j].enabled = false;
            }
            yield return new WaitForSeconds(0.1f);
            for (int j = 0; j < all_child_skinnedMeshRenderer.Length; j++)
            {
                all_child_skinnedMeshRenderer[j].enabled = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}