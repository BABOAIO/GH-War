using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    GameObject[] arryObject_PCPlayer;
    GameObject[] arryObject_VRPlayer;

    private void OnEnable()
    {
        Debug.Log("싸우는 게임모드 시작.");

        arryObject_PCPlayer = GameObject.FindGameObjectsWithTag("PC_Player");
        arryObject_VRPlayer = GameObject.FindGameObjectsWithTag("VR_Player");
    }
}
