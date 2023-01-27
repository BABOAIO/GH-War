using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager instance;

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
    [Header("VR �÷��̾� ��� ��")]
    public int i_VRDeathCount = 0;

    [Header("������ �������� �˷��ִ� ����")]
    public bool B_IsGameOver = false;
    [Header("������ �����ߴ��� �˷��ִ� ����")]
    public bool B_GameStart = false;

    // ��ī�̹ڽ� �ʴ� ȸ�� �� ����
    [Header("��ī�̹ڽ� �ʴ� ȸ�� �� ����")]
    public float RotationPerSecond = 2;

    AudioSource as_gm;

    // 1 �� 1 ���� �ٸ� �÷��̾ ����ϴ� �Լ�
    IEnumerator WaitPlayer()
    {
        while (!B_GameStart)
        {
            // ������ Ŭ���̾�Ʈ�� �׻� �����ϱ� ������ �����س���
            if (PhotonNetwork.IsMasterClient)
            {
                // ConnManager���� �÷��̾ ������ ��, �̱������� �־���
                if (Array_AllPlayers[0])
                {
                    Debug.Log("Wait for Other Player...");
                    GameObject o_otherPlayer = GameObject.FindGameObjectWithTag("PC_Player");

                    if (o_otherPlayer)
                    {
                        Array_AllPlayers[1] = o_otherPlayer;
                        Array_txtWinner[1] = Array_AllPlayers[1].GetComponent<PC_Player_Move>().Txt_winnerText_PC;
                        Debug.Log("Game Start!");
                        B_GameStart= true;
                        // sound �� ���ӽ��� �˸�
                    }
                }
                // ConnManager���� �÷��̾ ������ ��, �̱������� �־���
                else if (Array_AllPlayers[1])
                {
                    Debug.Log("Wait for Other Player...");
                    GameObject o_otherPlayer = GameObject.FindGameObjectWithTag("VR_Player");

                    if (o_otherPlayer)
                    {
                        Array_AllPlayers[0] = o_otherPlayer;
                        Array_txtWinner[0] = Array_AllPlayers[0].GetComponent<VRPlayerMove1>().Txt_winnerText_VR;
                        Debug.Log("Game Start!");
                        B_GameStart = true;

                        // sound �� ���ӽ��� �˸�
                    }
                }

            }
            else
            {
                if (Array_AllPlayers[0])
                {
                    Debug.Log("Wait for Other Player...");
                    GameObject o_otherPlayer = GameObject.FindGameObjectWithTag("PC_Player");

                    if (o_otherPlayer)
                    {
                        Array_AllPlayers[1] = o_otherPlayer;
                        Array_txtWinner[1] = Array_AllPlayers[1].GetComponent<PC_Player_Move>().Txt_winnerText_PC;
                        Debug.Log("Game Start!");
                        B_GameStart = true;
                        // sound �� ���ӽ��� �˸�
                    }
                }
                else if (Array_AllPlayers[1])
                {
                    Debug.Log("Wait for Other Player...");
                    GameObject o_otherPlayer = GameObject.FindGameObjectWithTag("VR_Player");

                    if (o_otherPlayer)
                    {
                        Array_AllPlayers[0] = o_otherPlayer;
                        Array_txtWinner[0] = Array_AllPlayers[0].GetComponent<VRPlayerMove1>().Txt_winnerText_VR;
                        Debug.Log("Game Start!");
                        B_GameStart = true;

                        // sound �� ���ӽ��� �˸�
                    }
                }
            }
            yield return null;
        }
    }

    void ResetDeathCount(int i)
    {
        i_VRDeathCount = i;
        i_PCDeathCount = i;
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
        Array_AllPlayers[1].GetComponent<PC_Player_Move>().enabled = false;

        yield return new WaitForSeconds(2f);

        if (PhotonNetwork.IsMasterClient)
        {
            Array_AllPlayers[1].GetComponent<PC_Player_Move>().enabled = true;
            --i_PCDeathCount;

            // ���� �ð� ���� �ο�
            Array_AllPlayers[1].GetComponent<PCPlayerHit>().currentTime = 0;
        }
        else
        {
            Array_AllPlayers[1].GetComponent<PC_Player_Move>().enabled = true;
            --i_PCDeathCount;
            // anim idle

            // ���� �ð� ���� �ο�
            Array_AllPlayers[1].GetComponent<PCPlayerHit>().currentTime = 0;
        }
    }
    IEnumerator RebirthVRPlayer()
    {
        // 2�� ���� ������ ����
        Array_AllPlayers[0].GetComponent<VRPlayerMove1>().enabled = false;

        yield return new WaitForSeconds(2f);

        if (PhotonNetwork.IsMasterClient)
        {
            Array_AllPlayers[1].GetComponent<VRPlayerMove1>().enabled = true;
            --i_PCDeathCount;

            // ���� �ð� ���� �ο�
            Array_AllPlayers[1].GetComponent<VRPlayerHit>().currentTime = 0;
        }
        else
        {
            Array_AllPlayers[1].GetComponent<VRPlayerMove1>().enabled = true;
            --i_PCDeathCount;
            // anim idle

            // ���� �ð� ���� �ο�
            Array_AllPlayers[1].GetComponent<VRPlayerHit>().currentTime = 0;
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
            if (PhotonNetwork.IsMasterClient)
            {
                // PC�� �̰��� ���
                if (Array_AllPlayers[0].GetComponent<VRPlayerHit>().HP <= 0)
                //if (i_VRDeathCount <= 0) 
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
                    // 3�� �� ���� ����
                    StartCoroutine(LeaveEnd(3f));
                }
                // VR�� �̰��� ���
                else if (Array_AllPlayers[1].GetComponent<PCPlayerHit>().HP <= 0)
                //else if (i_PCDeathCount <= 0)
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
                    // 3�� �� ���� ����
                    StartCoroutine(LeaveEnd(3f));
                }
            }
            else
            {
                // PC�� �̰��� ���
                if (Array_AllPlayers[0].GetComponent<VRPlayerHit>().HP <= 0)
                {
                    B_IsGameOver = true;

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
                // VR�� �̰��� ���
                else if (Array_AllPlayers[1].GetComponent<PCPlayerHit>().HP <= 0)
                {
                    B_IsGameOver = true;

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

    private void Awake()
    {
        // �̱���
        if (instance == null) instance = this;

        // VR ������� üũ
        Debug.Log("VR Device = " + IsPresent().ToString());
        IsVR = IsPresent();

        //ResetDeathCount(2);
    }

    void Start()
    {
        // ����ȭ�鿡 ���� �ػ� 960x640
        Screen.SetResolution(960, 640, FullScreenMode.Windowed);

        // ������ �ۼ��� �� �ʴ� 30���� ����
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;

        as_gm = GetComponent<AudioSource>();

        StartCoroutine(WaitPlayer());
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

    public IEnumerator LeaveEnd(float ftime)
    {
        yield return new WaitForSeconds(ftime);
        LeaveRoom();
    }

    public void LeaveRoom()
    {
        Array_AllPlayers[0] = null;
        Array_AllPlayers[1] = null;

        B_IsGameOver = false;
        B_GameStart = false;

        // Canvas, Animator State �ʱ�ȭ

        // ���� ���� �� ��������
        PhotonNetwork.LeaveRoom();
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
