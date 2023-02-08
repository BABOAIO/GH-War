using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// ��ġ ����� �̹����� �ִ´�.
// �̹����� 1�� ���� ���̵� �ƿ�
public class ActiveImageFade : MonoBehaviour
{
    Image img_fade;

    private void Awake()
    {
        img_fade = GetComponent<Image>();
    }

    private void OnEnable()
    {
        img_fade.DOFade(0, 1.0f);

        img_fade.rectTransform.DOLocalMoveY(-0.8f, 1.0f).SetEase(Ease.OutQuad);
    }
}
