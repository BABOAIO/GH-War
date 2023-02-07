using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 시작 전 상화작용을 없애는 용도, 보통 나무 등 환경에 넣는다.
public class SetActiveKinetic : MonoBehaviour
{
    public Vector3 V3_origonPos;
    public Quaternion Q_origonRot;

    private void Awake()
    {
        V3_origonPos= transform.position;
        Q_origonRot= transform.rotation;
    }

    private void Update()
    {
        if(GameManager.instance.B_GameStart)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        if (!GameManager.instance.B_GameStart)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
