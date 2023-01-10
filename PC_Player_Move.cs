using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PC_Player_Move : MonoBehaviourPun, IPunObservable
{
    [Header("�̵��ӵ�")]
    [SerializeField] float f_moveSpeed = 3.0f;
    [Header("ȸ���ӵ�")]
    [SerializeField] float f_rotSpeed = 50.0f;
    [Header("��������")]
    [SerializeField] float f_jumpPower = 5.0f;

    [Header("PC �÷��̾� ī�޶�")]
    [SerializeField] GameObject PC_Player_Cam;
    [Header("PC �÷��̾� Ʈ������")]
    [SerializeField] Transform PC_Player_Transform;
    //[Header("PC �÷��̾� �ִϸ��̼�")]
    //[SerializeField] Animator a_player;
<<<<<<< Updated upstream
    [Header("PC �÷��̾� ��Ʈ�ѷ�")]
    [SerializeField] Rigidbody PC_Player_Rigidbody;

    float f_mouseX = 0; 
=======
    [Header("PC �÷��̾� ������ٵ�")]
    [SerializeField] Rigidbody PC_Player_Rigidbody;

    float f_mouseX = 0; 
    float f_mouseY = 0;
    float f_mouseYlim;
>>>>>>> Stashed changes

    Vector3 v3_setPos;
    Quaternion q_setRot;

<<<<<<< Updated upstream
    void Start()
    {
        PC_Player_Cam.SetActive(true);
        PC_Player_Rigidbody = GetComponent<Rigidbody>();
=======
    public int jumpCount = 2;
    bool isGround = false;
    bool isJump = false;

    public int DodgeCount = 2;
    bool isDodge = false;

    Animator anim;

    private PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();

        PC_Player_Cam.SetActive(true);
        PC_Player_Rigidbody = GetComponent<Rigidbody>();

        jumpCount = 0;

        Cursor.lockState = CursorLockMode.Locked;

        if(!pv.IsMine)
        {
            PC_Player_Cam.SetActive(false);
        }
>>>>>>> Stashed changes
    }

    void Update()
    {
<<<<<<< Updated upstream
        Move();
        Rotate(); // Rotate�� FixedUpdate�� ������ �Ҷ� ���ܺ���
=======
        if (pv.IsMine)
        {
            Move();
            Rotate(); // Rotate�� FixedUpdate�� ������ �Ҷ� ���ܺ���
            Jump();
            Dodge();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
>>>>>>> Stashed changes
    }

    void Move()
    {
<<<<<<< Updated upstream
        if (photonView.IsMine)
=======
        if (pv.IsMine)
>>>>>>> Stashed changes
        {
            float f_h = Input.GetAxis("Horizontal");    
            float f_v = Input.GetAxis("Vertical");      // �÷��̾� ������ �Է� �ޱ� (�����¿�)

            Vector3 v3_moveDirection = new Vector3(f_h, 0, f_v);

            transform.Translate(v3_moveDirection * Time.deltaTime * f_moveSpeed);

            //anim...
        }
        else
        {
<<<<<<< Updated upstream
            transform.position = Vector3.Lerp(transform.position, v3_setPos, Time.deltaTime * 20f);
=======
            PC_Player_Transform.transform.position = Vector3.Lerp(PC_Player_Transform.transform.position, v3_setPos, Time.deltaTime * 20f);
>>>>>>> Stashed changes
            PC_Player_Transform.rotation = Quaternion.Lerp(PC_Player_Transform.rotation, q_setRot, Time.deltaTime * 20f);
        }
    }

    void Rotate()
    {
<<<<<<< Updated upstream
        if (photonView.IsMine)
        {
            f_mouseX += Input.GetAxis("Mouse X") * f_rotSpeed;
            transform.eulerAngles = new Vector3(0, f_mouseX, 0);
=======
        if (pv.IsMine)
        {
            f_mouseX += Input.GetAxis("Mouse X") * f_rotSpeed;
            f_mouseY -= Input.GetAxis("Mouse Y") * f_rotSpeed;
            transform.eulerAngles = new Vector3(0, f_mouseX, 0);

            
            f_mouseYlim = f_mouseYlim + f_mouseY;
            f_mouseYlim = Mathf.Clamp(f_mouseY, -90, 90); // ���콺 ���Ʒ� ȸ���� ����
            PC_Player_Cam.transform.eulerAngles = new Vector3(f_mouseYlim, f_mouseX, 0);
>>>>>>> Stashed changes
        }
    }

    void Jump()
    {
<<<<<<< Updated upstream
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PC_Player_Rigidbody.AddForce(Vector3.up * f_jumpPower, ForceMode.Impulse);
            }
=======
        if (pv.IsMine)
        {
            if (isGround && isDodge == false)
            {
                if (jumpCount > 0)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        PC_Player_Rigidbody.AddForce(Vector3.up * f_jumpPower, ForceMode.Impulse);
                        jumpCount--;
                        isJump = true;
                    }
                }
            }

        }
    }

    void Dodge()
    {
        if (pv.IsMine)
        {
            if(isJump == false && isDodge == false)
            {
                if (DodgeCount > 0)
                {
                    if (Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        Debug.Log("������");
                        f_moveSpeed *= 2;
                        anim.SetTrigger("Roll");
                        DodgeCount--;
                        isDodge= true;

                        Invoke("DodgeOut", 0.5f);
                    }
                }
            }
        }
    }

    void DodgeOut()
    {
        if (pv.IsMine)
        {
            f_moveSpeed *= 0.5f;
            DodgeCount = 2;
            isDodge = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            isJump = false;
            jumpCount = 2;
>>>>>>> Stashed changes
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
<<<<<<< Updated upstream
            stream.SendNext(transform.position);
            stream.SendNext(PC_Player_Transform.rotation);
=======
            stream.SendNext(PC_Player_Transform.transform.position);
            stream.SendNext(PC_Player_Transform.rotation);
            stream.SendNext(PC_Player_Cam.transform.rotation);
>>>>>>> Stashed changes
            //stream.SendNext(anim.GetFloat("Speed"));
        }

        else if (stream.IsReading)
        {
            v3_setPos = (Vector3)stream.ReceiveNext();
            q_setRot = (Quaternion)stream.ReceiveNext();
            //f_directionSpeed= (float)stream.ReceiveNext();
        }
    }
}
