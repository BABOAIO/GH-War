using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// VR 플레이어의 손(콜라이더가 있는)에 넣어준다.
// 손 애니메이션 관련 스크립트
public class HandPresence : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;    
    private InputDevice targetDevice;
    public Animator handAnimator;

    public float triggerValue;
    public float gripValue;

    void Start()
    {
        TryInitialize();
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    void UpdateHandAnimation()
    {
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            UpdateHandAnimation();
        }
    }
}
