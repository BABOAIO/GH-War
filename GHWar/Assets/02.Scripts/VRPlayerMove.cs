using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using Photon.Realtime;
using UnityEngine.UI;
using OVR.OpenVR;

// VR �÷��̾� ��ũ��Ʈ
public class VRPlayerMove : MonoBehaviourPun//, IPunObservable
{
    public Text Txt_winnerText_VR;
    public GameObject o_vrFace;

    [SerializeField] GameObject o_cam;
    [SerializeField] Transform t_player;

    // ��ũ��Ʈ Ȱ��ȭ �� ī�޶� ��ġ �������� ���� >> ����
    private void OnEnable()
    {
        Txt_winnerText_VR.text = string.Empty;
    }

    void Start()
    {
        // ��ġ �� ī�޶� �ü� ���� >> ����
        o_cam.SetActive(true);

        // ī�޶� ������ �浹 ����
        if (!photonView.IsMine)
        {
            o_cam.SetActive(false);
        }

        //a_o_PCPlayers = GameObject.FindGameObjectsWithTag("PC_Player");
    }

}
