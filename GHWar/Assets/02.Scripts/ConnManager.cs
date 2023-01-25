using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;
using System.Data;
using UnityEngine.UI;

public class ConnManager : MonoBehaviourPunCallbacks
{
    public byte Byte_maxPlayer = 2;

    [Header("PC �÷��̾� ���� ��ġ")]
    [SerializeField] Vector3 PC_Spawn;

    // �� ���� �� ������ ����
    void Start()
    {
        PhotonNetwork.GameVersion = "0.1";

        // �÷��̾ ����� ������ �г���
        int num_Player = Random.Range(0, 1000);
        PhotonNetwork.NickName = "Player" + num_Player.ToString();

        // ������ Ŭ���̾�Ʈ�� �ڵ����� ����ȭ������
        PhotonNetwork.AutomaticallySyncScene = true;

        // ���� ����(ȣ�� �� ȣ�� ���� �ÿ��� ����ϸ� ������ �õ�)
        PhotonNetwork.ConnectUsingSettings();
    }

    // ������ ���� ���� �� �κ� ���� ȣ��
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    // ���� ���� ���� �� ��õ�
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("�������� : ���� ����...");
        PhotonNetwork.ConnectUsingSettings();
    }
    // �κ� ���� �Ϸ� �� �� ����
    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ���� �Ϸ�!");
        // �濡 ������ �� �ִ� �ִ� �÷��̾� 2��
        RoomOptions ro_0 = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = Byte_maxPlayer };
        PhotonNetwork.JoinOrCreateRoom("NetTest", ro_0, TypedLobby.Default);
    }

    // �� ���� ���� �� ���� �ɼ����� �����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError("�� ���� ����...");
        RoomOptions ro_0 = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = Byte_maxPlayer };
        PhotonNetwork.JoinOrCreateRoom("NetTest", ro_0, TypedLobby.Default);
    }

    // �� ���Ӱ� ���ÿ� �÷��̾� ����
    public override void OnJoinedRoom()
    {
        Debug.Log("�� ����!");

        // �������� VR �÷��̾� ����
        if (GameManager.instance.IsVR)
        {
            //Vector2 originPos = Random.insideUnitCircle * 2.0f;
            GameManager.instance.Array_AllPlayers[0] = PhotonNetwork.Instantiate("VRPlayerXR", Vector3.zero, Quaternion.identity);
            GameManager.instance.Array_txtWinner[0] = GameManager.instance.Array_AllPlayers[0].GetComponent<VRPlayerMove1>().Txt_winnerText_VR;
        }
        // �ݰ� 2m �̳��� ���� ��ġ���� PC �÷��̾� ����
        else
        {
            //Vector2 originPos = Random.insideUnitCircle * 2.0f;
            //PhotonNetwork.Instantiate("PCPlayerXR", PC_Spawn, Quaternion.identity); 
            GameManager.instance.Array_AllPlayers[1] = PhotonNetwork.Instantiate("UniversalMale", PC_Spawn, Quaternion.identity);
            GameManager.instance.Array_txtWinner[1] = GameManager.instance.Array_AllPlayers[1].GetComponent<PC_Player_Move>().Txt_winnerText_PC;
        }

    }

    void Update()
    {
        
    }
}
