using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;
using UnityEngine.UI;
using Oculus.Interaction;

// �κ� ���� �Լ�
// �κ� ������Ʈ�� �ִ� ��ư�� �����Ͽ� ���� ����
// (UI) �̺�Ʈ�ý��ۿ� ���ԵǾ�������, ��� ����־ �������.(Lobby, ConnManager ����)
public class UIButtonManagement : MonoBehaviourPun
{
    // ������ ����
    [SerializeField] GameObject connManager;
    // ���ӽ��۽� ������ �κ�
    [SerializeField] GameObject deactiveObj;

    // ȭ����ȯ �� ���̵���/�ƿ��� �̹��� �迭
    [SerializeField] Image[] img_fade = new Image[6];
    // ���̵� ��/�ƿ� �ð�
    [SerializeField] float f_fadeDuration = 2.0f;

    private void Start()
    {
        // ���� �� ���̵� �ƿ����� ����
        foreach(var img in img_fade)
        {
            img.DOFade(0, f_fadeDuration);
        }
    }

    // �κ� �ִ� ��ư ���� �Լ�
    public void OnClickButton_GameStart()
    {
        foreach (var img in img_fade)
        {
            img.DOFade(1, f_fadeDuration).OnStart(() =>
            {
                // ���̵� ���� �� �۵�
            }).OnComplete(() =>
            {
                // ���̵� ���� �� �۵�
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
