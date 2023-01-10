using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;
using System.Data;

public class ConnManager : MonoBehaviourPunCallbacks
{
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
        RoomOptions ro_0 = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom("NetTest", ro_0, TypedLobby.Default);
    }

    // �� ���� ���� �� ���� �ɼ����� �����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError("�� ���� ����...");
        RoomOptions ro_0 = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom("NetTest", ro_0, TypedLobby.Default);
    }

    // �� ���Ӱ� ���ÿ� �÷��̾� ����
    public override void OnJoinedRoom()
    {
        Debug.Log("�� ����!");

        // �������� VR �÷��̾� ����
        if (GameManager.instance.isVR)
        {
            Vector2 originPos = Random.insideUnitCircle * 2.0f;
            PhotonNetwork.Instantiate("VRPlayerXR", Vector3.zero, Quaternion.identity);
        }
        // �ݰ� 2m �̳��� ���� ��ġ���� PC �÷��̾� ����
        else
        {
            Vector2 originPos = Random.insideUnitCircle * 2.0f;
            PhotonNetwork.Instantiate("PCPlayerXR", new Vector3(originPos.x, 0, originPos.y), Quaternion.identity);
        }

    }

    void Update()
    {
        
    }
}
