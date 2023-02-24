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
    public bool IsGrab = false;
    XRGrabInteractionPun _xr;

    private void Start()
    {
        IsVR = GameManager.instance.IsVR;
        if (!IsVR)
        {
            _xr = GetComponent<XRGrabInteractionPun>();
        }
        else
        {
            _xr = null;
        }
    }

    private void FixedUpdate()
    {
        if(_xr != null)
        {
            if (!IsGrab)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    if (photonView.ControllerActorNr != photonView.CreatorActorNr)
                    {
                        print("PCPlayer제거");
                        PhotonNetwork.Destroy(this.gameObject);
                    }
                }
            }
        }
        if (_xr == null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (photonView.ControllerActorNr != photonView.CreatorActorNr)
                {
                    print("VRPlayer제거");
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
        }
    }

}
