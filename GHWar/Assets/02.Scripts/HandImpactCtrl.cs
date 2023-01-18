using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class HandImpactCtrl : MonoBehaviourPunCallbacks
{
    [SerializeField] XRController controller_xr;
    [SerializeField] bool b_checkFist = false;

    [SerializeField] ParticleSystem ps_attackGroundEffect;
    [SerializeField] GameObject o_Stone;

    PhotonView pv;
    Rigidbody rb_this;
    float f_fistVelocity;

    private void OnCollisionEnter(Collision collision)
    {
        if(
            (pv.IsMine)
            && (b_checkFist)
            && (collision.gameObject.CompareTag("Ground")
            && (f_fistVelocity)>10)
            )
        {
            print("충돌은 함");
            Vector3 v3_hitPosition = collision.contacts[0].point;
            Quaternion q_hitNormal = Quaternion.Euler(collision.contacts[0].normal);
            AttackGround(v3_hitPosition, q_hitNormal);
            pv.RPC("AttackGround", RpcTarget.All, v3_hitPosition, q_hitNormal);
        }
    }

    void Start()
    {
        pv= GetComponent<PhotonView>();
        rb_this = GetComponent<Rigidbody>();
    }

    void Update()
    {
        f_fistVelocity = rb_this.velocity.magnitude;

        if(controller_xr.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool isFist))
        {
            b_checkFist = isFist;
        }
    }

    [PunRPC]
    public void AttackGround(Vector3 hitPosition, Quaternion hitNormal)
    {
        Instantiate(ps_attackGroundEffect, hitPosition, hitNormal);
        Vector3 randomPosition = hitPosition + new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-20f, 20f));
        Instantiate(o_Stone, randomPosition, hitNormal);
        randomPosition = hitPosition + new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-20f, 20f));
        Instantiate(o_Stone, randomPosition, hitNormal);
    }
}
