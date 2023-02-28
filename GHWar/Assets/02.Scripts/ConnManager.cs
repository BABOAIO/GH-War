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
    public string[] array_PlayerType = new string[2];

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

    // ���� ������
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
        RoomOptions ro_0 = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = Byte_maxPlayer, CleanupCacheOnLeave = false };
        PhotonNetwork.JoinOrCreateRoom("NetTest", ro_0, TypedLobby.Default);
    }

    // �� ���� ���� �� ���� �ɼ����� �����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError("�� ���� ����...");
        RoomOptions ro_0 = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = Byte_maxPlayer, CleanupCacheOnLeave = false };
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
            GameManager.instance.VRPlayer = PhotonNetwork.Instantiate(array_PlayerType[0], VR_Spawn, Quaternion.Euler(0,200,0));
            // �̱��� ���ӸŴ����� ������ �÷��̾��� �ؽ�Ʈ�� ����
            GameManager.instance.VRText = GameManager.instance.VRPlayer.GetComponent<VRPlayerMove>().Txt_winnerText_VR;
        }
        // PC �÷��̾� ����
        else
        {
            //Vector2 originPos = Random.insideUnitCircle * 2.0f;
            //PhotonNetwork.Instantiate("PCPlayerXR", PC_Spawn, Quaternion.identity);
            GameManager.instance.PCPlayer = PhotonNetwork.Instantiate(array_PlayerType[1], PC_Spawn, Quaternion.identity);
            // �̱��� ���ӸŴ����� ������ �÷��̾��� �ؽ�Ʈ�� ����
            GameManager.instance.PCText = GameManager.instance.PCPlayer.GetComponent<PC_Player_Move>().Txt_winnerText_PC;
        }
    }


    [PunRPC]
    void DESTROY()
    {
        GameObject tmp = GameObject.FindGameObjectWithTag("PC_Player");
        print("ã�Ҵ�!");
        PhotonNetwork.Destroy(tmp);
        print("�׿���!");
    }
}
