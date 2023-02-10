using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using Photon.Realtime;
using UnityEngine.UI;
using OVR.OpenVR;

// VR 플레이어 스크립트
public class VRPlayerMove : MonoBehaviourPun//, IPunObservable
{
    public Text Txt_winnerText_VR;
    public GameObject o_vrFace;

    [SerializeField] GameObject o_cam;
    [SerializeField] Transform t_player;

    // 스크립트 활성화 시 카메라 위치 정면으로 고정 >> 실패
    private void OnEnable()
    {
        Txt_winnerText_VR.text = string.Empty;
    }

    void Start()
    {
        // 위치 및 카메라 시선 고정 >> 실패
        o_cam.SetActive(true);

        // 카메라 사이의 충돌 방지
        if (!photonView.IsMine)
        {
            o_cam.SetActive(false);
        }

        //a_o_PCPlayers = GameObject.FindGameObjectsWithTag("PC_Player");
    }

}
