using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TransferInteractionMaster : MonoBehaviour
{
    PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.GetComponent<PhotonView>().IsMine)
        {
            pv.RequestOwnership();
        }
    }
}
