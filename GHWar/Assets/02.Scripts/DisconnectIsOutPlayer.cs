using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// �÷��̾ 2�� �̻��� ���, �Ѹ��� Disconnect�� �� �� �÷��̾��� ����䰡 ������ Ŭ���̾�Ʈ�� �Ѿ��.
// ���� �÷��̾ ���ֱ� ���� ��ũ��Ʈ, �÷��̾�鿡�� �ִ´�.
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
