using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ForceReducingInTree : MonoBehaviourPun
{
    [Header("저항력, 값이 커질수록 저항값이 작아짐")]
    //[SerializeField] float f_forceReducingValue = 1.5f;

    Rigidbody rb_this;

    void Start()
    {
        rb_this = GetComponent<Rigidbody>();
    }

    //void Update()
    //{
    //    Vector3 v3_velocity = rb_this.velocity;
    //    if(v3_velocity.magnitude > 100f)
    //    {
    //        //rb_this.velocity = Vector3.zero;
    //        rb_this.velocity -= v3_velocity / f_forceReducingValue;
    //    }
    //    else if (v3_velocity.magnitude < 10f)
    //    {
    //        rb_this.velocity += v3_velocity * f_forceReducingValue;
    //    }

    //}
}
