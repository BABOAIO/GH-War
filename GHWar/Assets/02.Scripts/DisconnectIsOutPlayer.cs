using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DisconnectIsOutPlayer : MonoBehaviourPun
{
    XRGrabInteractionPun _XRGrabInteractionPun;

    private void Start()
    {
        _XRGrabInteractionPun = GetComponent<XRGrabInteractionPun>();    
    }
    private void Update()
    {
        if (_XRGrabInteractionPun.isGrab) return;
        if (photonView.IsMine)
        {
            if(photonView.ControllerActorNr != photonView.CreatorActorNr)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }   
    }

}
