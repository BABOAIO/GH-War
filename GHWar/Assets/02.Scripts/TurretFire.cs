using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurretFire : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject cannonBall;

    [SerializeField] float f_shotPower = 50f;
    Animation anim_this;

    GameObject o_target;
    Transform tr_this;

    void Start()
    {
        anim_this = GetComponent<Animation>();
        o_target = GameObject.FindGameObjectWithTag("VRPlayerHead");
        tr_this = GetComponent<Transform>();
    }

    void Update()
    {
        LookVRPlayer();
    }

    [PunRPC]
    void LookVRPlayer()
    {
        if (o_target != null) { o_target = GameObject.FindGameObjectWithTag("VRPlayerHead"); }

        Vector3 v3_direction = o_target.transform.position - tr_this.position;
        transform.rotation = Quaternion.LookRotation(v3_direction.normalized);
    }

    public void AnimationEvnet()
    {

        //GameObject ball = Instantiate(cannonBall, transform.position + transform.forward * 0.5f, transform.rotation);

        //GameObject ball = PhotonNetwork.Instantiate("CannonBall", transform.position + transform.forward * 0.5f, transform.rotation);
        //ball.GetComponent<Rigidbody>().AddForce(transform.forward * f_shotPower, ForceMode.Impulse);
    }
}
