using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class VRPlayerMove : MonoBehaviourPun, IPunObservable
{
    [SerializeField] float f_moveSpeed = 3.0f;
    [SerializeField] float f_rotSpeed = 200.0f;
    [SerializeField] GameObject o_cam;
    [SerializeField] Transform t_player;
    //[SerializeField] Animator a_player;
    [SerializeField] GameObject hand_L;
    [SerializeField] GameObject hand_R;

    Vector3 v3_setPos;
    Quaternion q_setRot;
    Vector3 v3_setPos_handL;
    Quaternion q_setRot_handL;
    Vector3 v3_setPos_handR;
    Quaternion q_setRot_handR;

    // 스크립트 활성화 시 카메라 위치 정면으로 고정 >> 실패
    private void OnEnable()
    {

    }

    void Start()
    {
        o_cam.SetActive(true);
        o_cam.transform.LookAt(GameObject.FindGameObjectWithTag("Ground").transform.position);

        if (!photonView.IsMine)
        {
            o_cam.SetActive(false);
        }
    }


    void Update()
    {
        if(photonView.IsMine)
        {
            Move();
            Rotate();
        }
    }

    void Move()
    {
        // 서버접속중인 것이 나라면 이동
        if(photonView.IsMine)
        {
            Vector2 v2_stickPos = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
            Vector3 v3_direction = new Vector3(v2_stickPos.x, 0, v2_stickPos.y);
            v3_direction.Normalize();

            v3_direction = o_cam.transform.TransformDirection(v3_direction);
            transform.position += v3_direction * f_moveSpeed * Time.deltaTime;

            float f_magnitude = v3_direction.magnitude;

            if (f_magnitude > 0)
            {
                t_player.rotation = Quaternion.LookRotation(v3_direction);
            }

            //anim...
        }
        // 상대방 위치 동기화
        else
        {
            transform.position = Vector3.Lerp(transform.position, v3_setPos, Time.deltaTime * 20f);
            t_player.rotation = Quaternion.Lerp(t_player.rotation, q_setRot, Time.deltaTime * 20f);

            hand_L.transform.position = Vector3.Lerp(hand_L.transform.position, v3_setPos_handL, Time.deltaTime * 20f);
            hand_L.transform.rotation = Quaternion.Lerp(hand_L.transform.rotation, q_setRot_handL, Time.deltaTime * 20f);

            hand_R.transform.position = Vector3.Lerp(hand_R.transform.position, v3_setPos_handL, Time.deltaTime * 20f);
            hand_R.transform.rotation = Quaternion.Lerp(hand_R.transform.rotation, q_setRot_handL, Time.deltaTime * 20f);
            // PC 플레이어 위치동기화
            // PC 플레이어 전부 받아오기, 포톤뷰로 담아와야하잖아?
            //GameObject[] tmp = GameObject.FindGameObjectsWithTag("PC_Player");

            //foreach(var item in tmp)
            //{

            //}
        }
    }

    void Rotate()
    {
        // 서버접속중인 것이 나라면 회전
        if (photonView.IsMine)
        {
            float f_rotH = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;

            o_cam.transform.eulerAngles += new Vector3(0, f_rotH, 0) * f_rotSpeed * Time.deltaTime;
        }
    }

    // 매 시간마다 변한 상대방의 위치, 회전값 전송, 읽어오기
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(t_player.rotation);
            stream.SendNext(hand_L.transform.position);
            stream.SendNext(hand_L.transform.rotation);
            stream.SendNext(hand_R.transform.position);
            stream.SendNext(hand_R.transform.rotation);

            //stream.SendNext(anim.GetFloat("Speed"));
        }

        else if (stream.IsReading)
        {
            v3_setPos = (Vector3)stream.ReceiveNext();
            q_setRot= (Quaternion)stream.ReceiveNext();
            v3_setPos_handL = (Vector3)stream.ReceiveNext();
            q_setRot_handL = (Quaternion)stream.ReceiveNext();
            v3_setPos_handR = (Vector3)stream.ReceiveNext();
            q_setRot_handR = (Quaternion)stream.ReceiveNext();

            //f_directionSpeed= (float)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void GrabObject()
    {

    }
}
