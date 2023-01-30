using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class OnClickButtonManager : MonoBehaviourPunCallbacks
{
    int playerCount = 0;
    int maxPlayerCount = 0;

    HPManager hpManager;

    [SerializeField] GameObject canvasVR;
    [SerializeField] GameObject canvasPC;

    private void Start()
    {
        canvasPC.SetActive(false); canvasVR.SetActive(false);

        maxPlayerCount = GetComponent<ConnManager>().Byte_maxPlayer;
        Debug.Log($"�ִ� �÷��̾� : {maxPlayerCount}");

        if (GameManager.instance.IsVR)
        {
            print("��");
            canvasVR.SetActive(true); canvasPC.SetActive(false);
        }
        else
        {
            canvasVR.SetActive(false); canvasPC.SetActive(true);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("������ Ŭ���̾�Ʈ Ȱ��ȭ");
            hpManager = GetComponent<HPManager>();

            hpManager.enabled = false;
        }
    }

    private void Update()
    {
        
    }

    [PunRPC]
    public void OnClickReadyButton_PC()
    {
        photonView.RPC("CheckReadyCount", RpcTarget.MasterClient);
        canvasPC.SetActive(false);
    }

    [PunRPC]
    public void OnClickReadyButton_VR()
    {
        photonView.RPC("CheckReadyCount", RpcTarget.MasterClient);
        canvasVR.SetActive(false);
    }

    [PunRPC]
    public void CheckReadyCount()
    {
        if(!photonView.Controller.IsMasterClient) { return; }

        playerCount++;
        if(playerCount == maxPlayerCount)
        {
            canvasVR.SetActive(false); canvasPC.SetActive(false);
            hpManager.enabled = true;
        }
    }
}
