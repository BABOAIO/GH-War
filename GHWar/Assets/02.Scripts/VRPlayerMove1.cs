using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using Photon.Realtime;
using UnityEngine.UI;
using OVR.OpenVR;

public class VRPlayerMove1 : MonoBehaviourPun//, IPunObservable
{
    public float HP = 2.0f;

    public Text Txt_winnerText_VR;

    //[SerializeField] float f_moveSpeed = 3.0f;
    //[SerializeField] float f_rotSpeed = 200.0f;
    [SerializeField] GameObject o_cam;
    [SerializeField] Transform t_player;
    //[SerializeField] Animator a_player;
    //[SerializeField] GameObject hand_L;
    //[SerializeField] GameObject hand_R;
    
    //Vector3 v3_setPos;
    //Quaternion q_setRot;
    //Vector3 v3_setPos_handL;
    //Quaternion q_setRot_handL;
    //Vector3 v3_setPos_handR;
    //Quaternion q_setRot_handR;

    //GameObject[] a_o_PCPlayers;

    //Vector3[] v3_setPCpos;
    //Quaternion[] q_setPCrot;

    // 스크립트 활성화 시 카메라 위치 정면으로 고정 >> 실패
    private void OnEnable()
    {
        Txt_winnerText_VR.text = string.Empty;
    }

    void Start()
    {
        // 위치 및 카메라 시선 고정 >> 실패
        o_cam.SetActive(true);
        //o_cam.transform.LookAt(GameObject.FindGameObjectWithTag("Ground").transform.position);

        // 카메라 사이의 충돌 방지
        if (!photonView.IsMine)
        {
            o_cam.SetActive(false);
        }

        //a_o_PCPlayers = GameObject.FindGameObjectsWithTag("PC_Player");
    }


    void FixedUpdate()
    {
        if(photonView.IsMine)
        {
            //Move();
            //Rotate();
        }
        //for(int i = 0; i < a_o_PCPlayers.Length; i++)
        //{
        //    v3_setPCpos[i] = a_o_PCPlayers[i].transform.position;
        //    q_setPCrot[i] = a_o_PCPlayers[i].transform.rotation;
        //}
    }

    void Move()
    {
        // 서버접속중인 것이 나라면 이동
        //if(photonView.IsMine)
        //{
        //    Vector2 v2_stickPos = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
        //    Vector3 v3_direction = new Vector3(v2_stickPos.x, 0, v2_stickPos.y);
        //    v3_direction.Normalize();

        //    v3_direction = o_cam.transform.TransformDirection(v3_direction);
        //    transform.position += v3_direction * f_moveSpeed * Time.deltaTime;

        //    float f_magnitude = v3_direction.magnitude;

        //    if (f_magnitude > 0)
        //    {
        //        t_player.rotation = Quaternion.LookRotation(v3_direction);
        //    }

        //}
        
    }

    void Rotate()
    {
        // 서버접속중인 것이 나라면 회전
        //if (photonView.IsMine)
        //{
        //    float f_rotH = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;

        //    o_cam.transform.eulerAngles += new Vector3(0, f_rotH, 0) * f_rotSpeed * Time.deltaTime;
        //}
    }

    [PunRPC]
    public void Hit_VRPlayer(float damage)
    {
        HP -= damage;
        Debug.Log($"VR Player {photonView.Controller} is Damaged : Dmg {damage}");

        if (HP <= 0)
        {
        }
    }

}
