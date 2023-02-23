using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IsVROrPC : MonoBehaviour
{
    [SerializeField] GameObject[] deactiveObj =new GameObject[2];
    [SerializeField] GameObject[] deactiveCanvas = new GameObject[2];

    private void Start()
    {
        if (GameManager.instance.IsVR)
        {
            deactiveObj[0].SetActive(true);
            deactiveCanvas[0].SetActive(true);
            deactiveObj[1].SetActive(false);
            deactiveCanvas[1].SetActive(false);
        }
        else if(!GameManager.instance.IsVR)
        {
            deactiveObj[1].SetActive(true);
            deactiveCanvas[1].SetActive(true);
            deactiveObj[0].SetActive(false);
            deactiveCanvas[0].SetActive(false);
        }
    }

}
