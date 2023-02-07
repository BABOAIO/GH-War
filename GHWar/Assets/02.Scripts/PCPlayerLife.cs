using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

// PC플레이어 최상단에 넣는다.
public class PCPlayerLife : MonoBehaviour
{
    [Header("PC 플레이어 생명 이미지")]
    [SerializeField] GameObject[] lifeImage = new GameObject[2];
    [Header("PC 플레이어 잃은 생명 이미지")]
    [SerializeField] GameObject[] lifeLossImage = new GameObject[2];

    private void Start()
    {

    }

    void Update()
    {
        switch (GameManager.instance.i_PCDeathCount)
        {
            case 0:
                lifeImage[0].SetActive(false);
                lifeImage[1].SetActive(false);
                lifeLossImage[0].SetActive(true);
                lifeLossImage[0].GetComponent<ActiveImageFade>().enabled = true;
                lifeLossImage[1].SetActive(true);
                lifeLossImage[1].GetComponent<ActiveImageFade>().enabled = true;
                break;
            case 1:
                lifeImage[0].SetActive(true);
                lifeImage[1].SetActive(false);
                lifeLossImage[0].SetActive(false);
                lifeLossImage[0].GetComponent<ActiveImageFade>().enabled = false;
                lifeLossImage[1].SetActive(true);
                lifeLossImage[1].GetComponent<ActiveImageFade>().enabled = true;
                break;
            case 2:
                lifeImage[0].SetActive(true);
                lifeImage[1].SetActive(true);
                lifeLossImage[0].SetActive(false);
                lifeLossImage[0].GetComponent<ActiveImageFade>().enabled = false;
                lifeLossImage[1].SetActive(false);
                lifeLossImage[1].GetComponent<ActiveImageFade>().enabled = false;
                break;
        }
    }

    //public void HeartBreak()
    //{
    //    switch (GameManager.instance.i_PCDeathCount)
    //    {
    //        case 0:
    //            break;
    //        case 1:
    //            lifeLossImage[0].DOFade(0, 0.5f);
    //            break;
    //        case 2:
    //            lifeLossImage[1].DOFade(0, 0.5f);
    //            break;
    //    }
    //}
}
