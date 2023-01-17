using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using Photon.Realtime;

public class VRPlayerMove1 : MonoBehaviourPun//, IPunObservable
{
    [SerializeField] float f_moveSpeed = 3.0f;
    [SerializeField] float f_rotSpeed = 200.0f;
    [SerializeField] GameObject o_cam;
    [SerializeField] Transform t_player;
    //[SerializeField] Animator a_player;
    [SerializeField] GameObject hand_L;
    [SerializeField] GameObject hand_R;
    
    Vector3 v3_setPos;
    Quaternion q_setRot;
    Vector3 v3_setPos_handL;
    Quaternion q_setRot_handL;
    Vector3 v3_setPos_handR;
    Quaternion q_setRot_handR;

    GameObject[] a_o_PCPlayers;

    Vector3[] v3_setPCpos;
    Quaternion[] q_setPCrot;

    // ��ũ��Ʈ Ȱ��ȭ �� ī�޶� ��ġ �������� ���� >> ����
    private void OnEnable()
    {

    }

    void Start()
    {
        // ��ġ �� ī�޶� �ü� ���� >> ����
        o_cam.SetActive(true);
        //o_cam.transform.LookAt(GameObject.FindGameObjectWithTag("Ground").transform.position);

        // ī�޶� ������ �浹 ����
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
            Move();
            Rotate();
        }
        //for(int i = 0; i < a_o_PCPlayers.Length; i++)
        //{
        //    v3_setPCpos[i] = a_o_PCPlayers[i].transform.position;
        //    q_setPCrot[i] = a_o_PCPlayers[i].transform.rotation;
        //}
    }

    void Move()
    {
        // ������������ ���� ����� �̵�
        if(photonView.IsMine)
        {
            Vector2 v2_stickPos = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
            Vector3 v3_direction = new Vector3(v2_stickPos.x, 0, v2_stickPos.y);
            v3_direction.Normalize();

            v3_direction = o_cam.transform.TransformDirection(v3_direction);
            transform.position += v3_direction * f_moveSpeed * Time.deltaTime;

            float f_magnitude = v3_direction.magnitude;

            if (f_magnitude > 0)
            {
                t_player.rotation = Quaternion.LookRotation(v3_direction);
            }

            //anim...
        }

        // ���� ��ġ ����ȭ, ��ǻ� ���ǹ�
        else
        {
            transform.position = Vector3.Lerp(transform.position, v3_setPos, Time.deltaTime * 20f);
            t_player.rotation = Quaternion.Lerp(t_player.rotation, q_setRot, Time.deltaTime * 20f);

            if (a_o_PCPlayers.Length <= 0) { return; }

            for(int i = 0; i < a_o_PCPlayers.Length; i++)
            {
                transform.position = Vector3.Lerp(transform.position, v3_setPCpos[i], Time.deltaTime * 20f);
                t_player.rotation = Quaternion.Lerp(t_player.rotation, q_setPCrot[i], Time.deltaTime * 20f);
            }

            hand_L.transform.position = Vector3.Lerp(hand_L.transform.position, v3_setPos_handL, Time.deltaTime * 20f);
            hand_L.transform.rotation = Quaternion.Lerp(hand_L.transform.rotation, q_setRot_handL, Time.deltaTime * 20f);

            hand_R.transform.position = Vector3.Lerp(hand_R.transform.position, v3_setPos_handR, Time.deltaTime * 20f);
            hand_R.transform.rotation = Quaternion.Lerp(hand_R.transform.rotation, q_setRot_handR, Time.deltaTime * 20f);
        }
    }

    void Rotate()
    {
        // ������������ ���� ����� ȸ��
        if (photonView.IsMine)
        {
            float f_rotH = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;

            o_cam.transform.eulerAngles += new Vector3(0, f_rotH, 0) * f_rotSpeed * Time.deltaTime;
        }
    }

    // �� �κ��� ���, ���� ������ �۾��� ���� �ۼ��ϴ� ��ũ��Ʈ�̳� Lerp ���� 30000������ ���߸� ������� �ڿ����������Ƿ� ����
    // �� �ð����� ���� ������ ��ġ, ȸ���� ����, �о����
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(transform.position);
    //        stream.SendNext(t_player.rotation);
    //        stream.SendNext(hand_L.transform.position);
    //        stream.SendNext(hand_L.transform.rotation);
    //        stream.SendNext(hand_R.transform.position);
    //        stream.SendNext(hand_R.transform.rotation);
    //        for(int i = 0; i < a_o_PCPlayers.Length; i++)
    //        {
    //            stream.SendNext(v3_setPCpos[i]);
    //            stream.SendNext(q_setPCrot[i]);
    //        }
            
    //        //stream.SendNext(anim.GetFloat("Speed"));
    //    }

    //    else if (stream.IsReading)
    //    {
    //        v3_setPos = (Vector3)stream.ReceiveNext();
    //        q_setRot= (Quaternion)stream.ReceiveNext();
    //        v3_setPos_handL = (Vector3)stream.ReceiveNext();
    //        q_setRot_handL = (Quaternion)stream.ReceiveNext();
    //        v3_setPos_handR = (Vector3)stream.ReceiveNext();
    //        q_setRot_handR = (Quaternion)stream.ReceiveNext();
    //        for(int i = 0; i < a_o_PCPlayers.Length; i++)
    //        {
    //            v3_setPCpos[i] = (Vector3)stream.ReceiveNext();
    //            q_setPCrot[i] = (Quaternion)stream.ReceiveNext();
    //        }

    //        //f_directionSpeed= (float)stream.ReceiveNext();
    //    }
    //}
    
}
