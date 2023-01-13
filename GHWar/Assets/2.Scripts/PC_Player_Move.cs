using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PC_Player_Move : MonoBehaviourPun, IPunObservable
{
    [Header("이동속도")]
    [SerializeField] float f_moveSpeed = 3.0f;
    [Header("회전속도")]
    [SerializeField] float f_rotSpeed = 1000.0f;
    [Header("점프위력")]
    [SerializeField] float f_jumpPower = 1500.0f;

    [Header("PC 플레이어 카메라")]
    [SerializeField] GameObject PC_Player_Cam;

    [SerializeField] float f_lerpSpeed = 50.0f;

    Transform PC_Player_Transform;
    //[Header("PC 플레이어 애니메이션")]
    //[SerializeField] Animator a_player;
    Rigidbody PC_Player_Rigidbody;

    float f_mouseX = 0;
    float f_mouseY = 0;

    Vector3 v3_setPos;
    Quaternion q_setRot;

    // 변경점 //
    GameObject[] array_o_VRPlayers;
    GameObject[] array_o_handL;
    GameObject[] array_o_handR;

    Vector3[] a_v3_setVRpos;
    Quaternion[] a_q_setVRrot;
    Vector3[] a_v3_setPos_handL;
    Quaternion[] a_q_setRot_handL;
    Vector3[] a_v3_setPos_handR;
    Quaternion[] a_q_setRot_handR;


    private PhotonView pv = null;

    void Awake()
    {
        // 굳이 넣지 않아도 될 부분
        PC_Player_Transform = GetComponent<Transform>();
        PC_Player_Rigidbody = GetComponent<Rigidbody>();

        if (!photonView.IsMine)
        {
            // 카메라 충돌 방지
            PC_Player_Cam.SetActive(false);

            // 변경점 // 
            // 트랜스폼 뷰 할때 떨림 방지 > 실패
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        // 초기 자기 포지션 백업? 왜 써있는거지?
        v3_setPos = PC_Player_Transform.position;
        q_setRot = PC_Player_Transform.rotation;

        // VR 플레이어 찾기
        array_o_VRPlayers = GameObject.FindGameObjectsWithTag("VR_Player");

        for (int i = 0; i < array_o_VRPlayers.Length; i++)
        {
            array_o_handL[i] = array_o_VRPlayers[i].transform.Find("LeftHand").gameObject;
            array_o_handR[i] = array_o_VRPlayers[i].transform.Find("RightHand").gameObject;
        }
    }

    void Update()
    {
        //pv.RPC("Move", RpcTarget.All);
        //pv.RPC("Rotate", RpcTarget.All);
        //pv.RPC("Jump", RpcTarget.All);

        // 기본 움직임 구현
        Move();
        Rotate(); // Rotate는 FixedUpdate에 넣으면 뚝뚝 끊겨보임
        Jump();

        // 모든 VR 플레이어 위치 보간, IsMine이 필요한가??
        if (array_o_VRPlayers.Length > 0)
        {
            for (int i = 0; i < array_o_VRPlayers.Length; i++)
            {
                array_o_VRPlayers[i].transform.position = Vector3.Lerp(array_o_VRPlayers[i].transform.position, a_v3_setVRpos[i], Time.deltaTime * f_lerpSpeed);
                array_o_VRPlayers[i].transform.rotation = Quaternion.Lerp(array_o_VRPlayers[i].transform.rotation, a_q_setVRrot[i], Time.deltaTime * f_lerpSpeed);
                array_o_handL[i].transform.position = Vector3.Lerp(array_o_handL[i].transform.position, a_v3_setPos_handL[i], Time.deltaTime * f_lerpSpeed);
                array_o_handL[i].transform.rotation = Quaternion.Lerp(array_o_handL[i].transform.rotation, a_q_setRot_handL[i], Time.deltaTime * f_lerpSpeed);
                array_o_handR[i].transform.position = Vector3.Lerp(array_o_handR[i].transform.position, a_v3_setPos_handR[i], Time.deltaTime * f_lerpSpeed);
                array_o_handR[i].transform.rotation = Quaternion.Lerp(array_o_handR[i].transform.rotation, a_q_setRot_handR[i], Time.deltaTime * f_lerpSpeed);
            }
        }

        // start 함수에서 탐색 못한 VR 플레이어 재탐색
        if (array_o_VRPlayers.Length <= 0)
        {
            array_o_VRPlayers = GameObject.FindGameObjectsWithTag("VR_Player");
        }

        if (!pv.IsMine)
        {
            // PC 플레이어 보간
            PC_Player_Transform.position = Vector3.Lerp(PC_Player_Transform.position, v3_setPos, Time.deltaTime * f_lerpSpeed);
            PC_Player_Transform.rotation = Quaternion.Slerp(PC_Player_Transform.rotation, q_setRot, Time.deltaTime * f_lerpSpeed);
        }

    }

    [PunRPC]
    void Move()
    {
        if (pv.IsMine)
        {
            // 플레이어 움직임 입력 받기 (상하좌우)
            float f_h = Input.GetAxis("Horizontal");
            float f_v = Input.GetAxis("Vertical");      

            Vector3 v3_moveDirection = new Vector3(f_h, 0, f_v);

            transform.Translate(v3_moveDirection * Time.deltaTime * f_moveSpeed);

            //anim...
        }
    }

    [PunRPC]
    void Rotate()
    {
        if (pv.IsMine)
        {
            f_mouseX += Input.GetAxis("Mouse X") * f_rotSpeed * Time.deltaTime;
            f_mouseY -= Input.GetAxis("Mouse Y") * f_rotSpeed * Time.deltaTime;
            transform.eulerAngles = new Vector3(0, f_mouseX, 0);
            PC_Player_Cam.transform.eulerAngles = new Vector3(f_mouseY, f_mouseX, 0);
        }
    }

    [PunRPC]
    void Jump()
    {
        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PC_Player_Rigidbody.AddForce(Vector3.up * f_jumpPower * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(PC_Player_Transform.position);
    //        stream.SendNext(PC_Player_Transform.rotation);
    //    }
    //    else
    //    {
    //        v3_setPos = (Vector3)stream.ReceiveNext();
    //        q_setRot = (Quaternion)stream.ReceiveNext();
    //    }
    //}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(PC_Player_Transform.position);
            stream.SendNext(PC_Player_Transform.rotation);

            if (array_o_VRPlayers.Length > 0)
            {
                for (int i = 0; i < array_o_VRPlayers.Length; i++)
                {
                    stream.SendNext(array_o_VRPlayers[i].transform.position);
                    stream.SendNext(array_o_VRPlayers[i].transform.rotation);
                    stream.SendNext(array_o_handL[i].transform.position);
                    stream.SendNext(array_o_handL[i].transform.rotation);
                    stream.SendNext(array_o_handR[i].transform.position);
                    stream.SendNext(array_o_handR[i].transform.rotation);
                }
            }
        }

        else if (stream.IsReading)
        {
            v3_setPos = (Vector3)stream.ReceiveNext();
            q_setRot = (Quaternion)stream.ReceiveNext();

            if (array_o_VRPlayers.Length > 0)
            {
                for (int i = 0; i < array_o_VRPlayers.Length; i++)
                {
                    a_v3_setVRpos[i] = (Vector3)stream.ReceiveNext();
                    a_q_setVRrot[i] = (Quaternion)stream.ReceiveNext();
                    a_v3_setPos_handL[i] = (Vector3)stream.ReceiveNext();
                    a_q_setRot_handL[i] = (Quaternion)stream.ReceiveNext();
                    a_v3_setPos_handR[i] = (Vector3)stream.ReceiveNext();
                    a_q_setRot_handR[i] = (Quaternion)stream.ReceiveNext();
                }
            }
        }
    }
}