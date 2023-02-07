using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;
using UnityEngine.UI;
using Oculus.Interaction;

// 로비 관련 함수
// 로비 오브젝트에 있는 버튼에 연결하여 서버 접속
// (UI) 이벤트시스템에 포함되어있지만, 사실 어디있어도 상관없다.(Lobby, ConnManager 제외)
public class UIButtonManagement : MonoBehaviourPun
{
    // 꺼놓고 시작
    [SerializeField] GameObject connManager;
    // 게임시작시 꺼지는 로비
    [SerializeField] GameObject deactiveObj;

    // 화면전환 시 페이드인/아웃될 이미지 배열
    [SerializeField] Image[] img_fade = new Image[6];
    // 페이드 인/아웃 시간
    [SerializeField] float f_fadeDuration = 2.0f;

    private void Start()
    {
        // 시작 시 페이드 아웃으로 시작
        foreach(var img in img_fade)
        {
            img.DOFade(0, f_fadeDuration);
        }
    }

    // 로비에 있는 버튼 연결 함수
    public void OnClickButton_GameStart()
    {
        foreach (var img in img_fade)
        {
            img.DOFade(1, f_fadeDuration).OnStart(() =>
            {
                // 페이드 시작 시 작동
            }).OnComplete(() =>
            {
                // 페이드 끝날 시 작동
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
