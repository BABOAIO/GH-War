using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;
using UnityEditor.ShortcutManagement;

// �μ����� ������ �ִ´�
// ������ٵ� �ܽ�Ʈ���ν�Ʈ ���� üũ + �޽��ö��̴� �ܺ���+����Ʈ���� üũ �ʼ�
// ���� Ʈ���� ������ �÷��̾ ���� ���� �� ����.
public class FractureTest : MonoBehaviourPunCallbacks
{
    public AudioSource as_parent;
    public AudioClip ac_destruction;
    public AudioClip ac_shake;
    public AudioClip ac_warning;

    public Collider[] colliders;    // �浹ü�� �迭�� ������

    // �÷��̾ �������� ������ ��ġ, PCPlayerHit ����
    public Transform tr_spawnPoint;

    [HideInInspector]
    // player�� �ν��ϴ� �������� �ð�, �ʱ�ȭ�� 100���� �����Ѵ�.
    public int i_destroyTime = 100;

    private void Awake()
    {
        // �ش� ���ӿ�����Ʈ�� �ڽ� ������Ʈ�� ������Ʈ�� �ҷ���
        colliders = GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
        {
            // �θ� �� ���� �ͷ����� ����ó��
            if (c.name.Contains("_Parent") || c.gameObject.layer == LayerMask.NameToLayer("Turret")) continue;
            c.gameObject.GetComponent<Renderer>().enabled = false;  // ������ false
            Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();  // �� �ڽĿ�����Ʈ�� ������ٵ� ������Ʈ ���� �Ҵ�
            rb.constraints = (RigidbodyConstraints)126; // ����Ʈ���� ���� üũ
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha2)) { }
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
            // �θ� �� ���� �ͷ����� ����ó��
            if (c.name.Contains("_Parent") || c.gameObject.layer == LayerMask.NameToLayer("Turret")) continue;
            c.gameObject.GetComponent<Renderer>().enabled = true;
            Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();
            rb.constraints = (RigidbodyConstraints)0;   // ����Ʈ���� ���� üũ����
            // ������ �ſ� ���� ������ �ּ� 100�������� ����� ��� ����ȴ�.
            float overPow = 1000000; 
            // �μ����� �پ��� ������� ����������
            rb.AddForce(new Vector3(Random.Range(-overPow, overPow), Random.Range(-overPow, -0.1f), Random.Range(-overPow, overPow)), ForceMode.Impulse); 
        }
    }

    [SerializeField] List<GameObject> list_O_Bridge = new List<GameObject>();

    // �ð��� ���� �� �������� �� �ٸ�
    IEnumerator DestructionAllBridge()
    {
        yield return new WaitForSeconds(10.0f);

        for (int i = 0; i<list_O_Bridge.Count; i++)
        {
            yield return new WaitForSeconds(1.0f);

        }

    }
}