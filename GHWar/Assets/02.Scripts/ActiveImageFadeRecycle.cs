using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// ��ġ ������ٰ� �ٽ� ������ �̹����� �ִ´�.
// �̹����� 1�� ���� ���̵� �ƿ�, 1�� �� ���̵� ��
public class ActiveImageFadeRecycle : MonoBehaviour
{
    Image img_fade;

    private void Start()
    {
        img_fade = GetComponent<Image>();
        img_fade.DOFade(0.1f, 2.0f).OnStart(() => { }).OnComplete(() => { img_fade.DOFade(1, 1.5f); }).SetLoops(-1, LoopType.Restart);
    }
}
