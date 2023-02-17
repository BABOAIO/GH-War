using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 맵을 둘러싸놓은 투명 벽에 넣는다.(트리거를 체크해야 동작)
// 밖으로 빠져나간 오브젝트에 대한 연산을 줄여주기 위한 벽
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
