using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DisconnectIsOutPlayer : MonoBehaviourPunCallbacks
{
    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            if (photonView.CreatorActorNr == 0)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }

}
