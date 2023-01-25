using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ArrowProperty : MonoBehaviour
{
    [SerializeField] float shotPower = 200.0f;
    Rigidbody rb_this;
    Transform tr_this;

    void Awake()
    {
        rb_this.GetComponent<Rigidbody>();
        tr_this = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        rb_this.AddForce(new Vector3(0, 0, shotPower));
    }

    void FixedUpdate()
    {
        
    }
}
