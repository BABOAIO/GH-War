using System.Collections;
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

    Animator a_PCPlayer;

    float invincibilityTime = 2.0f;
    public float currentTime = 2.0f;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        a_PCPlayer = GetComponent<PC_Player_Move>().a_player;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Rigidbody>() == null) { return; }

        float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        print("PC Player Hit Object Velocity : " + f_objVelocity);
        if (f_objVelocity >= 10f && currentTime >= invincibilityTime)
        {
            print("PC È÷Æ®");
            Hit_PCPlayer(1);
            //pv.RPC("Hit_PCPlayer", RpcTarget.AllBuffered, 1);
            currentTime = 0.0f;
        }

        if (pv.IsMine)
        {
        }
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
    }

    [PunRPC]
    public void Hit_PCPlayer(float damage)
    {
        HP -= damage;
        Debug.Log($"PC Player {pv.Controller} is Damaged : Dmg {damage}");

        if (HP <= 0)
        {
            HP = MaxHP;
            a_PCPlayer.SetBool("IsDie", true);
            Observable.NextFrame().Subscribe(_ => a_PCPlayer.SetBool("IsDie", false));
            GameManager.instance.CheckRebirthPCPlayer();
        }
    }
}
