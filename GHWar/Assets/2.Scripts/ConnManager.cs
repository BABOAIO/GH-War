using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;
using System.Data;

public class ConnManager : MonoBehaviourPunCallbacks
{
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
        RoomOptions ro_0 = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom("NetTest", ro_0, TypedLobby.Default);
    }

    // 룸 생성 실패 시 같은 옵션으로 재생성
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError("룸 접속 실패...");
        RoomOptions ro_0 = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom("NetTest", ro_0, TypedLobby.Default);
    }

    // 방 접속과 동시에 플레이어 생성
    public override void OnJoinedRoom()
    {
        Debug.Log("룸 입장!");

        // 원점에서 VR 플레이어 생성
        if (GameManager.instance.isVR)
        {
            Vector2 originPos = Random.insideUnitCircle * 2.0f;
            PhotonNetwork.Instantiate("VRPlayerXR", Vector3.zero, Quaternion.identity);
        }
        // 반경 2m 이내에 랜덤 위치에서 PC 플레이어 생성
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
