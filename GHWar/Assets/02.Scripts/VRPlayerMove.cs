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
    public AudioSource as_VR;

    float currentTime = 0.0f;
    float delayTime = 5.0f;

    [SerializeField] GameObject o_cam;
    [SerializeField] Transform t_player;

    // ��ũ��Ʈ Ȱ��ȭ �� ī�޶� ��ġ �������� ���� >> ����
    private void OnEnable()
    {
        Txt_winnerText_VR.text = string.Empty;
    }

    void Start()
    {
        as_VR = GetComponent<AudioSource>();

        // ��ġ �� ī�޶� �ü� ���� >> ����
        o_cam.SetActive(true);

        // ī�޶� ������ �浹 ����
        if (!photonView.IsMine)
        {
            o_cam.SetActive(false);
        }

        currentTime = 0f;
        //a_o_PCPlayers = GameObject.FindGameObjectsWithTag("PC_Player");
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            currentTime += Time.fixedDeltaTime;
            if (currentTime > delayTime)
            {
                photonView.RPC("MonsterHowlling", RpcTarget.All);
                currentTime = 0;
                delayTime = Random.Range(5, 10);
            }
        }
    }

    [PunRPC]
    void MonsterHowlling()
    {
        as_VR.Play();
    }
}
