using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurretFire : MonoBehaviourPunCallbacks
{
    [SerializeField] float f_shotPower = 50f;
    Animation anim_this;

    Transform tr_target;

    void Start()
    {
        anim_this = GetComponent<Animation>();
        tr_target = GameObject.FindGameObjectWithTag("VRPlayerHead").transform;
    }

    void Update()
    {
        if (tr_target != null) { tr_target = GameObject.FindGameObjectWithTag("VRPlayerHead").transform; }

        Vector3 v3_direction = tr_target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(v3_direction.normalized);
    }

    public void AnimationEvnet()
    {
        GameObject ball = PhotonNetwork.Instantiate("CannonBall", transform.position + transform.forward * 0.5f, transform.rotation);
        ball.GetComponent<Rigidbody>().AddForce(transform.forward * f_shotPower, ForceMode.Impulse);
    }
}
