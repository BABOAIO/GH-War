using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PCPlayerLife : MonoBehaviour
{
    [SerializeField] GameObject[] lifeImage = new GameObject[2];
    [SerializeField] Image[] lifeLossImage = new Image[2];

    private void Start()
    {
        lifeLossImage[0].DOFade(1, 0.1f);
        lifeLossImage[1].DOFade(1, 0.1f);
    }

    void Update()
    {
        switch (GameManager.instance.i_PCDeathCount)
        {
            case 0:
                lifeImage[0].SetActive(false);
                lifeImage[1].SetActive(false);
                break;
            case 1:
                lifeImage[0].SetActive(true);
                lifeImage[1].SetActive(false);
                break;
            case 2:
                lifeImage[0].SetActive(true);
                lifeImage[1].SetActive(true);
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
