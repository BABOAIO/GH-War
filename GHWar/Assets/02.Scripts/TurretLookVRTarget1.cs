using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// 대포 모델 최상단에 넣는다.
// 이 부분이 오류가 날 경우, 게임매니저를 살펴본다.
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
        if(o_target!= null)
        {
            Vector3 v3_direction = (o_target.transform.position - tr_this.position).normalized;
            //transform.rotation = Quaternion.LookRotation(new Vector3(v3_direction.x, v3_direction.y, v3_direction.z));
            transform.LookAt(new Vector3(o_target.transform.position.x, tr_this.transform.position.y, o_target.transform.position.z), Vector3.up);
        }
        else
        {
            o_target = GameObject.FindGameObjectWithTag("VRPlayerHead").transform;
        }
    }
}
