using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using System.Reflection;
using System;

public class TurretManager : MonoBehaviourPunCallbacks
{
    [SerializeField] float f_threshold = 0.1f;
    [SerializeField] float f_deadZone = 0.025f;
    [SerializeField] GameObject o_button;

    Transform tr_this;

    bool b_isPresse;
    Vector3 v3_originPos;

    public UnityEvent OnPressButton_Turret, OnReleaseButton_Turret;

    void Start()
    {
        tr_this = GetComponent<Transform>();
        v3_originPos= tr_this.localPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        b_isPresse = true;
        tr_this.Translate(Vector3.down * 0.05f, Space.Self);
        OnPressButton_Turret.Invoke();
        Debug.Log("Turret Active!");
    }
    private void OnCollisionExit(Collision collision)
    {
        b_isPresse = false;
        tr_this.position = v3_originPos;
        OnReleaseButton_Turret.Invoke();
        Debug.Log("Turret Deactive..");
    }
}
