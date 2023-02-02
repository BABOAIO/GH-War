using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class UIEnabled : MonoBehaviour
{
    InputSystemUIInputModule pcUI;
    XRUIInputModule vrUI;

    private void Awake()
    {
        pcUI = GetComponent<InputSystemUIInputModule>();
        vrUI = GetComponent<XRUIInputModule>();
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
}
