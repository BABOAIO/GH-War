using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DisconnectIsOutPlayer : MonoBehaviourPunCallbacks
{
    XRGrabInteractionPun _XRGrabInteractionPun;

    private void Start()
    {
        _XRGrabInteractionPun = GetComponent<XRGrabInteractionPun>();    
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if(photonView.IsMine)
        {
            photonView.RPC("OnDisconnect", RpcTarget.All);
        }   
    }

    [PunRPC]
    void OnDisconnect()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    //private void FixedUpdate()
    //{
    //    if (_XRGrabInteractionPun.isGrab) return;
    //    if (photonView.IsMine)
    //    {
    //        if(photonView.ControllerActorNr != photonView.CreatorActorNr)
    //        {
    //            PhotonNetwork.Destroy(this.gameObject);
    //        }
    //    }   
    //}

}
