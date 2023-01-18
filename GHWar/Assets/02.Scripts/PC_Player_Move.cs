using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//using Photon.Realtime;

public class PC_Player_Move : MonoBehaviourPunCallbacks//, IPunObservable
{
    [Header("Max HP")]
    public float MaxHP = 100.0f;
    [Header("HP")]
    public float HP = 100.0f;
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
    [Header("PC 플레이어 애니메이션")]
    [SerializeField] Animator a_player;
    [Header("PC 플레이어 컨트롤러")]
    [SerializeField] Rigidbody PC_Player_Rigidbody;

    float f_mouseX = 0;
    float f_mouseY = 0;

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
            // 변경점 // 
            // 트랜스폼 뷰 할때 떨림 방지
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        //v3_setPos = PC_Player_Transform.position;
        //q_setRot = PC_Player_Transform.rotation;

        //st_PC = PC_Player_State.Normal;
    }

    void FixedUpdate()
    {

        if (pv.IsMine)
        {
            Move();
            Rotate(); // Rotate는 FixedUpdate에 넣으면 뚝뚝 끊겨보임
            Jump();
        }
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

    //[PunRPC]
    //public void Hit_PCPlayer(float damage)
    //{
    //    HP -= damage;
    //    if (pv.IsMine)
    //    {
    //        Debug.Log($"{pv.Controller} is Damaged : Dmg {damage}");
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

    //    if (f_objVelocity > 10 && currentTime >= invincibilityTime)
    //    {
    //        if (pv.IsMine)
    //        {
    //            pv.RPC("Hit_PCPlayer", RpcTarget.All, f_objVelocity);
    //        }
    //        currentTime = 0.0f;
    //    }
    //}
}