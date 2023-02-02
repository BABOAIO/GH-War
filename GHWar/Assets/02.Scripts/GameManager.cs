using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using DG.Tweening;
using UniRx.Triggers;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager instance;

    // VR인지 PC인지를 구분
    [Header("VR 상태 변수")]
    public bool IsVR;

    // VR 플레이어는 0, PC 플레이어는 1
    [Header("상태텍스트 / 0이면 VR, 1이면 PC")]
    public Text[] Array_txtWinner = new Text[2];

    // VR 플레이어는 0, PC 플레이어는 1
    [Header("대전 플레이어 / 0이면 VR, 1이면 PC")]
    public GameObject[] Array_AllPlayers = new GameObject[2];

    // 게임 시작 시 리셋
    [Header("PC 플레이어 목숨 수")]
    public int i_PCDeathCount = 0;
    [Header("VR 플레이어 목숨 수")]
    public int i_VRDeathCount = 0;

    [Header("게임이 끝났는지 알려주는 변수")]
    public bool B_IsGameOver = false;
    [Header("게임이 시작했는지 알려주는 변수")]
    public bool B_GameStart = false;

    // 스카이박스 초당 회전 값 변수
    [Header("스카이박스 초당 회전 값 변수")]
    public float RotationPerSecond = 2;

    AudioSource as_gm;

    private void Awake()
    {
        // 싱글톤
        if (instance == null) instance = this;

        // VR 기기인지 체크
        Debug.Log("VR Device = " + IsPresent().ToString());
        IsVR = IsPresent();

        //ResetDeathCount(2);
    }

    void Start()
    {
        // 실행화면에 대한 해상도 960x640
        Screen.SetResolution(960, 640, FullScreenMode.Windowed);

        // 데이터 송수신 빈도 초당 30으로 설정
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;

        as_gm = GetComponent<AudioSource>();

        B_GameStart = false; B_IsGameOver = false;

        ResetDeathCount(2);

        StartCoroutine(WaitPlayer());
    }

    // 1 대 1 한정 다른 플레이어를 대기하는 함수
    IEnumerator WaitPlayer()
    {
        string str = "Wait for Other Players...";
        float delay = 1f;

        while (!B_GameStart)
        {

            // 마스터 클라이언트는 항상 존재하기 때문에 구분해놓음
            if (PhotonNetwork.IsMasterClient)
            {
                // ConnManager에서 플레이어가 생성될 때, 싱글턴으로 넣어줌
                if (Array_AllPlayers[0])
                {
                    Array_txtWinner[0].text = str;
                    Debug.Log(str);
                    GameObject o_otherPlayer = GameObject.FindGameObjectWithTag("PC_Player");

                    if (o_otherPlayer)
                    {
                        Array_AllPlayers[1] = o_otherPlayer;
                        Array_txtWinner[1] = Array_AllPlayers[1].GetComponent<PC_Player_Move>().Txt_winnerText_PC;
                        Debug.Log("Game Start!");

                        Array_txtWinner[0].text = "Game Start!";
                        Invoke("ResetText", delay);
                        // sound 등 게임시작 알림
                    }
                }
                // ConnManager에서 플레이어가 생성될 때, 싱글턴으로 넣어줌
                else if (Array_AllPlayers[1])
                {
                    Array_txtWinner[1].text = str;
                    Debug.Log(str);
                    GameObject o_otherPlayer = GameObject.FindGameObjectWithTag("VR_Player");

                    if (o_otherPlayer)
                    {
                        Array_AllPlayers[0] = o_otherPlayer;
                        Array_txtWinner[0] = Array_AllPlayers[0].GetComponent<VRPlayerMove1>().Txt_winnerText_VR;
                        Debug.Log("Game Start!");

                        Array_txtWinner[0].text = "Game Start!";
                        Invoke("ResetText", delay);
                        // sound 등 게임시작 알림
                    }
                }

            }
            else
            {
                if (Array_AllPlayers[0])
                {
                    Array_txtWinner[0].text = str;
                    Debug.Log(str);
                    GameObject o_otherPlayer = GameObject.FindGameObjectWithTag("PC_Player");

                    if (o_otherPlayer)
                    {
                        Array_AllPlayers[1] = o_otherPlayer;
                        Array_txtWinner[1] = Array_AllPlayers[1].GetComponent<PC_Player_Move>().Txt_winnerText_PC;
                        Debug.Log("Game Start!");

                        Array_txtWinner[0].text = "Game Start!";
                        Invoke("ResetText", delay);
                        // sound 등 게임시작 알림
                    }
                }
                else if (Array_AllPlayers[1])
                {
                    Array_txtWinner[1].text = str;
                    Debug.Log(str);
                    GameObject o_otherPlayer = GameObject.FindGameObjectWithTag("VR_Player");

                    if (o_otherPlayer)
                    {
                        Array_AllPlayers[0] = o_otherPlayer;
                        Array_txtWinner[0] = Array_AllPlayers[0].GetComponent<VRPlayerMove1>().Txt_winnerText_VR;
                        Debug.Log("Game Start!");

                        Array_txtWinner[0].text = "Game Start!";
                        Invoke("ResetText", delay);
                        // sound 등 게임시작 알림
                    }
                }
            }
            yield return null;
        }

        Array_txtWinner[1].text = "Game Start!";
    }

    void ResetText()
    {
        Array_txtWinner[0].text = "";
        Array_txtWinner[1].text = "";
        B_GameStart = true;
    }

    void ResetDeathCount(int i)
    {
        i_VRDeathCount = i;
        i_PCDeathCount = i;
    }

    // VR 기기 연결 상태 확인
    public static bool IsPresent()
    {
        var list_xrDisplaySubsystem = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances<XRDisplaySubsystem>(list_xrDisplaySubsystem);

        foreach(var xrDisplay in list_xrDisplaySubsystem)
        {
            if (xrDisplay.running)
            {
                return true;
            }
        }
        return false;
    }

    // PC 플레이어 2초 후 그 자리에서 부활, 애니메이션 초기화를 하지 않을 경우 꼬일 수 있으니 주의!
    // 추가로 움직임을 멈추게 하는 장치 필요
    IEnumerator RebirthPCPlayer()
    {
        // 2초 동안 움직임 방지
        Array_AllPlayers[1].GetComponent<PC_Player_Move>().enabled = false;

        yield return new WaitForSeconds(2f);

        if (PhotonNetwork.IsMasterClient)
        {
            Array_AllPlayers[1].GetComponent<PC_Player_Move>().enabled = true;
            --i_PCDeathCount;

            // 일정 시간 무적 부여
            Array_AllPlayers[1].GetComponent<PCPlayerHit>().currentTime = 0;
        }
        else
        {
            Array_AllPlayers[1].GetComponent<PC_Player_Move>().enabled = true;
            --i_PCDeathCount;
            // anim idle

            // 일정 시간 무적 부여
            Array_AllPlayers[1].GetComponent<PCPlayerHit>().currentTime = 0;
        }
    }
    IEnumerator RebirthVRPlayer()
    {
        // 2초 동안 움직임 방지
        Array_AllPlayers[0].GetComponent<VRPlayerMove1>().enabled = false;

        yield return new WaitForSeconds(2f);

        if (PhotonNetwork.IsMasterClient)
        {
            Array_AllPlayers[0].GetComponent<VRPlayerMove1>().enabled = true;
            --i_PCDeathCount;

            // 일정 시간 무적 부여
            Array_AllPlayers[0].GetComponent<VRPlayerMove1>().o_vrFace.GetComponent<VRPlayerHit>().currentTime = 0;
            //Array_AllPlayers[0].GetComponent<VRPlayerHit>().currentTime = 0;
        }
        else
        {
            Array_AllPlayers[1].GetComponent<VRPlayerMove1>().enabled = true;
            --i_PCDeathCount;
            // anim idle

            // 일정 시간 무적 부여
            Array_AllPlayers[0].GetComponent<VRPlayerMove1>().o_vrFace.GetComponent<VRPlayerHit>().currentTime = 0;
        }
    }

    public void CheckRebirthPCPlayer()
    {
        StartCoroutine(RebirthPCPlayer());
    }

    public void CheckRebirthVRPlayer()
    {
        StartCoroutine(RebirthVRPlayer());
    }

    // 업데이트문으로 돌릴 포톤네트워크 함수
    void UpdatePhotonNetwork()
    {
        if (B_GameStart)
        {
            CheckWinner();
        }
    }

    // 승자 결정
    void CheckWinner()
    {
        if (!B_IsGameOver)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // PC가 이겼을 경우
                //if (Array_AllPlayers[0].GetComponent<VRPlayerMove1>().o_vrFace.GetComponent<VRPlayerHit>().HP <= 0)
                if (i_VRDeathCount <= 0)
                {
                    B_IsGameOver = true;

                    if (Array_txtWinner[0] != null)
                    {
                        Array_txtWinner[0].text = "VR Player Lose..";
                    }
                    if (Array_txtWinner[1] != null)
                    {
                        Array_txtWinner[1].text = "PC Player Win!!";
                    }

                    Debug.Log("PC Player Win!!");
                    // 3초 뒤 서버 종료
                    StartCoroutine(LeaveEnd(3f));
                }
                // VR이 이겼을 경우
                //else if (Array_AllPlayers[1].GetComponent<PCPlayerHit>().HP <= 0)
                else if (i_PCDeathCount <= 0)
                        {
                    B_IsGameOver = true;

                    if (Array_txtWinner[0] != null)
                    {
                        Array_txtWinner[0].text = "VR Player Win!!";
                    }
                    if (Array_txtWinner[1] != null)
                    {
                        Array_txtWinner[1].text = "PC Player Lose..";
                    }

                    Debug.Log("VR Player Win!!");
                    // 3초 뒤 서버 종료
                    StartCoroutine(LeaveEnd(3f));
                }
            }
            else
            {
                // PC가 이겼을 경우
                //if (Array_AllPlayers[0].GetComponent<VRPlayerMove1>().o_vrFace.GetComponent<VRPlayerHit>().HP <= 0)
                if (i_VRDeathCount <= 0)
                {
                    B_IsGameOver = true;

                    // canvas 추가, 부활막기
                    if (Array_txtWinner[0] != null)
                    {
                        Array_txtWinner[0].text = "VR Player Lose..";
                    }
                    if (Array_txtWinner[1] != null)
                    {
                        Array_txtWinner[1].text = "PC Player Win!!";
                    }

                    Debug.Log("PC Player Win!!");
                    // 3초 뒤 서버 종료
                    StartCoroutine(LeaveEnd(3f));
                }
                // VR이 이겼을 경우
                //else if (Array_AllPlayers[1].GetComponent<PCPlayerHit>().HP <= 0)
                else if (i_PCDeathCount <= 0)
                {
                    B_IsGameOver = true;

                    // canvas 추가
                    if (Array_txtWinner[0] != null)
                    {
                        Array_txtWinner[0].text = "VR Player Win!!";
                    }
                    if (Array_txtWinner[1] != null)
                    {
                        Array_txtWinner[1].text = "PC Player Lose..";
                    }

                    Debug.Log("VR Player Win!!");
                    // 3초 뒤 서버 종료
                    StartCoroutine(LeaveEnd(3f));
                }
            }
        }
    }

    void Update()
    {
        UpdatePhotonNetwork();
        RotateSkyBox(RotationPerSecond);

        if (as_gm.isPlaying) { return; }
        
        as_gm.Play();
    }

    void RotateSkyBox(float rotationSpeedPerSecond)
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeedPerSecond);
    }

    //[PunRPC]
    //public void PCPlayerDeath()
    //{
    //    i_PCDeathCount++;
    //}

    //[PunRPC]
    //public void VRPlayerDeath()
    //{
    //    i_VRDeathCount++;
    //}

    public IEnumerator LeaveEnd(float delay)
    {
        yield return new WaitForSeconds(delay);
        LeaveRoom();
    }

    public void LeaveRoom()
    {
        Array_AllPlayers[0] = null;
        Array_AllPlayers[1] = null;

        B_IsGameOver = false;
        B_GameStart = false;

        ResetDeathCount(2);

        // Canvas, Animator State 초기화

        // 승패 결정 시 게임종료
        PhotonNetwork.LeaveRoom();
        //Application.Quit();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (stream.IsWriting)
        //{
        //    if (IsVR)
        //    {
        //        stream.SendNext(i_VRDeathCount);
        //    }
        //}
        //if (stream.IsReading)
        //{
        //    if(!IsVR)
        //    {
        //        i_VRDeathCount = (int)stream.ReceiveNext();
        //    }
        //}
    }

    
}
