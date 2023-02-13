using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

// �̺�Ʈ �ý��ۿ� �־�, VR�� PC�� ��ư�� ���� �� �ְ� �Ѵ�.
// ������Ʈ�� �� �� ������ VR �ʿ��� ��ư�� ���� �� ����.
public class UIEnabled : MonoBehaviour
{
    InputSystemUIInputModule pcUI;
    XRUIInputModule vrUI;
    AudioSource as_eventSystem;

    private void Awake()
    {
        pcUI = GetComponent<InputSystemUIInputModule>();
        vrUI = GetComponent<XRUIInputModule>();
        as_eventSystem = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (GameManager.instance.IsVR)
        {
            pcUI.enabled = false;
        }
        else if(GameManager.instance.IsVR)
        {
            vrUI.enabled = false;
        }
    }

    private void Update()
    {
        if(!as_eventSystem.isPlaying && GameManager.instance.B_GameStart)
        {
            as_eventSystem.Play();
        }
        else if(GameManager.instance.B_IsGameOver|| !GameManager.instance.B_GameStart)
        {
            as_eventSystem.Stop();
        }
    }
}
