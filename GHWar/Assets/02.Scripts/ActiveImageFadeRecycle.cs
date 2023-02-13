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
