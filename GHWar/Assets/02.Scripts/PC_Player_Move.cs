using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UniRx;
using UnityEngine.UI;
using UniRx.Triggers;
using System;

public class PC_Player_Move : MonoBehaviourPunCallbacks, IPunObservable
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
    [Header("PC 플레이어 애니메이션")]
    [SerializeField] public Animator a_player;
    [Header("PC 플레이어 컨트롤러")]
    [SerializeField] Rigidbody PC_Player_Rigidbody;

    PCPlayerFireArrow fireArrow;

    XRGrabInteractionPun xrgrabinteractionPun;

    float f_mouseX = 0;
    float f_mouseY = 0;
    float f_mouseYlim;

    private PhotonView pv = null;

    public Text Txt_winnerText_PC;

    #region 캐릭터 점프, 구르기 관련
    //bool Run = false;

    public int jumpCount = 2;
    bool isGround = false;
    public bool isJump = false;

    public bool isDodge = false;
    #endregion

    public bool isDie = false;
    AudioSource as_move;
    [SerializeField] AudioClip ac_run;
    [SerializeField] AudioClip ac_aimWalk;
    [SerializeField] AudioClip ac_jumpUP;
    [SerializeField] AudioClip ac_jumpDown;
    [SerializeField] AudioClip ac_roll;

    void Awake()
    {
        Application.targetFrameRate = 120; // 화면 프레임 조절

        Txt_winnerText_PC.text = string.Empty;

        pv = GetComponent<PhotonView>();
        pv.Synchronization = ViewSynchronization.UnreliableOnChange;
        pv.ObservedComponents[0] = this;

        xrgrabinteractionPun = GetComponent<XRGrabInteractionPun>();

        PC_Player_Cam.SetActive(true);
        PC_Player_Rigidbody = GetComponent<Rigidbody>();
        fireArrow = GetComponent<PCPlayerFireArrow>();

        a_player = GetComponent<Animator>();

        jumpCount = 0;

        Cursor.lockState = CursorLockMode.Locked;

        as_move = GetComponent<AudioSource>();
        as_move.Stop();

        if (!photonView.IsMine)
        {
            PC_Player_Cam.SetActive(false);
            // 변경점 // 
            // 트랜스폼 뷰 할때 떨림 방지
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        isGround = false;
    }

    Vector3 pos;
    Quaternion rot;

    void FixedUpdate()
    {
        //print(xrgrabinteractionPun.isGrab);
        if (pv.IsMine)
        {
            if(!isDie)
            {
                Move();
                Rotate();
                Jump();
                Dodge();
                //if (PhotonNetwork.CountOfPlayers >= 2)
                //{
                //    pv.RPC("Move", RpcTarget.All);
                //    pv.RPC("Rotate", RpcTarget.All);
                //    pv.RPC("Jump", RpcTarget.All);
                //    pv.RPC("Dodge", RpcTarget.All);
                //}
                //else
                //{
                //    Move();
                //    Rotate();
                //    Jump();
                //    Dodge();
                //}
            }
            Wiggle();

            if (!isGround)
            {
                PC_Player_Rigidbody.AddForce(Vector3.down * Time.deltaTime * f_jumpPower, ForceMode.Acceleration);
            }
        }
        else
        {
            transform.position = pos;
            transform.rotation = rot;
        }

        this.GetComponent<CapsuleCollider>().enabled = !xrgrabinteractionPun.isGrab;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    [PunRPC]
    void Wiggle()
    {
        a_player.SetBool("Wiggle", xrgrabinteractionPun.isGrab);
    }

    [PunRPC]
    public void Move()
    {
        if (pv.IsMine
            //&& !isDodge
            )
        {
            if (!fireArrow.B_isReadyToShot)
            {
                float f_h = Input.GetAxis("Horizontal");
                float f_v = Input.GetAxis("Vertical");      // 플레이어 움직임 입력 받기 (상하좌우)

                Vector3 v3_moveDirection = new Vector3(f_h, 0, f_v);

                transform.Translate(v3_moveDirection * Time.deltaTime * f_moveSpeed);

                // 
                if (Mathf.Abs(f_h) <= 0.1f && Mathf.Abs(f_v) <= 0.1f)
                {
                    a_player.SetBool("IdleToRun", false);
                    //as_move.Stop();
                    //a_player.SetBool("Run", false);
                }
                else
                {
                    a_player.SetBool("IdleToRun", true);
                    a_player.SetFloat("_RunLeft", f_h);
                    a_player.SetFloat("_RunForward", f_v);

                    // 소리부분 //
                    if (!as_move.isPlaying && isGround)
                    {
                        as_move.PlayOneShot(ac_run, 0.7f);
                    }
                    //a_player.SetBool("Run", true);
                }
            }

            else if (fireArrow.B_isReadyToShot && isGround)
            {
                float f_h = Input.GetAxis("Horizontal");
                float f_v = Input.GetAxis("Vertical");      // 플레이어 움직임 입력 받기 (상하좌우)

                Vector3 v3_moveDirection = new Vector3(f_h, 0, f_v);

                transform.Translate(v3_moveDirection * Time.deltaTime * f_moveSpeed / 2.0f);

                if (Mathf.Abs(f_h) <= 0.1f && Mathf.Abs(f_v) <= 0.1f)
                {
                    a_player.SetBool("IdleToRun", false);
                    //as_move.Stop();
                    //a_player.SetBool("Run", false);
                }
                else
                {
                    a_player.SetBool("IdleToRun", true);
                    a_player.SetFloat("_AimWalkLeft", f_h);
                    a_player.SetFloat("_AimWalkForward", f_v);

                    if (!as_move.isPlaying && isGround)
                    {
                        as_move.PlayOneShot(ac_aimWalk);
                    }
                    //a_player.SetBool("Run", true);
                }
            }
        }
    }

    [PunRPC]
    void Rotate()
    {
        if (pv.IsMine)
        {
            if (!isDie)
            {
                f_mouseX += Input.GetAxis("Mouse X") * f_rotSpeed * Time.deltaTime;
                f_mouseY -= Input.GetAxis("Mouse Y") * f_rotSpeed * Time.deltaTime;
                transform.eulerAngles = new Vector3(0, f_mouseX, 0);

                f_mouseYlim = f_mouseYlim + f_mouseY;
                f_mouseY = Mathf.Clamp(f_mouseY, -70, 30);
                PC_Player_Cam.transform.eulerAngles = new Vector3(f_mouseY, f_mouseX, 0);
            }
            else
            {
                PC_Player_Cam.transform.rotation = Quaternion.identity;
                PC_Player_Cam.transform.LookAt(gameObject.transform);
                transform.rotation = Quaternion.identity;
            }
        }
    }

    [PunRPC]
    public void Jump()
    {
        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //if (!isDodge)
                //{
                if (jumpCount > 0)
                {
                    as_move.PlayOneShot(ac_jumpUP);
                    PC_Player_Rigidbody.AddForce(Vector3.up * f_jumpPower * 1000f * Time.deltaTime, ForceMode.Impulse);
                    jumpCount--;
                    isJump = true;
                    //a_player.SetTrigger("Jump");

                    fireArrow.B_isReadyToShot = false;

                    // setbool을 settrigger처럼 쓸 수 있음
                    a_player.SetBool("IsJump", true);
                    Observable.NextFrame().Subscribe(_ => a_player.SetBool("IsJump", false));
                }
                //}
            }
        }
    }

    [PunRPC]
    public void Dodge()
    {
        if (pv.IsMine)
        {
            if (!isDodge)
            {
                float f_h = Input.GetAxis("Horizontal");
                float f_v = Input.GetAxis("Vertical");
                if (Mathf.Abs(f_v) > 0.0f)
                {
                    if (Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        f_moveSpeed *= 2f;
                        //PC_Player_Rigidbody.AddForce(PC_Player_Transform.forward * 300f, ForceMode.Impulse);
                        //a_player.SetTrigger("Roll")
                        fireArrow.B_isReadyToShot = false;
                        a_player.SetBool("IsRoll", true);
                        Observable.NextFrame().Subscribe(_ => a_player.SetBool("IsRoll", false));
                        isDodge = true;

                        as_move.PlayOneShot(ac_roll);

                        Invoke("DodgeOut", 0.7f);
                    }
                }
            }
            else
            {
                //a_player.SetBool("Roll", false);
            }
        }
    }

    [PunRPC]
    void DodgeOut()
    {
        if (pv.IsMine)
        {
            PC_Player_Rigidbody.velocity = Vector3.zero;
            f_moveSpeed /= 2f;
            Invoke("DelayedDodgeFalse", 1.3f);
        }
    }

    [PunRPC]
    void DelayedDodgeFalse()
    {
        isDodge = false;
    }

    [PunRPC]
    private void OnCollisionEnter(Collision collision)
    {
        isJump = false;
        jumpCount = 2;
    }

    private void OnCollisionStay(Collision collision)
    {
        isGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGround = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsReading)
        {
            pos = (Vector3)stream.ReceiveNext();
            rot= (Quaternion)stream.ReceiveNext();
        }
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
    }
}