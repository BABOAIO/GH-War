using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForwardPCPlayerHead : MonoBehaviour
{
    [SerializeField] Transform lookForward;
    Transform tr_this;

    void Start()
    {
        tr_this = GetComponent<Transform>();   
    }

    void Update()
    {
        tr_this.rotation = Quaternion.Euler(Vector3.left);
    }
}
