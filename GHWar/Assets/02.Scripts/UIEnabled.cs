using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

// 이벤트 시스템에 넣어, VR과 PC가 버튼을 누를 수 있게 한다.
// 컴포넌트가 둘 다 있으면 VR 쪽에서 버튼을 누를 수 없다.
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
