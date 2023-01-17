using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    // VR인지 PC인지를 구분
    public bool isVR;

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

    private void Awake()
    {
        if (instance == null) instance = this;

        Debug.Log("VR Device = " + isPresent().ToString());
        isVR = isPresent();
    }

    void Start()
    {
        // 실행화면에 대한 해상도 960x640
        Screen.SetResolution(960, 640, FullScreenMode.Windowed);

        // 데이터 송수신 빈도 초당 30으로 설정
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;
    }

    void Update()
    {
        
    }
}
