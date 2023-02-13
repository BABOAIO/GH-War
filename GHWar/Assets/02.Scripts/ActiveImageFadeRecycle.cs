using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 점치 사라졌다가 다시 나오는 이미지에 넣는다.
// 이미지가 1초 정도 페이드 아웃, 1초 뒤 페이드 인
public class ActiveImageFadeRecycle : MonoBehaviour
{
    Image img_fade;
    AudioSource as_warning;

    private void Start()
    {
        as_warning = GetComponent<AudioSource>();
        img_fade = GetComponent<Image>();

        img_fade.DOFade(0.1f, 1.0f).OnStart(() => { as_warning.Play(); }).OnComplete(() => { img_fade.DOFade(1, 0.5f); }).SetLoops(-1, LoopType.Restart);
    }

    private void Update()
    {
        if (!as_warning.isPlaying)
        {
            as_warning.Play();
        }
    }
}
