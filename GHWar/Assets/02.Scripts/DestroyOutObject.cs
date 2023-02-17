using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �ѷ��γ��� ���� ���� �ִ´�.(Ʈ���Ÿ� üũ�ؾ� ����)
// ������ �������� ������Ʈ�� ���� ������ �ٿ��ֱ� ���� ��
public class DestroyOutObject : MonoBehaviourPun
{
    private void Awake()
    {
        //photonView.TransferOwnership(PhotonNetwork.MasterClient);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle")
            || other.gameObject.CompareTag("Arrow")
            || other.gameObject.CompareTag("CannonBall")
            || other.gameObject.CompareTag("Rock")
            || other.gameObject.CompareTag("Ground")
            || other.gameObject.CompareTag("SmallRock"))
        {
            //other.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.MasterClient);
            PhotonNetwork.Destroy(other.gameObject);
        }
    }
}
