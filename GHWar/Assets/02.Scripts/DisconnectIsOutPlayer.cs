using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DisconnectIsOutPlayer : MonoBehaviourPun
{
    private void Update()
    {
        if (photonView.IsMine)
        {
            if(photonView.ControllerActorNr != photonView.CreatorActorNr)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }   
    }

}
