using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PC_Player_Move : MonoBehaviourPun, IPunObservable
{
    [Header("이동속도")]
    [SerializeField] float f_moveSpeed = 3.0f;
    [Header("회전속도")]
    [SerializeField] float f_rotSpeed = 50.0f;
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

    float f_mouseX = 0; 
    float f_mouseY = 0;

    Vector3 v3_setPos;
    Quaternion q_setRot;

    private PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();

        PC_Player_Cam.SetActive(true);
        PC_Player_Rigidbody = GetComponent<Rigidbody>();

        if (!photonView.IsMine)
        {
            PC_Player_Cam.SetActive(false);
        }
    }

    void Update()
    {
        if(pv.IsMine)
        {
            Move();
            Rotate(); // Rotate는 FixedUpdate에 넣으면 뚝뚝 끊겨보임
            Jump();
        }
    }

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
        }
    }

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
            stream.SendNext(PC_Player_Cam.transform.rotation);
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
