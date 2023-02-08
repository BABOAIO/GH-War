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

    private void Start()
    {
        img_fade = GetComponent<Image>();
        img_fade.DOFade(0.1f, 2.0f).OnStart(() => { }).OnComplete(() => { img_fade.DOFade(1, 1.5f); }).SetLoops(-1, LoopType.Restart);
    }
}
