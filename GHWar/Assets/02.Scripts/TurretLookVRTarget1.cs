using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretLookVRTarget1 : MonoBehaviour
{
    Transform tr_this;
    Transform o_target;

    // Start is called before the first frame update
    void Start()
    {
        tr_this = GetComponent<Transform>();
        o_target = GameObject.FindGameObjectWithTag("VRPlayerHead").transform;
    }

    // Update is called once per frame
    void Update()
    {
        LookVRPlayer();
    }


    [PunRPC]
    void LookVRPlayer()
    {
        Vector3 v3_direction = (o_target.transform.position - tr_this.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(v3_direction.x, 0*v3_direction.y, v3_direction.z));
    }
}
