using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PC_Player_Move : MonoBehaviourPunCallbacks//, IPunObservable
{
    [Header("Max HP")]
    public float MaxHP = 100.0f;
    [Header("HP")]
    public float HP = 100.0f;
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
    [Header("PC �÷��̾� �ִϸ��̼�")]
    [SerializeField] Animator a_player;
    [Header("PC �÷��̾� ��Ʈ�ѷ�")]
    [SerializeField] Rigidbody PC_Player_Rigidbody;

    // ������ //
    [SerializeField]
    GameObject hand_L;
    [SerializeField]
    GameObject hand_R;

    float f_mouseX = 0;
    float f_mouseY = 0;

    float invincibilityTime = 1.0f;
    float currentTime = 1.0f;

    //Vector3 v3_setPos = Vector3.zero;
    //Quaternion q_setRot = Quaternion.identity;

    //// ������ //
    //Vector3 v3_setPos_handL;
    //Quaternion q_setRot_handL;
    //Vector3 v3_setPos_handR;
    //Quaternion q_setRot_handR;

    private PhotonView pv = null;

    //public enum PC_Player_State
    //{
    //    Normal = 0,
    //    IsGrab = 1,
    //}

    //public PC_Player_State st_PC;

    void Awake()
    {

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

        //v3_setPos = PC_Player_Transform.position;
        //q_setRot = PC_Player_Transform.rotation;

        //st_PC = PC_Player_State.Normal;
    }

    void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        // ������ //
        // VR �÷��̾�� ���� ������
        //if (GameManager.instance.isVR) { return; }

        //pv.RPC("Move", RpcTarget.All);
        //pv.RPC("Rotate", RpcTarget.All);
        //pv.RPC("Jump", RpcTarget.All);

        //switch (st_PC)
        //{
        //    case PC_Player_State.Normal:
        //        break;
        //    case PC_Player_State.IsGrab:
        //        break;
        //}

        if (pv.IsMine)
        {
            Move();
            Rotate(); // Rotate�� FixedUpdate�� ������ �Ҷ� ���ܺ���
            Jump();
        }
        //else
        //{
        //    PC_Player_Transform.position = Vector3.Lerp(PC_Player_Transform.position, v3_setPos, Time.deltaTime * 3.0f);
        //    PC_Player_Transform.rotation = Quaternion.Slerp(PC_Player_Transform.rotation, q_setRot, Time.deltaTime * 3.0f);
        //}
    }

    [PunRPC]
    void Move()
    {
        if (pv.IsMine)
        {
            float f_h = Input.GetAxis("Horizontal");
            float f_v = Input.GetAxis("Vertical");      // �÷��̾� ������ �Է� �ޱ� (�����¿�)

            Vector3 v3_moveDirection = new Vector3(f_h, 0, f_v);

            transform.Translate(v3_moveDirection * Time.deltaTime * f_moveSpeed);

            //anim...
        }
        // Ʈ������ �� ���ÿ��� ��Ȱ��ȭ
        //else
        //{
        //    PC_Player_Transform.transform.position = Vector3.Lerp(PC_Player_Transform.transform.position, v3_setPos, Time.deltaTime * 20f);
        //    PC_Player_Transform.rotation = Quaternion.Lerp(PC_Player_Transform.rotation, q_setRot, Time.deltaTime * 20f);

        //    // ������ //
        //    if (!GameManager.instance.isVR)
        //    {
        //        hand_L.transform.position = Vector3.Lerp(hand_L.transform.position, v3_setPos_handL, Time.deltaTime * 20f);
        //        hand_L.transform.rotation = Quaternion.Lerp(hand_L.transform.rotation, q_setRot_handL, Time.deltaTime * 20f);

        //        hand_R.transform.position = Vector3.Lerp(hand_R.transform.position, v3_setPos_handR, Time.deltaTime * 20f);
        //        hand_R.transform.rotation = Quaternion.Lerp(hand_R.transform.rotation, q_setRot_handR, Time.deltaTime * 20f);
        //    }
        //}
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

    [PunRPC]
    public void Hit_PCPlayer(float damage)
    {
        HP -= damage;
        if (pv.IsMine)
        {
            Debug.Log($"{pv.Controller} is Damaged : Dmg {damage}");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

        if (f_objVelocity > 10 && currentTime >= invincibilityTime)
        {
            if (pv.IsMine)
            {
                pv.RPC("Hit_PCPlayer", RpcTarget.All, f_objVelocity);
            }
            currentTime = 0.0f;
        }
    }
}