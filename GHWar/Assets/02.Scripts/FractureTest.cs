using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

// �μ����� ������ �ִ´�
// ������ٵ� �ܽ�Ʈ���ν�Ʈ ���� üũ + �޽��ö��̴� �ܺ���+����Ʈ���� üũ �ʼ�
// ���� Ʈ���� ������ �÷��̾ ���� ���� �� ����.
public class FractureTest : MonoBehaviourPunCallbacks
{
    public Collider[] colliders;    // �浹ü�� �迭�� ������

    //[SerializeField] float f_quakeStrength = 1.0f;

    // player�� �ν��ϴ� �������� �ð�, �ʱ�ȭ�� 100���� �����Ѵ�.
    public int i_destroyTime = 100;

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

    private void Update()
    {
        //if(Input.GetKeyUp(KeyCode.Alpha2)) { this.transform.DOShakePosition(0.5f, f_quakeStrength); }
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