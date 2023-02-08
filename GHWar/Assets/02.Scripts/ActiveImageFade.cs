using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 점치 사라질 이미지에 넣는다.
// 이미지가 1초 정도 페이드 아웃
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
