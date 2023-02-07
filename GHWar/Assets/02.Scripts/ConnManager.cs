using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;
using System.Data;
using UnityEngine.UI;

// ConnManager�� �ִ´�. ��� ��� ���̵� ������� ���ӸŴ����� ����
public class ConnManager : MonoBehaviourPunCallbacks
{
    // ���漭���� ������ �÷��̾��� ������Ʈ�̸�(0�̸� VR, 1�̸� PC)
    [SerializeField] string[] array_PlayerType = new string[2];

    // �ִ� �����ο�
    public byte Byte_maxPlayer = 2;

    [Header("VR �÷��̾� ���� ��ġ")]
    public Vector3 VR_Spawn;
    [Header("PC �÷��̾� ���� ��ġ")]
    public Vector3 PC_Spawn;

    // �̱���
    public static ConnManager Conn = null;

    private void Awake()
    {
        if(Conn == null)
        {
            Conn = this;
        }

        //spawnRotation = target.rotation;
    }

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
            GameManager.instance.Array_AllPlayers[0] = PhotonNetwork.Instantiate(array_PlayerType[0], VR_Spawn, Quaternion.Euler(0,180,0));
            // �̱��� ���ӸŴ����� ������ �÷��̾��� �ؽ�Ʈ�� ����
            GameManager.instance.Array_txtWinner[0] = GameManager.instance.Array_AllPlayers[0].GetComponent<VRPlayerMove1>().Txt_winnerText_VR;
        }
        // PC �÷��̾� ����
        else
        {
            //Vector2 originPos = Random.insideUnitCircle * 2.0f;
            //PhotonNetwork.Instantiate("PCPlayerXR", PC_Spawn, Quaternion.identity);
            GameManager.instance.Array_AllPlayers[1] = PhotonNetwork.Instantiate(array_PlayerType[1], PC_Spawn, Quaternion.identity);
            // �̱��� ���ӸŴ����� ������ �÷��̾��� �ؽ�Ʈ�� ����
            GameManager.instance.Array_txtWinner[1] = GameManager.instance.Array_AllPlayers[1].GetComponent<PC_Player_Move>().Txt_winnerText_PC;
        }

    }

}
