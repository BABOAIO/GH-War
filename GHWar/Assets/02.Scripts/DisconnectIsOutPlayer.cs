using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DisconnectIsOutPlayer : MonoBehaviourPunCallbacks
{
    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (photonView.ControllerActorNr != photonView.CreatorActorNr)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
        //PhotonNetwork.Destroy(this.gameObject);

        //if (photonView.IsMine)
        //{
        //    if (photonView.ControllerActorNr != photonView.CreatorActorNr)
        //    {
        //        PhotonNetwork.Destroy(this.gameObject);
        //    }
        //}
    }

}
