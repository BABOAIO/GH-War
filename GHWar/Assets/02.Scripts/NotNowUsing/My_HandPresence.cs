using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class My_HandPresence : MonoBehaviour
{
    [SerializeField] InputDeviceCharacteristics idc_base;
    [SerializeField] List<GameObject> l_controllerPrefabs;
    InputDevice id_target;
    GameObject o_spawnedController;

    void Start()
    {
        List<InputDevice> l_devices = new List<InputDevice>();
        InputDeviceCharacteristics idc_right = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(idc_right, l_devices);

        foreach(var item in l_devices)
        {

        }

        if(l_devices.Count > 0)
        {
            id_target = l_devices[0];
            GameObject prefab = l_controllerPrefabs.Find(controller=> controller.name == id_target.name);
            if(prefab != null)
            {
                o_spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.LogError("핸드 모델을 찾을 수 없습니다...");
                o_spawnedController = Instantiate(l_controllerPrefabs[0], transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
