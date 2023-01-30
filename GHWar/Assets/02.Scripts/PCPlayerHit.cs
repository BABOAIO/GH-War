using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UniRx;

public class PCPlayerHit : MonoBehaviourPunCallbacks
{
    PhotonView pv;

    [Header("Max HP")]
    public float MaxHP = 2.0f;
    [Header("HP")]
    public float HP = 2.0f;

    // �� �ۺ��ΰ�...?
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() == null) { return; }

        //float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        //print("PC Player Hit Object Velocity : " + f_objVelocity);
        //if (f_objVelocity >= 10f && currentTime >= invincibilityTime)
        //{
        //    print("PC ��Ʈ");
        //    Hit_PCPlayer(1);
        //    pv.RPC("Hit_PCPlayer", RpcTarget.AllBuffered, 1);
        //    currentTime = 0.0f;
        //}

        if (pv.IsMine)
        {
            float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            print("PC Player Hit Object Velocity : " + f_objVelocity);
            if (f_objVelocity >= 10f && currentTime >= invincibilityTime)
            {
                //Hit_PCPlayer(1);
                pv.RPC("Hit_PCPlayer", RpcTarget.AllBuffered, 1);
                currentTime = 0.0f;
            }
        }
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;

    }


    [PunRPC]
    public void Hit_PCPlayer(int damage)
    {
        HP -= damage;
        Debug.Log($"PC Player {pv.Controller} is Damaged : Dmg {damage}");

        if (HP <= 0)
        {
            print("PC ����");
            a_PCPlayer.SetBool("IsDie", true);
            Observable.NextFrame().Subscribe(_ => a_PCPlayer.SetBool("IsDie", false));
            GameManager.instance.CheckRebirthPCPlayer();

            PPM.GetComponent<PC_Player_Move>().isDie = true;
            PPFA.GetComponent<PCPlayerFireArrow>().isDie = true;
            //HP = MaxHP;
        }
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