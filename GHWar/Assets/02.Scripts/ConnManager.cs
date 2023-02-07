using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;
using System.Data;
using UnityEngine.UI;

// ConnManager에 넣는다. 사실 어느 곳이든 상관지만 게임매니저는 제외
public class ConnManager : MonoBehaviourPunCallbacks
{
    // 포톤서버에 스폰할 플레이어의 오브젝트이름(0이면 VR, 1이면 PC)
    [SerializeField] string[] array_PlayerType = new string[2];

    // 최대 접속인원
    public byte Byte_maxPlayer = 2;

    [Header("VR 플레이어 스폰 위치")]
    public Vector3 VR_Spawn;
    [Header("PC 플레이어 스폰 위치")]
    public Vector3 PC_Spawn;

    // 싱글톤
    public static ConnManager Conn = null;

    private void Awake()
    {
        if(Conn == null)
        {
            Conn = this;
        }

        //spawnRotation = target.rotation;
    }

    // 씬 시작 시 서버에 접속
    void Start()
    {
        PhotonNetwork.GameVersion = "0.1";

        // 플레이어가 사용할 임의의 닉네임
        int num_Player = Random.Range(0, 1000);
        PhotonNetwork.NickName = "Player" + num_Player.ToString();

        // 마스터 클라이언트가 자동으로 동기화시켜줌
        PhotonNetwork.AutomaticallySyncScene = true;

        // 서버 접속(호출 및 호출 실패 시에도 사용하면 재접속 시도)
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버 접속 후 로비 접속 호출
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    // 서버 접속 실패 시 재시도
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("오프라인 : 접속 해제...");
        PhotonNetwork.ConnectUsingSettings();
    }
    // 로비 접속 완료 후 방 접속
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료!");
        // 방에 접속할 수 있는 최대 플레이어 2명
        RoomOptions ro_0 = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = Byte_maxPlayer };
        PhotonNetwork.JoinOrCreateRoom("NetTest", ro_0, TypedLobby.Default);
    }

    // 룸 생성 실패 시 같은 옵션으로 재생성
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError("룸 접속 실패...");
        RoomOptions ro_0 = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = Byte_maxPlayer };
        PhotonNetwork.JoinOrCreateRoom("NetTest", ro_0, TypedLobby.Default);
    }

    // 방 접속과 동시에 플레이어 생성
    public override void OnJoinedRoom()
    {
        Debug.Log("룸 입장!");

        // 원점에서 VR 플레이어 생성
        if (GameManager.instance.IsVR)
        {
            //Vector2 originPos = Random.insideUnitCircle * 2.0f;
            GameManager.instance.Array_AllPlayers[0] = PhotonNetwork.Instantiate(array_PlayerType[0], VR_Spawn, Quaternion.Euler(0,180,0));
            // 싱글톤 게임매니저에 생성된 플레이어의 텍스트를 연결
            GameManager.instance.Array_txtWinner[0] = GameManager.instance.Array_AllPlayers[0].GetComponent<VRPlayerMove1>().Txt_winnerText_VR;
        }
        // PC 플레이어 생성
        else
        {
            //Vector2 originPos = Random.insideUnitCircle * 2.0f;
            //PhotonNetwork.Instantiate("PCPlayerXR", PC_Spawn, Quaternion.identity);
            GameManager.instance.Array_AllPlayers[1] = PhotonNetwork.Instantiate(array_PlayerType[1], PC_Spawn, Quaternion.identity);
            // 싱글톤 게임매니저에 생성된 플레이어의 텍스트를 연결
            GameManager.instance.Array_txtWinner[1] = GameManager.instance.Array_AllPlayers[1].GetComponent<PC_Player_Move>().Txt_winnerText_PC;
        }

    }

}
