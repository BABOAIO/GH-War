using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Photon.Pun;

enum TARGET
{
    MASTER = 0,
    CLIENT = 1,
}
public partial class GameManager2 : MonoBehaviourPunCallbacks
{

    public GameObject[] communicators = new GameObject[2];
    // ������ �÷��̾���� ��ü�� �־���
    public bool bGameEnd = false;          // ������ ��������  
    public bool bNetWarStart = false;     // ��Ʈ��ũ ������ ���۵ƴ��� 

    void PhotonGameSetting()
    {   // ���� ���� �� ����

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master Login2:");
            uiMe[0].SetActive(true);
            uiMe[1].SetActive(false);
            communicators[0] = PhotonNetwork.Instantiate(photonPrefab.name, new Vector3(1f, 10f, 0f), Quaternion.identity, 0);
            communicators[0].GetComponent<PlayerPhoton>().iHp = iMyHpBase;
            communicators[0].transform.tag = "MASTER";
        }
        else
        {
            Debug.Log("Client Login2:");
            uiMe[0].SetActive(false);
            uiMe[1].SetActive(true);
            communicators[1] = PhotonNetwork.Instantiate(photonPrefab.name, new Vector3(5.1f, 10f, 0f), Quaternion.identity, 0);
            communicators[1].GetComponent<PlayerPhoton>().iHp = iMyHpBase;
            communicators[1].transform.tag = "CLIENT";
        }

        ResetCharAni();                  // ĳ���� �ִϸ��̼� IDLE�� �ʱ�ȭ
        ResetDisplayHp();                // HP �ʱⰪ �־��ֱ�

