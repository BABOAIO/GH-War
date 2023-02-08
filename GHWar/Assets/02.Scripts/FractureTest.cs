using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class FractureTest : MonoBehaviourPunCallbacks
{
    public Collider[] colliders;    // �浹ü�� �迭�� ������

    // Start is called before the first frame update

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider>(); // �ش� ���ӿ�����Ʈ�� �ڽ� ������Ʈ�� ������Ʈ�� �ҷ���
        foreach (Collider c in colliders)
        {
            if (c.name.Contains("RandShatter")) continue;  // �θ� ����ó��
            c.gameObject.GetComponent<Renderer>().enabled = false;  // ������ false
            Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();  // �� �ڽĿ�����Ʈ�� ������ٵ� ������Ʈ ���� �Ҵ�
            rb.constraints = (RigidbodyConstraints)126; // ����Ʈ���� ���� üũ
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shatter")
        {
            GetComponent<Renderer>().enabled = false;
            foreach (Collider c in colliders)
            {
                if (c.name == "RandShatterTest") continue;
                c.gameObject.GetComponent<Renderer>().enabled = true;
                Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();
                rb.constraints = (RigidbodyConstraints)0;   // ����Ʈ���� ���� üũ����
            }
        }
    }

    public void DestructionThisArea()
    {
        GetComponent<Renderer>().enabled = false;
        foreach (Collider c in colliders)
        {
            if (c.name.Contains("RandShatter")) continue;  // �θ� ����ó��
            c.gameObject.GetComponent<Renderer>().enabled = true;
            Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();
            rb.constraints = (RigidbodyConstraints)0;   // ����Ʈ���� ���� üũ����
            rb.AddForce(new Vector3(Random.Range(-10f, 10f), Random.Range(-1f, -0.1f), Random.Range(-10f, 10f)),ForceMode.Impulse); // �μ����� �پ��� ������� ����������
        }
    }
}