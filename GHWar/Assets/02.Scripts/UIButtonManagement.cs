using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UIButtonManagement : MonoBehaviourPun
{
    [SerializeField] GameObject connManager;
    [SerializeField] GameObject deactiveObj;

    public void OnClickButton_GameStart()
    {
        connManager.SetActive(true);
        Invoke("DelayedGenerate", 0.5f);
    }

    void DelayedGenerate()
    {
        deactiveObj.SetActive(false);
    }

    public void OnClickButton_QuitGame()
    {
        
    }
}
