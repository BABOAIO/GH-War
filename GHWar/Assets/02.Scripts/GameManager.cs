using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

// 게임매니저 오브젝트에 넣는다.
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    //[SerializeField] List<GameObject> Trees = new List<GameObject>();
    //List<Vector3> v3_treesOriginPos = new List<Vector3>();
    //List<Quaternion> q_treesRotation = new List<Quaternion>();

    //[SerializeField] GameObject obj_FallingArea;

    // 지정 안할 경우, TurretFire 에서 VR 플레이어를 인식 못하는 치명적 오류 발생
    // Turret의 Cannon_set 에 들어있음
    [Header("게임 시작하기 전 비활성화시킬 대포 오브젝트")]
    [SerializeField] List<TurretLookVRTarget1> turrets = new List<TurretLookVRTarget1>();

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

    [Header("게임이 끝났는지 알려주는 변수")]
    public bool B_IsGameOver = false;
    [Header("게임이 시작했는지 알려주는 변수")]
    public bool B_GameStart = false;

    public List<GameObject> o_PlayArea = new List<GameObject>();

    // 스카이박스 초당 회전 값 변수
    [Header("스카이박스 초당 회전 값 변수")]
    public float RotationPerSecond = 2;

    [Header("지형이 무너지는 초기 시간")]
    public float destroyAreaTime = 20f;
    [Header("무너질 지형의 인덱스")]
    public int num_destroyArea = 1;
    [Header("게임 시작 시, 경과된 시간")]
    public float currentTime = 0.0f;

    private void Awake()
    {
        // 싱글톤
        if (instance == null) instance = this;

        for (int i = 0; i < turrets.Count; i++)
        {
            turrets[i].enabled = false;
        }

        // VR 기기인지 체크
        Debug.Log("VR Device = " + IsPresent().ToString());
        IsVR = IsPresent();
    }

    void Start()
    {
        // 실행화면에 대한 해상도 960x640
        Screen.SetResolution(960, 640, FullScreenMode.Windowed);

        // 데이터 송수신 빈도 초당 30으로 설정
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;

        B_GameStart = false; B_IsGameOver = false;

        //for (int i = 0; i < Trees.Count; i++)
        //{
        //    v3_treesOriginPos[i] = Trees[i].GetComponent<SetActiveKinetic>().V3_origonPos;
        //    q_treesRotation[i] = Trees[i].GetComponent<SetActiveKinetic>().Q_origonRot;
        //}

        //v3_treesOriginPos = obj_trees.transform.position;
        //q_treesOriginRot = obj_trees.transform.rotation;
        //obj_FallingArea.SetActive(false);

        StartCoroutine(WaitPlayerText());
        StartCoroutine(WaitPlayer());
    }

    // 띄엄띄엄 텍스트 출력
    IEnumerator WaitPlayerText()
    {
        float delay = 0.1f;

        while (!B_GameStart)
        {
            if (Array_AllPlayers[0])
            {
                switch (Array_txtWinner[0].text)
                {
                    case "":
                        Array_txtWinner[0].text = "W";
                        break;
                    case "W":
                        Array_txtWinner[0].text += "a";
                        break;
                    case "Wa":
                        Array_txtWinner[0].text += "i";
                        break;
                    case "Wai":
                        Array_txtWinner[0].text += "t";
                        break;
                    case "Wait":
                        Array_txtWinner[0].text += " ";
                        break;
                    case "Wait ":
                        Array_txtWinner[0].text += "f";
                        break;
                    case "Wait f":
                        Array_txtWinner[0].text += "o";
                        break;
                    case "Wait fo":
                        Array_txtWinner[0].text += "r";
                        break;
                    case "Wait for":
                        Array_txtWinner[0].text += " ";
                        break;
                    case "Wait for ":
                        Array_txtWinner[0].text += "O";
                        break;
                    case "Wait for O":
                        Array_txtWinner[0].text += "t";
                        break;
                    case "Wait for Ot":
                        Array_txtWinner[0].text += "h";
                        break;
                    case "Wait for Oth":
                        Array_txtWinner[0].text += "e";
                        break;
                    case "Wait for Othe":
                        Array_txtWinner[0].text += "r";
                        break;
                    case "Wait for Other":
                        Array_txtWinner[0].text += " ";
                        break;
                    case "Wait for Other ":
                        Array_txtWinner[0].text += "P";
                        break;
                    case "Wait for Other P":
                        Array_txtWinner[0].text += "l";
                        break;
                    case "Wait for Other Pl":
                        Array_txtWinner[0].text += "a";
                        break;
                    case "Wait for Other Pla":
                        Array_txtWinner[0].text += "y";
                        break;
                    case "Wait for Other Play":
                        Array_txtWinner[0].text += "e";
                        break;
                    case "Wait for Other Playe":
                        Array_txtWinner[0].text += "r";
                        break;
                    case "Wait for Other Player":
                        Array_txtWinner[0].text += ".";
                        break;
                    case "Wait for Other Player.":
                        Array_txtWinner[0].text += ".";
                        break;
                    case "Wait for Other Player..":
                        Array_txtWinner[0].text += ".";
                        yield return new WaitForSeconds(1f);
                        break;
                    case "Wait for Other Player...":
                        Array_txtWinner[0].text = "";
                        break;
                }
            }

            else if (Array_AllPlayers[1])
            {
                switch (Array_txtWinner[1].text)
                {
                    case "":
                        Array_txtWinner[1].text = "W";
                        break;
                    case "W":
                        Array_txtWinner[1].text += "a";
                        break;
                    case "Wa":
                        Array_txtWinner[1].text += "i";
                        break;
                    case "Wai":
                        Array_txtWinner[1].text += "t";
                        break;
                    case "Wait":
                        Array_txtWinner[1].text += " ";
                        break;
                    case "Wait ":
                        Array_txtWinner[1].text += "f";
                        break;
                    case "Wait f":
                        Array_txtWinner[1].text += "o";
                        break;
                    case "Wait fo":
                        Array_txtWinner[1].text += "r";
                        break;
                    case "Wait for":
                        Array_txtWinner[1].text += " ";
                        break;
                    case "Wait for ":
                        Array_txtWinner[1].text += "O";
                        break;
                    case "Wait for O":
                        Array_txtWinner[1].text += "t";
                        break;
                    case "Wait for Ot":
                        Array_txtWinner[1].text += "h";
                        break;
                    case "Wait for Oth":
                        Array_txtWinner[1].text += "e";
                        break;
                    case "Wait for Othe":
                        Array_txtWinner[1].text += "r";
                        break;
                    case "Wait for Other":
                        Array_txtWinner[1].text += " ";
                        break;
                    case "Wait for Other ":
                        Array_txtWinner[1].text += "P";
                        break;
                    case "Wait for Other P":
                        Array_txtWinner[1].text += "l";
                        break;
                    case "Wait for Other Pl":
                        Array_txtWinner[1].text += "a";
                        break;
                    case "Wait for Other Pla":
                        Array_txtWinner[1].text += "y";
                        break;
                    case "Wait for Other Play":
                        Array_txtWinner[1].text += "e";
                        break;
                    case "Wait for Other Playe":
                        Array_txtWinner[1].text += "r";
                        break;
                    case "Wait for Other Player":
                        Array_txtWinner[1].text += ".";
                        break;
                    case "Wait for Other Player.":
                        Array_txtWinner[1].text += ".";
                        break;
                    case "Wait for Other Player..":
                        Array_txtWinner[1].text += ".";
                        yield return new WaitForSeconds(1f);
                        break;
                    case "Wait for Other Player...":
                        Array_txtWinner[1].text = "";
                        break;
                }
            }
            yield return new WaitForSeconds(delay);
        }
    }

    // 1 대 1 한정 다른 플레이어를 대기하는 코루틴
    IEnumerator WaitPlayer()
    {
        string str = "Wait for Other Players...";
        float delay = 2f;

        while (!B_GameStart)
        {
            // 마스터 클라이언트는 항상 존재하기 때문에 구분해놓음
            if (PhotonNetwork.IsMasterClient)
            {
                // ConnManager에서 플레이어가 생성될 때, 싱글턴으로 넣어줌
                if (Array_AllPlayers[0])
                {
                    Debug.Log(str);
                    GameObject o_otherPlayer = GameObject.FindGameObjectWithTag("PC_Player");

                    if (o_otherPlayer)
                    {
                        Array_AllPlayers[1] = o_otherPlayer;
                        Array_txtWinner[1] = Array_AllPlayers[1].GetComponent<PC_Player_Move>().Txt_winnerText_PC;
                        Debug.Log("Game Start!");

                        Array_txtWinner[0].text = "Game Start!";

                        ResetDeathCount(2);
                        Invoke("StartGame", delay);
                        // sound 등 게임시작 알림
                    }
                }
                // ConnManager에서 플레이어가 생성될 때, 싱글턴으로 넣어줌
                else if (Array_AllPlayers[1])
                {
                    //Array_txtWinner[1].text = str;
                    Debug.Log(str);
                    GameObject o_otherPlayer = GameObject.FindGameObjectWithTag("VR_Player");

                    if (o_otherPlayer)
                    {
                        Array_AllPlayers[0] = o_otherPlayer;
                        Array_txtWinner[0] = Array_AllPlayers[0].GetComponent<VRPlayerMove>().Txt_winnerText_VR;
                        Debug.Log("Game Start!");

                        Array_txtWinner[1].text = "Game Start!";

                        ResetDeathCount(2);
                        Invoke("StartGame", delay);
                        // sound 등 게임시작 알림
                    }
                }

            }
            else
            {
                if (Array_AllPlayers[0])
                {
                    //Array_txtWinner[0].text = str;
                    Debug.Log(str);
                    GameObject o_otherPlayer = GameObject.FindGameObjectWithTag("PC_Player");

                    if (o_otherPlayer)
                    {
                        Array_AllPlayers[1] = o_otherPlayer;
                        Array_txtWinner[1] = Array_AllPlayers[1].GetComponent<PC_Player_Move>().Txt_winnerText_PC;
                        Debug.Log("Game Start!");

                        Array_txtWinner[0].text = "Game Start!";

                        ResetDeathCount(2);
                        Invoke("StartGame", delay);
                        // sound 등 게임시작 알림
                    }
                }
                else if (Array_AllPlayers[1])
                {
                    //Array_txtWinner[1].text = str;
                    Debug.Log(str);
                    GameObject o_otherPlayer = GameObject.FindGameObjectWithTag("VR_Player");

                    if (o_otherPlayer)
                    {
                        Array_AllPlayers[0] = o_otherPlayer;
                        Array_txtWinner[0] = Array_AllPlayers[0].GetComponent<VRPlayerMove>().Txt_winnerText_VR;
                        Debug.Log("Game Start!");

                        Array_txtWinner[1].text = "Game Start!";

                        ResetDeathCount(2);
                        Invoke("StartGame", delay);
                        // sound 등 게임시작 알림
                    }
                }
            }
            yield return null;
        }
    }

    // 준비완료 체크 미완
    IEnumerator ReadyToGameStart()
    {
        while (!B_GameStart)
        {
            // 마스터 클라이언트는 항상 존재하기 때문에 구분해놓음
            if (PhotonNetwork.IsMasterClient)
            {

            }
            else
            {
                
            }
            yield return null;
        }
    }

    // 텍스트 초기화 + 게임 스타트
    void StartGame()
    {
        Array_txtWinner[0].text = "";
        Array_txtWinner[1].text = "";
        B_GameStart = true;
    }

    void ResetDeathCount(int i)
    {
        StopCoroutine(WaitPlayerText());
        i_PCDeathCount = i;

        //for (int j = 0; j < i_VRDeathCount; j++)
        //{
        //    Trees[j].transform.position = v3_treesOriginPos[j];
        //    Trees[j].transform.rotation = q_treesRotation[j];
        //}
        ////obj_trees.transform.position = v3_treesOriginPos;
        ////obj_trees.transform.rotation = q_treesOriginRot;
        //obj_FallingArea.SetActive(true);

        // 터렛 활성화 >> Start함수에서 VR플레이어 인식

        for (int j = 0; j < turrets.Count; j++)
        {
            turrets[j].enabled = true;
        }

        // 게임 시작 전, 방해되는 돌들 제거
        GameObject[] tmp_rock = GameObject.FindGameObjectsWithTag("Rock");
        if(tmp_rock != null)
        {
            for (int j = 0; j< tmp_rock.Length; j++)
            {
                Destroy(tmp_rock[j]);
            }

        }
        GameObject[] tmp_smallRock = GameObject.FindGameObjectsWithTag("SmallRock");
        if (tmp_smallRock != null)
        {
            for (int j = 0; j < tmp_smallRock.Length; j++)
            {
                Destroy(tmp_smallRock[j]);
            }
        }
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
        Array_AllPlayers[1].GetComponent<PC_Player_Move>().isDie = true;
        Array_AllPlayers[1].GetComponent<PCPlayerFireArrow>().isDie = true;

        yield return new WaitForSeconds(2f);

        if (!B_IsGameOver)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Array_AllPlayers[1].GetComponent<PC_Player_Move>().a_player.SetBool("Rebirth", true);
                Observable.NextFrame().Subscribe(_ => Array_AllPlayers[1].GetComponent<PC_Player_Move>().a_player.SetBool("Rebirth", false));
                yield return new WaitForSeconds(2f);
                Array_AllPlayers[1].GetComponent<PC_Player_Move>().a_player.SetBool("ReadyNextIdle", true);
                Array_AllPlayers[1].GetComponent<PC_Player_Move>().a_player.Rebind();

                // 일정 시간 무적 부여
                Array_AllPlayers[1].GetComponent<PCPlayerHit>().currentTime = 0;
                Array_AllPlayers[1].GetComponent<PCPlayerHit>().HP = Array_AllPlayers[1].GetComponent<PCPlayerHit>().MaxHP;
                Array_AllPlayers[1].GetComponent<PC_Player_Move>().isDie = false;
                Array_AllPlayers[1].GetComponent<PCPlayerFireArrow>().isDie = false;
            }
            else
            {
                Array_AllPlayers[1].GetComponent<PC_Player_Move>().a_player.SetBool("Rebirth", true);
                Observable.NextFrame().Subscribe(_ => Array_AllPlayers[1].GetComponent<PC_Player_Move>().a_player.SetBool("Rebirth", false));
                yield return new WaitForSeconds(2f);
                Array_AllPlayers[1].GetComponent<PC_Player_Move>().a_player.SetBool("ReadyNextIdle", true);
                Array_AllPlayers[1].GetComponent<PC_Player_Move>().a_player.Rebind();


                // 일정 시간 무적 부여
                Array_AllPlayers[1].GetComponent<PCPlayerHit>().currentTime = 0;
                Array_AllPlayers[1].GetComponent<PCPlayerHit>().HP = Array_AllPlayers[1].GetComponent<PCPlayerHit>().MaxHP;
                Array_AllPlayers[1].GetComponent<PC_Player_Move>().isDie = false;
                Array_AllPlayers[1].GetComponent<PCPlayerFireArrow>().isDie = false;
            }
        }
    }

    public void CheckRebirthPCPlayer()
    {
        StartCoroutine(RebirthPCPlayer());
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
            // 게임매니저에서 마스터클라이언트와 이즈마인은 같음
            if (PhotonNetwork.IsMasterClient)
            {
                // PC가 이겼을 경우, VR이 나갔을 경우,
                if (Array_AllPlayers[0].GetComponentInChildren<VRPlayerHit>().HP <= 0 || Array_AllPlayers[0] == null)
                {
                    B_IsGameOver = true;
                    currentTime = 0;

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
                // VR이 이겼을 경우, PC가 나갔을 경우,
                //else if (Array_AllPlayers[1].GetComponent<PCPlayerHit>().HP <= 0)
                else if (i_PCDeathCount <= 0 || Array_AllPlayers[1] == null)
                {
                    B_IsGameOver = true;
                    currentTime = 0;

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
                // PC가 이겼을 경우, VR이 나갔을 경우
                //if (Array_AllPlayers[0].GetComponent<VRPlayerMove1>().o_vrFace.GetComponent<VRPlayerHit>().HP <= 0)
                if (Array_AllPlayers[0].GetComponentInChildren<VRPlayerHit>().HP <= 0 || Array_AllPlayers[0] == null)
                {
                    B_IsGameOver = true;
                    currentTime = 0;

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
                // VR이 이겼을 경우, PC가 나갔을 경우,
                //else if (Array_AllPlayers[1].GetComponent<PCPlayerHit>().HP <= 0)
                else if (i_PCDeathCount <= 0 || Array_AllPlayers[1] == null)
                {
                    B_IsGameOver = true;
                    currentTime = 0;

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
        NarrowGround();
        UpdatePhotonNetwork();
        RotateSkyBox(RotationPerSecond);

        if (Input.GetKeyUp(KeyCode.Alpha3)) 
        { 
            GameObject tmp =
            PhotonNetwork.Instantiate("Rock", new Vector3(0.22314f, 0f, -13.8f), Quaternion.identity);
            tmp.layer = LayerMask.NameToLayer("PCPlayer");
            Destroy(tmp, 1.5f);
        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            B_GameStart = !B_GameStart;
        }
    }

    // 일정 시간이 지나면 지형 붕괴 코루틴을 발생시키는 스크립트
    void NarrowGround()
    {
        if (B_GameStart)
        {
            currentTime += Time.deltaTime;
            // *2 를 하지 않으면 붕괴될 10초가 중복되서 어색함, 10초를 넘기지 않으면 *2
            if (currentTime > destroyAreaTime * num_destroyArea * 2)
            {
                if (o_PlayArea.Count > num_destroyArea)
                {
                    StartCoroutine(DelayedDestructionArea(o_PlayArea[num_destroyArea - 1]));
                    //o_PlayArea[num_destroyArea - 1] = null;
                    num_destroyArea++;
                }
            }
        }
    }

    // 스카이박스 회전 스크립트
    void RotateSkyBox(float rotationSpeedPerSecond)
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeedPerSecond);
    }

    public IEnumerator LeaveEnd(float delay)
    {
        yield return new WaitForSeconds(delay);
        LeaveRoom();
    }

    public void LeaveRoom()
    {
        // 초기화
        Array_AllPlayers[0] = null;
        Array_AllPlayers[1] = null;

        B_IsGameOver = false;
        B_GameStart = false;

        ResetDeathCount(2);

        // Canvas, Animator State 초기화

        // 승패 결정 시 서버종료
        PhotonNetwork.LeaveRoom();
        //Application.Quit();
    }

    // 일정 시간이 지나면 발동하는 지형붕괴 코루틴
    // PC 플레이어에게는 10초 전에 알려준다. 다른 지형에 닿으면 그 지형의 지형붕괴시간을 보여준다.
    IEnumerator DelayedDestructionArea(GameObject _area)
    {
        FractureTest fractureTest = _area.GetComponent<FractureTest>();
        fractureTest.i_destroyTime = 10;
        AudioSource as_fracture = fractureTest.as_parent;
        int maxCouint = fractureTest.i_destroyTime;
        for (int i = 0; i < maxCouint; i++)
        {
            yield return new WaitForSeconds(1f);
            fractureTest.i_destroyTime -= 1;
            print(fractureTest.i_destroyTime);

            if (fractureTest.i_destroyTime <= 0)
            {
                as_fracture.Stop();
                fractureTest.DestructionThisArea();
            }
            if (fractureTest.i_destroyTime % 3 == 0 && fractureTest.i_destroyTime != 0)
            {
                as_fracture.Stop();
                as_fracture.PlayOneShot(fractureTest.ac_shake);
                _area.transform.DOShakePosition(0.5f, 3.0f / (fractureTest.i_destroyTime));
            }
        }

        as_fracture.PlayOneShot(fractureTest.ac_destruction);
        yield return new WaitForSeconds(1f);
        fractureTest.i_destroyTime = 100;
    }

}
