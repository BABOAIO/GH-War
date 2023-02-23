using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// 플레이어가 2명 이상인 경우, 한명이 Disconnect를 할 때 플레이어의 포톤뷰가 마스터 클라이언트로 넘어간다.
// 나간 플레이어를 없애기 위한 스크립트, 플레이어들에게 넣는다.
public class DisconnectIsOutPlayer : MonoBehaviourPunCallbacks
{
    bool IsVR;
    bool IsGrab
    {
        get { return GetComponent<XRGrabInteractionPun>().isGrab; }
        set
        {
            value = GetComponent<XRGrabInteractionPun>().isGrab;
        }
    }

    private void Start()
    {
        IsVR = GameManager.instance.IsVR;
    }

    private void FixedUpdate()
    {
        if (!IsGrab)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (photonView.ControllerActorNr != photonView.CreatorActorNr)
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
        }
    }

}
