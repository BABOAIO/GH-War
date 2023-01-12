using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PCPlayerMove : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("이동속도")]
    [SerializeField] float f_moveSpeed = 3.0f;
    [Header("회전속도")]
    [SerializeField] float f_rotSpeed = 10.0f;
    [Header("점프위력")]
    [SerializeField] float f_jumpPower = 5.0f;

    [Header("PC 플레이어 카메라")]
    [SerializeField] GameObject PC_Player_Cam;
    [Header("PC 플레이어 트랜스폼")]
    [SerializeField] Transform PC_Player_Transform;
    //[Header("PC 플레이어 애니메이션")]
    //[SerializeField] Animator a_player;
    [Header("PC 플레이어 컨트롤러")]
    [SerializeField] Rigidbody PC_Player_Rigidbody;

    // 변경점 //
    [SerializeField]
    GameObject hand_L;
    [SerializeField]
    GameObject hand_R;

    float f_mouseX = 0;
    float f_mouseY = 0;

    Vector3 v3_setPos;
    Quaternion q_setRot;

    // 변경점 //
    Vector3 v3_setPos_handL;
    Quaternion q_setRot_handL;
    Vector3 v3_setPos_handR;
    Quaternion q_setRot_handR;

    GameObject[] a_o_VRPlayers;

    Vector3[] a_v3_setVRpos;
    Quaternion[] a_q_setVRrot;

    private PhotonView pv;

    void Start()
    {
        if (GameManager.instance.isVR)
        {
            //Camera cam_this = GetComponentInChildren<Camera>();
            //cam_this.transform.LookAt(GameObject.FindGameObjectWithTag("Ground").transform.position);

        }
        // 변경점 //
        hand_L = GameObject.FindGameObjectWithTag("LeftHand");
        hand_R = GameObject.FindGameObjectWithTag("RightHand");
        print(hand_L.name);
        print(hand_R.name);

        pv = GetComponent<PhotonView>();

        PC_Player_Cam.SetActive(true);
        PC_Player_Rigidbody = GetComponent<Rigidbody>();

        if (!photonView.IsMine)
        {
            PC_Player_Cam.SetActive(false);
        }

        //a_o_VRPlayers = GameObject.FindGameObjectsWithTag("VR_Player");
        //for (int i = 0; i < a_o_VRPlayers.Length; i++)
        //{
        //    a_v3_setVRpos[i] = a_o_VRPlayers[i].transform.position;
        //    a_q_setVRrot[i] = a_o_VRPlayers[i].transform.rotation;
        //}
    }

    void Update()
    {
        // 변경점 //
        // VR 플레이어는 따로 움직임
        if (GameManager.instance.isVR) { return; }

        //pv.RPC("Move", RpcTarget.All);
        //pv.RPC("Rotate", RpcTarget.All);
        //pv.RPC("Jump", RpcTarget.All);
        Move();
        Rotate(); // Rotate는 FixedUpdate에 넣으면 뚝뚝 끊겨보임
        Jump();
    }

    [PunRPC]
    void Move()
    {
        if (pv.IsMine)
        {
            float f_h = Input.GetAxis("Horizontal");
            float f_v = Input.GetAxis("Vertical");      // 플레이어 움직임 입력 받기 (상하좌우)

            Vector3 v3_moveDirection = new Vector3(f_h, 0, f_v);

            transform.Translate(v3_moveDirection * Time.deltaTime * f_moveSpeed);

            //anim...
        }
        else
        {
            PC_Player_Transform.transform.position = Vector3.Lerp(PC_Player_Transform.transform.position, v3_setPos, Time.deltaTime * 20f);
            PC_Player_Transform.rotation = Quaternion.Lerp(PC_Player_Transform.rotation, q_setRot, Time.deltaTime * 20f);

            // 변경점 //
            if (!GameManager.instance.isVR)
            {
                hand_L.transform.position = Vector3.Lerp(hand_L.transform.position, v3_setPos_handL, Time.deltaTime * 20f);
                hand_L.transform.rotation = Quaternion.Lerp(hand_L.transform.rotation, q_setRot_handL, Time.deltaTime * 20f);

                hand_R.transform.position = Vector3.Lerp(hand_R.transform.position, v3_setPos_handR, Time.deltaTime * 20f);
                hand_R.transform.rotation = Quaternion.Lerp(hand_R.transform.rotation, q_setRot_handR, Time.deltaTime * 20f);
            }
        }
    }

    [PunRPC]
    void Rotate()
    {
        if (pv.IsMine)
        {
            f_mouseX += Input.GetAxis("Mouse X") * f_rotSpeed;
            f_mouseY -= Input.GetAxis("Mouse Y") * f_rotSpeed;
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
                PC_Player_Rigidbody.AddForce(Vector3.up * f_jumpPower, ForceMode.Impulse);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(PC_Player_Transform.transform.position);
            stream.SendNext(PC_Player_Transform.rotation);
            //stream.SendNext(anim.GetFloat("Speed"));
        }

        else if (stream.IsReading)
        {
            v3_setPos = (Vector3)stream.ReceiveNext();
            q_setRot = (Quaternion)stream.ReceiveNext();
            //f_directionSpeed= (float)stream.ReceiveNext();
        }

        // 변경점 //
        //if (stream.IsWriting)
        //{
        //    stream.SendNext(this.transform.position);
        //    stream.SendNext(PC_Player_Transform.rotation);

        //    if (GameManager.instance.isVR)
        //    {
        //        stream.SendNext(hand_L.transform.position);
        //        stream.SendNext(hand_L.transform.rotation);
        //        stream.SendNext(hand_R.transform.position);
        //        stream.SendNext(hand_R.transform.rotation);
        //    }

        //    //stream.SendNext(anim.GetFloat("Speed"));
        //}

        //else if (stream.IsReading)
        //{
        //    v3_setPos = (Vector3)stream.ReceiveNext();
        //    q_setRot = (Quaternion)stream.ReceiveNext();

        //    if (!GameManager.instance.isVR)
        //    {
        //        v3_setPos_handL = (Vector3)stream.ReceiveNext();
        //        q_setRot_handL = (Quaternion)stream.ReceiveNext();
        //        v3_setPos_handR = (Vector3)stream.ReceiveNext();
        //        q_setRot_handR = (Quaternion)stream.ReceiveNext();
        //    }

        //f_directionSpeed= (float)stream.ReceiveNext();
    }
    
}
