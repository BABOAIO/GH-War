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
        _xr = GetComponent<XRGrabInteractionPun>();
    }

    private void FixedUpdate()
    {
        if (IsGrab)
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
