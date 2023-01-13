using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PC_Player_Move : MonoBehaviourPun, IPunObservable
{
    [Header("�̵��ӵ�")]
    [SerializeField] float f_moveSpeed = 3.0f;
    [Header("ȸ���ӵ�")]
    [SerializeField] float f_rotSpeed = 10.0f;
    [Header("��������")]
    [SerializeField] float f_jumpPower = 5.0f;

    [Header("PC �÷��̾� ī�޶�")]
    [SerializeField] GameObject PC_Player_Cam;
    [Header("PC �÷��̾� Ʈ������")]
    [SerializeField] Transform PC_Player_Transform;
    //[Header("PC �÷��̾� �ִϸ��̼�")]
    //[SerializeField] Animator a_player;
    [Header("PC �÷��̾� ��Ʈ�ѷ�")]
    [SerializeField] Rigidbody PC_Player_Rigidbody;

    float f_mouseX = 0;
    float f_mouseY = 0;

    Vector3 v3_setPos = Vector3.zero;
    Quaternion q_setRot = Quaternion.identity;

    // ������ //
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
        if (GameManager.instance.isVR)
        {
            Camera cam_this = GetComponentInChildren<Camera>();
            cam_this.transform.LookAt(GameObject.FindGameObjectWithTag("Ground").transform.position);
            // ������ //
        }

        pv = GetComponent<PhotonView>();
        pv.Synchronization = ViewSynchronization.UnreliableOnChange;
        pv.ObservedComponents[0] = this;

        PC_Player_Cam.SetActive(true);
        PC_Player_Rigidbody = GetComponent<Rigidbody>();

        if (!photonView.IsMine)
        {
            PC_Player_Cam.SetActive(false);
            // ������ // 
            // Ʈ������ �� �Ҷ� ���� ����
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        v3_setPos = PC_Player_Transform.position;
        q_setRot = PC_Player_Transform.rotation;

        array_o_VRPlayers = GameObject.FindGameObjectsWithTag("PC_Player");

        for (int i = 0; i < array_o_VRPlayers.Length; i++)
        {
            array_o_handL[i] = array_o_VRPlayers[i].transform.Find("LeftHand").gameObject;
            array_o_handR[i] = array_o_VRPlayers[i].transform.Find("RightHand").gameObject;
        }
    }

    void Update()
    {
        // ������ //
        // VR �÷��̾�� ���� ������
        if (GameManager.instance.isVR) { return; }

        //pv.RPC("Move", RpcTarget.All);
        //pv.RPC("Rotate", RpcTarget.All);
        //pv.RPC("Jump", RpcTarget.All);
        if (pv.IsMine)
        {
            Move();
            Rotate(); // Rotate�� FixedUpdate�� ������ �Ҷ� ���ܺ���
            Jump();
        }
        else
        {
            // PC �÷��̾� ����
            PC_Player_Transform.position = Vector3.Lerp(PC_Player_Transform.position, v3_setPos, Time.deltaTime * 3.0f);
            PC_Player_Transform.rotation = Quaternion.Slerp(PC_Player_Transform.rotation, q_setRot, Time.deltaTime * 3.0f);
        }

        // VR �÷��̾� ����
        if (array_o_VRPlayers != null) return;

        for (int i = 0; i < array_o_VRPlayers.Length; i++)
        {
            a_v3_setVRpos[i] = array_o_VRPlayers[i].transform.position;
            a_q_setVRrot[i] = array_o_VRPlayers[i].transform.rotation;
            a_v3_setPos_handL[i] = array_o_handL[i].transform.position;
            a_q_setRot_handL[i] = array_o_handL[i].transform.rotation;
            a_v3_setPos_handR[i] = array_o_handR[i].transform.position;
            a_q_setRot_handR[i] = array_o_handR[i].transform.rotation;
        }
    }

    [PunRPC]
    void Move()
    {
        if (pv.IsMine)
        {
            // �÷��̾� ������ �Է� �ޱ� (�����¿�)
            float f_h = Input.GetAxis("Horizontal");
            float f_v = Input.GetAxis("Vertical");      

            Vector3 v3_moveDirection = new Vector3(f_h, 0, f_v);

            transform.Translate(v3_moveDirection * Time.deltaTime * f_moveSpeed);

            //anim...
        }

        // Ʈ������ �� ���ÿ��� ��Ȱ��ȭ
        else
        {
            PC_Player_Transform.position = Vector3.Lerp(PC_Player_Transform.position, v3_setPos, Time.deltaTime * 20f);
            PC_Player_Transform.rotation = Quaternion.Lerp(PC_Player_Transform.rotation, q_setRot, Time.deltaTime * 20f);

            if (array_o_VRPlayers.Length <= 0) { return; }

            for (int i = 0; i < array_o_VRPlayers.Length; i++)
            {
                array_o_VRPlayers[i].transform.position = Vector3.Lerp(array_o_VRPlayers[i].transform.position, a_v3_setVRpos[i], Time.deltaTime * 20f);
                array_o_VRPlayers[i].transform.rotation = Quaternion.Lerp(array_o_VRPlayers[i].transform.rotation, a_q_setVRrot[i], Time.deltaTime * 20f);
                array_o_handL[i].transform.position = Vector3.Lerp(array_o_handL[i].transform.position, a_v3_setPos_handL[i], Time.deltaTime * 20f);
                array_o_handL[i].transform.rotation = Quaternion.Lerp(array_o_handL[i].transform.rotation, a_q_setRot_handL[i], Time.deltaTime * 20f);
                array_o_handR[i].transform.position = Vector3.Lerp(array_o_handR[i].transform.position, a_v3_setPos_handR[i], Time.deltaTime * 20f);
                array_o_handR[i].transform.rotation = Quaternion.Lerp(array_o_handR[i].transform.rotation, a_q_setRot_handR[i], Time.deltaTime * 20f);
            }
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

            for (int i = 0; i < array_o_VRPlayers.Length; i++)
            {
                stream.SendNext(a_v3_setVRpos[i]);
                stream.SendNext(a_q_setVRrot[i]);
                stream.SendNext(a_v3_setPos_handL[i]);
                stream.SendNext(a_q_setRot_handL[i]);
                stream.SendNext(a_v3_setPos_handR[i]);
                stream.SendNext(a_q_setRot_handR[i]);
            }
        }

        else if (stream.IsReading)
        {
            v3_setPos = (Vector3)stream.ReceiveNext();
            q_setRot = (Quaternion)stream.ReceiveNext();

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