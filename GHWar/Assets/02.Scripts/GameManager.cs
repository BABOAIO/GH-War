using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager instance;

    // VR���� PC������ ����
    public bool IsVR;

    // VR �÷��̾�� 0, PC �÷��̾�� 1
    public Text[] Array_txtWinner = new Text[2];

    // VR �÷��̾�� 0, PC �÷��̾�� 1
    public GameObject[] Array_AllPlayers = new GameObject[2];

    [SerializeField] int i_PCDeathCount = 0;
    [SerializeField] int i_VRDeathCount = 0;

    public bool B_IsGameOver = false;
    public bool B_GameStart = false;

    // ��ī�̹ڽ� �ʴ� ȸ�� �� ����
    public float RotationPerSecond = 2; 

    IEnumerator WaitPlayer()
    {
        while (!B_GameStart)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (Array_AllPlayers[0])
                {
                    GameObject o_otherPlayer = GameObject.FindGameObjectWithTag("PC_Player");

                    if (o_otherPlayer)
                    {
                        Array_AllPlayers[1] = o_otherPlayer;
                        Array_txtWinner[1] = Array_AllPlayers[1].GetComponent<PC_Player_Move>().Txt_winnerText_PC;
                        B_GameStart= true;
                        // sound �� ���ӽ��� �˸�
                    }
                }
                else if (Array_AllPlayers[1])
                {
                    GameObject o_otherPlayer = GameObject.FindGameObjectWithTag("VR_Player");

                    if (o_otherPlayer)
                    {
                        Array_AllPlayers[0] = o_otherPlayer;
                        Array_txtWinner[0] = Array_AllPlayers[0].GetComponent<VRPlayerMove1>().Txt_winnerText_VR;
                        B_GameStart = true;

                        // sound �� ���ӽ��� �˸�
                    }
                }

            }
            else
            {
                if (Array_AllPlayers[0])
                {
                    GameObject o_otherPlayer = GameObject.FindGameObjectWithTag("PC_Player");

                    if (o_otherPlayer)
                    {
                        Array_AllPlayers[1] = o_otherPlayer;
                        B_GameStart = true;
                        // sound �� ���ӽ��� �˸�
                    }
                }
                else if (Array_AllPlayers[1])
                {
                    GameObject o_otherPlayer = GameObject.FindGameObjectWithTag("VR_Player");

                    if (o_otherPlayer)
                    {
                        Array_AllPlayers[0] = o_otherPlayer;
                        B_GameStart = true;

                        // sound �� ���ӽ��� �˸�
                    }
                }
            }
            yield return null;
        }
    }

    public static bool isPresent()
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

    void UpdatePhotonNetwork()
    {
        if (B_GameStart)
        {
            CheckWinner();
        }
    }

    void CheckWinner()
    {
        if (!B_IsGameOver)
        {
            B_IsGameOver= true;

            if (PhotonNetwork.IsMasterClient)
            {
                // VR�� �̰��� ���
                if (Array_AllPlayers[0].GetComponent<VRPlayerMove1>().HP <= 0)
                {
                    // canvas �߰�, ��Ȱ����
                    Array_txtWinner[0].text = "VR Player Win!!";
                    Array_txtWinner[1].text = "PC Player Lose..";
                    StartCoroutine(LeaveEnd(3f));
                }
                // PC�� �̰��� ���
                else if (Array_AllPlayers[1].GetComponent<PC_Player_Move>().HP <= 0)
                {
                    // canvas �߰�
                    Array_txtWinner[0].text = "VR Player Lose..";
                    Array_txtWinner[1].text = "PC Player Win!!";
                    StartCoroutine(LeaveEnd(3f));
                }
            }
            else
            {
                // VR�� �̰��� ���
                if (Array_AllPlayers[0].GetComponent<VRPlayerMove1>().HP <= 0)
                {
                    // canvas �߰�
                    Array_txtWinner[0].text = "VR Player Win!!";
                    Array_txtWinner[1].text = "PC Player Lose..";
                    StartCoroutine(LeaveEnd(3f));
                }
                // PC�� �̰��� ���
                else if (Array_AllPlayers[1].GetComponent<PC_Player_Move>().HP <= 0)
                {
                    // canvas �߰�
                    Array_txtWinner[0].text = "VR Player Lose..";
                    Array_txtWinner[1].text = "PC Player Win!!";
                    StartCoroutine(LeaveEnd(3f));
                }
            }
        }
    }

    private void Awake()
    {
        if (instance == null) instance = this;

        Debug.Log("VR Device = " + isPresent().ToString());
        IsVR = isPresent();

        ResetDeathCount();
    }

    void Start()
    {
        // ����ȭ�鿡 ���� �ػ� 960x640
        Screen.SetResolution(960, 640, FullScreenMode.Windowed);

        // ������ �ۼ��� �� �ʴ� 30���� ����
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;

        StartCoroutine(WaitPlayer());
    }

    void Update()
    {
        UpdatePhotonNetwork();
        RotateSkyBox(RotationPerSecond);
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

    private void ResetDeathCount()
    {
        i_PCDeathCount = 0;
        i_VRDeathCount = 0;
    }

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
        if (stream.IsWriting)
        {

        }
        if (stream.IsReading)
        {

        }
    }

    
}
