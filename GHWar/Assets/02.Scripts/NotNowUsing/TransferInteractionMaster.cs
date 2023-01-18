using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// VR �÷��̾ ���� ��� ������ IsMine�� ������ �Ѿ(XRGrabInteractionPun ����)
// > ������ PC�� �ٸ� ������Ʈ��� ��ȣ�ۿ��� �ȵ�...
// �̷��� �Ҷ� ������ : �ٸ� ������Ʈ��� ��ȣ�ۿ��� �ǳ� ��ĩ �߸� �ǵ鿩 ƨ���� ����...
public class TransferInteractionMaster : MonoBehaviour
{
    PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.GetComponent<PhotonView>().IsMine && !(collision.collider.CompareTag("PC_Player") || collision.collider.CompareTag("VR_Player")))
        {
            pv.RequestOwnership();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.collider.GetComponent<PhotonView>().IsMine && !(collision.collider.CompareTag("PC_Player") || collision.collider.CompareTag("VR_Player")))
        {
            pv.RequestOwnership();
        }
    }
}
