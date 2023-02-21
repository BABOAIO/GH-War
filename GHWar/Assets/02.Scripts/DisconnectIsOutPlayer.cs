using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DisconnectIsOutPlayer : MonoBehaviourPunCallbacks
{
    XRGrabInteractionPun _XRGrabInteractionPun;

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (photonView.IsMine)
        {
            if (photonView.ControllerActorNr != photonView.CreatorActorNr)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }

    private void Start()
    {
        _XRGrabInteractionPun = GetComponent<XRGrabInteractionPun>();    
    }
    private void Update()
    {
        if (_XRGrabInteractionPun.isGrab) return;
    }

}
