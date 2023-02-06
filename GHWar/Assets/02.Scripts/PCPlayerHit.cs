using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UniRx;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using Photon.Pun.Demo.PunBasics;

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
        if(collision.gameObject.layer == LayerMask.NameToLayer("Turret")) { return; }
        if (collision.gameObject.GetComponent<Rigidbody>() == null) { return; }
        if (pv.IsMine)
        {
            float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            //print("PC Player Hit Object Velocity : " + f_objVelocity);

            if (currentTime >= invincibilityTime)
            {
                if ((collision.gameObject.CompareTag("RightHand") || collision.gameObject.CompareTag("LeftHand")))
                {
                    if (f_objVelocity >= 5f)
                    {
                        if (collision.gameObject.GetComponent<HandPresence>().gripValue >= 0.5f)
                        {
                            pv.RPC("Hit_PCPlayer", RpcTarget.AllBuffered, 1);
                            currentTime = 0.0f;
                        }
                        else
                        {
                            pv.RPC("FunctionForceReducing", RpcTarget.AllBuffered);
                        }
                    }
                    else
                    {
                        pv.RPC("FunctionForceReducing", RpcTarget.AllBuffered);
                    }
                }
                else
                {
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
                GameManager.instance.i_PCDeathCount--;
                PPM.GetComponent<PC_Player_Move>().isDie = false;
                PPFA.GetComponent<PCPlayerFireArrow>().isDie = false;
            }
        }
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