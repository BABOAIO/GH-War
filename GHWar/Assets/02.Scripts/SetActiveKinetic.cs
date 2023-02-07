using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� �� ��ȭ�ۿ��� ���ִ� �뵵, ���� ���� �� ȯ�濡 �ִ´�.
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
