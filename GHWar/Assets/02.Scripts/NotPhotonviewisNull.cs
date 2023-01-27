using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NotPhotonviewisNull : MonoBehaviour
{
    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(this.gameObject.activeSelf) { return; }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
