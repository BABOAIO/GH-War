using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;
using UnityEngine.UI;
using Oculus.Interaction;

public class UIButtonManagement : MonoBehaviourPun
{
    [SerializeField] GameObject connManager;
    [SerializeField] GameObject deactiveObj;

    [SerializeField] Image[] img_fade = new Image[6];
    [SerializeField] float f_fadeDuration = 2.0f;

    private void Start()
    {
        foreach(var img in img_fade)
        {
            img.DOFade(0, f_fadeDuration);
        }
    }

    public void OnClickButton_GameStart()
    {
        foreach (var img in img_fade)
        {
            img.DOFade(1, f_fadeDuration).OnStart(() =>
            {

            }).OnComplete(() =>
            {
                connManager.SetActive(true);
            });
        }
        //connManager.SetActive(true);
        Invoke("DelayedGenerate", 1.0f);
    }

    void DelayedGenerate()
    {
        deactiveObj.SetActive(false);
    }

    public void OnClickButton_QuitGame()
    {
        
    }
}
