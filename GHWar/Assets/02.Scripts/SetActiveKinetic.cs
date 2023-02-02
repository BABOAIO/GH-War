using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveKinetic : MonoBehaviour
{
    private void Update()
    {
        if (!GameManager.instance.B_GameStart)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