        StartCoroutine(WaitPlayer());   // ���� ��� ��ٸ��� 
    }

    IEnumerator WaitPlayer()
    {    // ���� ��� ��ٸ��� 

        while (!bNetWarStart)
        {

            GameObject tmpComm = GameObject.FindGameObjectWithTag("COMM");

            if (PhotonNetwork.IsMasterClient)
            {

                if (tmpComm && tmpComm.GetComponent<PhotonView>().viewID == 2001)
                {    // ���� ������ �� ������ ����
                    bNetWarStart = true;

                    communicators[1] = tmpComm;

                    DeleteAllBlock(); // ���� ��� ����
                    CreateBlock();     // ��� �ٽ� ���� 

                    Debug.Log("client come in");
                }

            }
            else
            {

                if (tmpComm && tmpComm.GetComponent<PhotonView>().viewID == 1001)
                {   // ���� ����� �� ���� ������ ���� 
                    bNetWarStart = true;

                    communicators[0] = tmpComm;

                    DeleteAllBlock(); // ���� ��� ����
                    CreateBlock();     // ��� �ٽ� ���� 

                    Debug.Log("master come in");
                }

            }
            yield return null;
        }
    }

    void UpdatePhoton()
    {      // GameManager �� Update() �Լ����� ���ư� 

        if (bNetWarStart)
        {       // ��Ʈ��ũ ������ �� 

            CheckWhoWin();
            DisplayHp();       // HP �����ֱ�

        }

    }

    void CheckWhoWin()
    {
        if (!bGameEnd && communicators[0].GetComponent<PlayerPhoton>().iHp <= 0 || communicators[1].GetComponent<PlayerPhoton>().iHp <= 0)
        {
            bGameEnd = true;

            if (PhotonNetwork.isMasterClient)
            {    // Master Win
                if (communicators[1].GetComponent<PlayerPhoton>().iHp <= 0)
                {  // Master Win
                    panelWin.SetActive(true);
                    StartCoroutine(UpDownPanel(panelWin));
                    masterChar.playerChar.GetComponent<PlayerFSM>().SetState(CHARCTERSTATE.WinPose1);
                    clientChar.playerChar.GetComponent<PlayerFSM>().SetState(CHARCTERSTATE.Dead);
                    StartCoroutine(LeaveEnd(3f));
                }
                if (communicators[0].GetComponent<PlayerPhoton>().iHp <= 0)
                {  // Master Lose
                    panelLose.SetActive(true);
                    StartCoroutine(UpDownPanel(panelLose));
                    masterChar.playerChar.GetComponent<PlayerFSM>().SetState(CHARCTERSTATE.Dead);
                    clientChar.playerChar.GetComponent<PlayerFSM>().SetState(CHARCTERSTATE.WinPose1);
                    StartCoroutine(LeaveEnd(3f));
                }
            }
            else
            {
                if (communicators[1].GetComponent<PlayerPhoton>().iHp <= 0)
                {  // Client Lose
                    panelLose.SetActive(true);
                    StartCoroutine(UpDownPanel(panelLose));
                    masterChar.playerChar.GetComponent<PlayerFSM>().SetState(CHARCTERSTATE.WinPose1);
                    clientChar.playerChar.GetComponent<PlayerFSM>().SetState(CHARCTERSTATE.Dead);
                    StartCoroutine(LeaveEnd(3f));
                }
                if (communicators[0].GetComponent<PlayerPhoton>().iHp <= 0)
                {  // Client Win
                    panelWin.SetActive(true);
                    StartCoroutine(UpDownPanel(panelWin));
                    clientChar.playerChar.GetComponent<PlayerFSM>().SetState(CHARCTERSTATE.WinPose1);
                    masterChar.playerChar.GetComponent<PlayerFSM>().SetState(CHARCTERSTATE.Dead);
                    StartCoroutine(LeaveEnd(3f));
                }
            }

        }
    }

    void ResetCharAni()
    {     // ĳ���� �ִ� ����
        masterChar.playerChar.GetComponent<PlayerFSM>().SetState(CHARCTERSTATE.Idle);
        clientChar.playerChar.GetComponent<PlayerFSM>().SetState(CHARCTERSTATE.Idle);
    }
    void DisplayHp()
    {   // ������ ������
        TxtHpMaster.text = communicators[0].GetComponent<PlayerPhoton>().iHp.ToString();
        TxtHpClient.text = communicators[1].GetComponent<PlayerPhoton>().iHp.ToString();

        hpGaugeMaster.fillAmount = (float)communicators[0].GetComponent<PlayerPhoton>().iHp / iMyHpBase;
        hpGaugeClient.fillAmount = (float)communicators[1].GetComponent<PlayerPhoton>().iHp / iMyHpBase;
    }
    void ResetDisplayHp()
    {          // HP ǥ�� ����
        TxtHpMaster.text = iMyHpBase.ToString();
        TxtHpClient.text = iEnemyHpBase.ToString();

        hpGaugeMaster.fillAmount = 1f;
        hpGaugeClient.fillAmount = 1f;
    }

    public void NetAttackDamage(int damage)
    {

        int attackTarget = 0;

        if (PhotonNetwork.isMasterClient)
        {
            attackTarget = (int)TARGET.CLIENT;

        }
        else
        {
            attackTarget = (int)TARGET.MASTER;
        }

        communicators[0].GetComponent<PhotonView>().RPC("AttackInfo", PhotonTargets.Others, attackTarget, damage);

    }
    public void AttackProcess(int attackTarget, int damage)
    {
        communicators[attackTarget].GetComponent<PlayerPhoton>().iHp -= damage;

        if (communicators[attackTarget].GetComponent<PlayerPhoton>().iHp <= 0)
        {
            communicators[attackTarget].GetComponent<PlayerPhoton>().iHp = 0;
        }

        if (attackTarget == (int)TARGET.MASTER)
        {
            hpGaugeMaster.fillAmount = (float)communicators[attackTarget].GetComponent<PlayerPhoton>().iHp / iMyHpBase;

            clientChar.playerChar.GetComponent<PlayerFSM>().SetState(CHARCTERSTATE.Attack);   // ĳ���� ���� �ִ�
            StartCoroutine(RotateChar(clientChar.playerChar, false));
            StartCoroutine(HitChar(masterChar.playerChar, true, false, damage));
        }
        else
        {
            hpGaugeClient.fillAmount = (float)communicators[attackTarget].GetComponent<PlayerPhoton>().iHp / iMyHpBase;

            masterChar.playerChar.GetComponent<PlayerFSM>().SetState(CHARCTERSTATE.Attack);   // ĳ���� ���� �ִ�
            StartCoroutine(RotateChar(masterChar.playerChar, true));
            StartCoroutine(HitChar(clientChar.playerChar, false, false, damage));
        }
    }

    public void OnPhotonPlayerConnected(PhotonPlayer other)         // �ٸ� ������ �������� �� 
    {
        Debug.Log("other player connect");
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer other)      // ����� ������ ������ �� 
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("other player disconnected");

        }
        else
        {
            Debug.Log("master disconnected");

        }

        StartCoroutine(LeaveEnd(4f));
    }

    public IEnumerator LeaveEnd(float ftime)
    {
        yield return new WaitForSeconds(ftime);
        LeaveRoom();
    }

    public void LeaveRoom()
    {
        communicators[0] = null;
        communicators[1] = null;

        bGameEnd = false;
        bNetWarStart = false;

        panelWin.SetActive(false);
        panelLose.SetActive(false);

        ResetCharAni();                  // ĳ���� �ִϸ��̼� IDLE�� �ʱ�ȭ
        ResetDisplayHp();                // HP �ʱⰪ �־��ֱ�

        uiMe[0].SetActive(true);
        uiMe[1].SetActive(false);

        PhotonNetwork.LeaveRoom();
    }

}