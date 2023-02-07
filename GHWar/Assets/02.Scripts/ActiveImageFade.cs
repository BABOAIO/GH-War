using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

        img_fade.rectTransform.DOMoveY(-0.1f, 1.0f);
    }

    public void ActiveFade()
    {
        img_fade.DOFade(0, 1.0f);

        img_fade.rectTransform.DOMoveY(-2.5f, 1.0f);
    }
}
