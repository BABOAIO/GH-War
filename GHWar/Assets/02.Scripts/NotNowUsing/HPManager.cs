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
        Debug.Log("�ο�� ���Ӹ�� ����.");

        arryObject_PCPlayer = GameObject.FindGameObjectsWithTag("PC_Player");
        arryObject_VRPlayer = GameObject.FindGameObjectsWithTag("VR_Player");
    }
}
