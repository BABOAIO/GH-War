using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UniRx;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using Photon.Pun.Demo.PunBasics;

// PC�÷��̾� �ֻ�ܿ� �ִ´�.
// �ö��̴��� �ְ�, �±׸� PC_Player�� �ٲ۴�.
public class PCPlayerHit : MonoBehaviourPunCallbacks
{
    PhotonView pv;

    [Header("Max HP")]
    public float MaxHP = 2.0f;
    [Header("HP")]
    public float HP = 2.0f;

    [Header("HP �����̴���")]
    [SerializeField] Slider hpBar;

    private PC_Player_Move PPM;
    private PCPlayerFireArrow PPFA;
    Animator a_PCPlayer;

    float invincibilityTime = 2.0f;
    public float currentTime = 2.0f;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        a_PCPlayer = GetComponent<PC_Player_Move>().a_player;
        PPM = GetComponent<PC_Player_Move>();
        PPFA = GetComponent<PCPlayerFireArrow>();

        HP = MaxHP;
        hpBar.value = HP / MaxHP;
    }

    private void OnCollisionEnter(Collision collision)
    {
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

    // �÷��̾ �ٴ����� ������ ���, ������ҷ� ������
    // ���� ���� ��� �ǰ� ����
    private void OnTriggerEnter(Collider other)
    {
        if (pv.IsMine)
        {
            if (other.gameObject.CompareTag("FallingZone"))
            {
                // �̱����� ���� ���� ���� ��ġ�� ����
                transform.position = ConnManager.Conn.PC_Spawn;

                // �ݵ����� ���������� �� ����
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                currentTime = 0.0f;

                if (GameManager.instance.B_GameStart)
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

    }


    [PunRPC]
    public void Hit_PCPlayer(int damage)
    {
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
                HP = MaxHP;
                GameManager.instance.CheckRebirthPCPlayer();
                //GameManager.instance.i_PCDeathCount--;
                PPM.GetComponent<PC_Player_Move>().isDie = false;
                PPFA.GetComponent<PCPlayerFireArrow>().isDie = false;
                print(GameManager.instance.i_PCDeathCount);
                //gameObject.GetComponent<PCPlayerLife>().HeartBreak();
            }
        }
        // �ǰݸ��
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

//using System.Collections;
 //using System.Collections.Generic;
 //using UnityEngine;
 //using Photon.Realtime;
 //using Photon.Pun;
 //using UniRx;

//public class PCPlayerHit : MonoBehaviourPunCallbacks, IPunObservable
//{
//    PhotonView pv;

//    [Header("Max HP")]
//    public float MaxHP = 2.0f;
//    [Header("HP")]
//    public float HP = 2.0f;

//    float tmpHP = 2.0f;

//    Animator a_PCPlayer;

//    float invincibilityTime = 2.0f;
//    public float currentTime = 2.0f;

//    private void Start()
//    {
//        pv = GetComponent<PhotonView>();
//        a_PCPlayer = GetComponent<PC_Player_Move>().a_player;
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        if(collision.gameObject.GetComponent<Rigidbody>() == null) { return; }

//        //float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
//        //print("PC Player Hit Object Velocity : " + f_objVelocity);
//        //if (f_objVelocity >= 10f && currentTime >= invincibilityTime)
//        //{
//        //    print("PC ��Ʈ");
//        //    Hit_PCPlayer(1);
//        //    pv.RPC("Hit_PCPlayer", RpcTarget.AllBuffered, 1);
//        //    currentTime = 0.0f;
//        //}

//        if (pv.IsMine)
//        {
//            float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
//            print("PC Player Hit Object Velocity : " + f_objVelocity);
//            if (f_objVelocity >= 10f && currentTime >= invincibilityTime)
//            {
//                Hit_PCPlayer(1);
//                //pv.RPC("Hit_PCPlayer", RpcTarget.AllBuffered, 1);
//                currentTime = 0.0f;
//            }
//        }
//    }

//    private void FixedUpdate()
//    {
//        currentTime += Time.fixedDeltaTime;

//    }

//    private void Update()
//    {
//        // HP ����ȭ �κ�
//        if (pv.IsMine)
//        {

//        }
//        else
//        {
//            HP = tmpHP;
//        }

//    }

//    [PunRPC]
//    public void Hit_PCPlayer(int damage)
//    {
//        HP -= damage;
//        Debug.Log($"PC Player {pv.Controller} is Damaged : Dmg {damage}");

//        if (HP <= 0)
//        {
//            print("PC ����");
//            if (pv.IsMine)
//            {
//                a_PCPlayer.SetBool("IsDie", true);
//                Observable.NextFrame().Subscribe(_ => a_PCPlayer.SetBool("IsDie", false));
//                GameManager.instance.CheckRebirthPCPlayer();
//            }
//            //HP = MaxHP;
//        }
//    }

//    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//    {
//        if(stream.IsReading)
//        {
//            print("������ ���� ����");
//            tmpHP = (float)stream.ReceiveNext();
//            print("������ ���� ���� ��");
//        }
//        if(stream.IsWriting)
//        {
//            print("������ ����");
//            stream.SendNext(this.HP);
//            print("������ ���� ��");
//        }
//    }
//}using System.Collections;using System.Collections;