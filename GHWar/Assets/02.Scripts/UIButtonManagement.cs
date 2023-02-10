using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;
using UnityEngine.UI;
using Oculus.Interaction;
using UniRx;
using System.Threading;
using System;

// �κ� ���� �Լ�
// �κ� ������Ʈ�� �ִ� ��ư�� �����Ͽ� ���� ����
// (UI) �̺�Ʈ�ý��ۿ� ���ԵǾ�������, ��� ����־ �������.(Lobby, ConnManager ����)
public class UIButtonManagement : MonoBehaviourPun
{
    // ������ ����
    [SerializeField] GameObject connManager;
    // ���ӽ��۽� ������ �κ�
    [SerializeField] GameObject deactiveObj;
    [SerializeField] GameObject deactiveObj_LineL;
    [SerializeField] GameObject deactiveObj_LineR;

    // ȭ����ȯ �� ���̵���/�ƿ��� �̹��� �迭
    [SerializeField] Image[] img_fade = new Image[6];
    // ���̵� ��/�ƿ� �ð�
    [SerializeField] float f_fadeDuration = 2.0f;

    private void Start()
    {
        // ���� �� ���̵� �ƿ����� ����
        for(int i = 0;i< img_fade.Length; i++)
        {
            img_fade[i].DOFade(0, f_fadeDuration);
        }
    }

    // �κ� �ִ� ��ư ���� �Լ�
    public void OnClickButton_GameStart()
    {
        for (int i = 0; i < img_fade.Length; i++)
        {
            img_fade[i].DOFade(1, f_fadeDuration).OnStart(() =>
            {
                // ���̵� ���� �� �۵�
                deactiveObj_LineL.SetActive(false);
                deactiveObj_LineR.SetActive(false);
            }).OnComplete(() =>
            {
                // ���̵� ���� �� �۵�
                // 0.8�� ���� ��onnManager ����
                Observable.Timer(TimeSpan.FromMilliseconds(100)).Subscribe(_ => connManager.SetActive(true));                
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
