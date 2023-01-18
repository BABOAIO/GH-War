using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// VR 플레이어가 손을 댔기 때문에 IsMine의 권한이 넘어감(XRGrabInteractionPun 참고)
// > 때문에 PC가 다른 오브젝트들과 상호작용이 안됨...
// 이렇게 할때 문제점 : 다른 오브젝트들과 상호작용은 되나 자칫 잘못 건들여 튕겨져 나옴...
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
