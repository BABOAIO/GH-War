using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PCPlayerMove : MonoBehaviourPun, IPunObservable
{
    [Header("이동속도")]
    [SerializeField] float f_moveSpeed = 3.0f;
    [Header("회전속도")]
    [SerializeField] float f_rotSpeed = 200.0f;
    [SerializeField] GameObject o_cam;
    [SerializeField] Transform t_player;
    //[SerializeField] Animator a_player;
    [SerializeField] CharacterController cc_playerCtrl;

    float f_mouseX = 0;

    Vector3 v3_setPos;
    Quaternion q_setRot;

    void Start()
    {
        o_cam.SetActive(true);
    }


    void FixedUpdate()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        if (photonView.IsMine)
        {
            float f_h = Input.GetAxis("Horizontal");
            float f_v = Input.GetAxis("Vertical");

            Vector3 v3_moveDirection = new Vector3(f_h, 0, f_v);

            cc_playerCtrl.Move(v3_moveDirection * Time.deltaTime * f_moveSpeed);

            //anim...
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, v3_setPos, Time.deltaTime * 20f);
            t_player.rotation = Quaternion.Lerp(t_player.rotation, q_setRot, Time.deltaTime * 20f);
        }
    }

    void Rotate()
    {
        if (photonView.IsMine)
        {
            f_mouseX += Input.GetAxis("Mouse X") * f_rotSpeed;
            transform.eulerAngles = new Vector3(0, f_mouseX, 0);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(t_player.rotation);
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
