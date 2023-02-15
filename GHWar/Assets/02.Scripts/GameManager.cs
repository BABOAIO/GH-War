using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

// ���ӸŴ��� ������Ʈ�� �ִ´�.
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    //[SerializeField] List<GameObject> Trees = new List<GameObject>();
    //List<Vector3> v3_treesOriginPos = new List<Vector3>();
    //List<Quaternion> q_treesRotation = new List<Quaternion>();

    //[SerializeField] GameObject obj_FallingArea;

    // ���� ���� ���, TurretFire ���� VR �÷��̾ �ν� ���ϴ� ġ���� ���� �߻�
    // Turret�� Cannon_set �� �������
    [Header("���� �����ϱ� �� ��Ȱ��ȭ��ų ���� ������Ʈ")]
    [SerializeField] List<TurretLookVRTarget1> turrets = new List<TurretLookVRTarget1>();

    // VR���� PC������ ����
    [Header("VR ���� ����")]
    public bool IsVR;

    // VR �÷��̾�� 0, PC �÷��̾�� 1
    [Header("�����ؽ�Ʈ / 0�̸� VR, 1�̸� PC")]
    public Text[] Array_txtWinner = new Text[2];

    // VR �÷��̾�� 0, PC �÷��̾�� 1
    [Header("���� �÷��̾� / 0�̸� VR, 1�̸� PC")]
    public GameObject[] Array_AllPlayers = new GameObject[2];

    // ���� ���� �� ����
    [Header("PC �÷��̾� ��� ��")]
    public int i_PCDeathCount = 0;

    [Header("������ �������� �˷��ִ� ����")]
    public bool B_IsGameOver = false;
    [Header("������ �����ߴ��� �˷��ִ� ����")]
    public bool B_GameStart = false;

    public List<GameObject> o_PlayArea = new List<GameObject>();

    // ��ī�̹ڽ� �ʴ� ȸ�� �� ����
    [Header("��ī�̹ڽ� �ʴ� ȸ�� �� ����")]
    public float RotationPerSecond = 2;

    [Header("������ �������� �ʱ� �ð�")]
    public float destroyAreaTime = 20f;
    [Header("������ ������ �ε���")]
    public int num_destroyArea = 1;
    [Header("���� ���� ��, ����� �ð�")]
    public float currentTime = 0.0f;

    private void Awake()
    {
        // �̱���
        if (instance == null) instance = this;

        for (int i = 0; i < turrets.Count; i++)
        {
            turrets[i].enabled = false;
        }

        // VR ������� üũ
        Debug.Log("VR Device = " + IsPresent().ToString());
        IsVR = IsPresent();
    }

    void Start()
    {
        // ����ȭ�鿡 ���� �ػ� 960x640
        Screen.SetResolution(960, 640, FullScreenMode.Windowed);

        // ������ �ۼ��� �� �ʴ� 30���� ����
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

    // ������ �ؽ�Ʈ ���
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

    // 1 �� 1 ���� �ٸ� �÷��̾ ����ϴ� �ڷ�ƾ
    IEnumerator WaitPlayer()
    {
        string str = "Wait for Other Players...";
        float delay = 2f;

        while (!B_GameStart)
        {
            // ������ Ŭ���̾�Ʈ�� �׻� �����ϱ� ������ �����س���
            if (PhotonNetwork.IsMasterClient)
            {
                // ConnManager���� �÷��̾ ������ ��, �̱������� �־���
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
                        // sound �� ���ӽ��� �˸�
                    }
                }
                // ConnManager���� �÷��̾ ������ ��, �̱������� �־���
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
                        // sound �� ���ӽ��� �˸�
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
                        // sound �� ���ӽ��� �˸�
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
                        // sound �� ���ӽ��� �˸�
                    }
                }
            }
            yield return null;
        }
    }

    // �غ�Ϸ� üũ �̿�
    IEnumerator ReadyToGameStart()
    {
        while (!B_GameStart)
        {
            // ������ Ŭ���̾�Ʈ�� �׻� �����ϱ� ������ �����س���
            if (PhotonNetwork.IsMasterClient)
            {

            }
            else
            {
                
            }
            yield return null;
        }
    }

    // �ؽ�Ʈ �ʱ�ȭ + ���� ��ŸƮ
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

        // �ͷ� Ȱ��ȭ >> Start�Լ����� VR�÷��̾� �ν�

        for (int j = 0; j < turrets.Count; j++)
        {
            turrets[j].enabled = true;
        }

        // ���� ���� ��, ���صǴ� ���� ����
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

    // VR ��� ���� ���� Ȯ��
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

    // PC �÷��̾� 2�� �� �� �ڸ����� ��Ȱ, �ִϸ��̼� �ʱ�ȭ�� ���� ���� ��� ���� �� ������ ����!
    // �߰��� �������� ���߰� �ϴ� ��ġ �ʿ�
    IEnumerator RebirthPCPlayer()
    {
        // 2�� ���� ������ ����
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

                // ���� �ð� ���� �ο�
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


                // ���� �ð� ���� �ο�
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

    // ������Ʈ������ ���� �����Ʈ��ũ �Լ�
    void UpdatePhotonNetwork()
    {
        if (B_GameStart)
        {
            CheckWinner();
        }
    }

    // ���� ����
    void CheckWinner()
    {
        if (!B_IsGameOver)
        {
            // ���ӸŴ������� ������Ŭ���̾�Ʈ�� ������� ����
            if (PhotonNetwork.IsMasterClient)
            {
                // PC�� �̰��� ���, VR�� ������ ���,
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
                    // 3�� �� ���� ����
                    StartCoroutine(LeaveEnd(3f));
                }
                // VR�� �̰��� ���, PC�� ������ ���,
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
                    // 3�� �� ���� ����
                    StartCoroutine(LeaveEnd(3f));
                }
            }
            else
            {
                // PC�� �̰��� ���, VR�� ������ ���
                //if (Array_AllPlayers[0].GetComponent<VRPlayerMove1>().o_vrFace.GetComponent<VRPlayerHit>().HP <= 0)
                if (Array_AllPlayers[0].GetComponentInChildren<VRPlayerHit>().HP <= 0 || Array_AllPlayers[0] == null)
                {
                    B_IsGameOver = true;
                    currentTime = 0;

                    // canvas �߰�, ��Ȱ����
                    if (Array_txtWinner[0] != null)
                    {
                        Array_txtWinner[0].text = "VR Player Lose..";
                    }
                    if (Array_txtWinner[1] != null)
                    {
                        Array_txtWinner[1].text = "PC Player Win!!";
                    }

                    Debug.Log("PC Player Win!!");
                    // 3�� �� ���� ����
                    StartCoroutine(LeaveEnd(3f));
                }
                // VR�� �̰��� ���, PC�� ������ ���,
                //else if (Array_AllPlayers[1].GetComponent<PCPlayerHit>().HP <= 0)
                else if (i_PCDeathCount <= 0 || Array_AllPlayers[1] == null)
                {
                    B_IsGameOver = true;
                    currentTime = 0;

                    // canvas �߰�
                    if (Array_txtWinner[0] != null)
                    {
                        Array_txtWinner[0].text = "VR Player Win!!";
                    }
                    if (Array_txtWinner[1] != null)
                    {
                        Array_txtWinner[1].text = "PC Player Lose..";
                    }

                    Debug.Log("VR Player Win!!");
                    // 3�� �� ���� ����
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

    // ���� �ð��� ������ ���� �ر� �ڷ�ƾ�� �߻���Ű�� ��ũ��Ʈ
    void NarrowGround()
    {
        if (B_GameStart)
        {
            currentTime += Time.deltaTime;
            // *2 �� ���� ������ �ر��� 10�ʰ� �ߺ��Ǽ� �����, 10�ʸ� �ѱ��� ������ *2
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

    // ��ī�̹ڽ� ȸ�� ��ũ��Ʈ
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
        // �ʱ�ȭ
        Array_AllPlayers[0] = null;
        Array_AllPlayers[1] = null;

        B_IsGameOver = false;
        B_GameStart = false;

        ResetDeathCount(2);

        // Canvas, Animator State �ʱ�ȭ

        // ���� ���� �� ��������
        PhotonNetwork.LeaveRoom();
        //Application.Quit();
    }

    // ���� �ð��� ������ �ߵ��ϴ� �����ر� �ڷ�ƾ
    // PC �÷��̾�Դ� 10�� ���� �˷��ش�. �ٸ� ������ ������ �� ������ �����ر��ð��� �����ش�.
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
