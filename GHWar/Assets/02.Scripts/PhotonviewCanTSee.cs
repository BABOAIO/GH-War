using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonviewCanTSee : MonoBehaviourPun
{
    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            this.gameObject.SetActive(false);
        }
    }
}
