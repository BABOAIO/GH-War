using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeadFollowCam : MonoBehaviourPun
{
    [SerializeField] Transform tr_cam;
    Transform tr_this;

    void Start()
    {
        tr_this = GetComponent<Transform>();
    }

    void Update()
    {
        if(photonView.IsMine)
        {
            tr_this.position = tr_cam.position;
            tr_this.rotation = tr_cam.rotation;
        }
    }
}
