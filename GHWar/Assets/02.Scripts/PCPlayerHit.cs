using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UniRx;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using Photon.Pun.Demo.PunBasics;

// PC플레이어 최상단에 넣는다.
// 컬라이더를 넣고, 태그를 PC_Player로 바꾼다.
public class PCPlayerHit : MonoBehaviourPunCallbacks
{
    PhotonView pv;

    [Header("Max HP")]
    public float MaxHP = 2.0f;
    [Header("HP")]
    public float HP = 2.0f;

    [Header("HP 슬라이더바")]
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
                HP = MaxHP;
                GameManager.instance.CheckRebirthPCPlayer();
                //GameManager.instance.i_PCDeathCount--;
                PPM.GetComponent<PC_Player_Move>().isDie = false;
                PPFA.GetComponent<PCPlayerFireArrow>().isDie = false;
                print(GameManager.instance.i_PCDeathCount);
                //gameObject.GetComponent<PCPlayerLife>().HeartBreak();
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
//        //    print("PC 히트");
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
//        // HP 동기화 부분
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
//            print("PC 죽음");
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
//            print("데이터 전송 받음");
//            tmpHP = (float)stream.ReceiveNext();
//            print("데이터 전송 받은 후");
//        }
//        if(stream.IsWriting)
//        {
//            print("데이터 전송");
//            stream.SendNext(this.HP);
//            print("데이터 전송 후");
//        }
//    }
//}using System.Collections;using System.Collections;