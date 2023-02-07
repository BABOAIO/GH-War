using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 플레이어에게 넣는다. 너무 멀리 날라가버리는 불상사 방지
public class ForceReducing : MonoBehaviourPun
{
    [Header("저항력, 값이 커질수록 저항값이 작아짐")]
    [SerializeField] float f_forceReducingValue = 1.5f;

    Rigidbody rb_this;

    void Start()
    {
        rb_this = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 v3_velocity = rb_this.velocity;
        if(v3_velocity.magnitude > 30f)
        {
            //rb_this.velocity = Vector3.zero;
            rb_this.velocity -= v3_velocity / f_forceReducingValue;
        }

    }
}
