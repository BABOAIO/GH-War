using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// �÷��̾ 2�� �̻��� ���, �Ѹ��� Disconnect�� �� �� �÷��̾��� ����䰡 ������ Ŭ���̾�Ʈ�� �Ѿ��.
// ���� �÷��̾ ���ֱ� ���� ��ũ��Ʈ, �÷��̾�鿡�� �ִ´�.
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
                        print("PCPlayer����");
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
                    print("VRPlayer����");
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
        }
    }

}
