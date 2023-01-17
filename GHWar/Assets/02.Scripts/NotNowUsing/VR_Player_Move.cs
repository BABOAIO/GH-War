using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using Photon.Realtime;
using UnityEngine.XR;

public class VR_Player_Move : MonoBehaviourPun//, IPunObservable
{
    //[SerializeField] float f_moveSpeed = 3.0f;
    //[SerializeField] float f_rotSpeed = 200.0f;
    [SerializeField] GameObject o_cam;
    [SerializeField] Transform /*t_head,*/ t_leftHand, t_rightHand;
    //[SerializeField] Animator a_player;

    PhotonView pv;

    Vector3 v3_setPos;
    Quaternion q_setRot;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (pv.IsMine)
        {
            t_rightHand.gameObject.SetActive(false);
            t_leftHand.gameObject.SetActive(false);
            //t_head.gameObject.SetActive(false);

            //MapPosition(t_head, XRNode.Head);
            MapPosition(t_leftHand, XRNode.LeftHand);
            MapPosition(t_rightHand, XRNode.RightHand);
        }
    }

    void MapPosition(Transform target, XRNode node)
    {
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);

        target.position = position;
        target.rotation = rotation;
    }
}

//    // 매 시간마다 변한 상대방의 위치, 회전값 전송, 읽어오기
//    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//    {
//        if (stream.IsWriting)
//        {
//            stream.SendNext(transform.position);
//           // stream.SendNext(t_player.rotation);
//            //stream.SendNext(anim.GetFloat("Speed"));
//        }

//        else if (stream.IsReading)
//        {
//            v3_setPos = (Vector3)stream.ReceiveNext();
//            q_setRot = (Quaternion)stream.ReceiveNext();
//            //f_directionSpeed= (float)stream.ReceiveNext();
//        }
//    }
//}
