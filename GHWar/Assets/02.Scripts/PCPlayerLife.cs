using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPlayerLife : MonoBehaviour
{
    [SerializeField] GameObject[] lifeImage = new GameObject[2];

    void Update()
    {
        switch(GameManager.instance.i_PCDeathCount)
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
}
