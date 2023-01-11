using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HandPresencePhysic : MonoBehaviour
{
    [Header("���� �̵��� ��Ʈ�ѷ� ��ġ")]
    [SerializeField] Transform t_target;
    [Header("������ ���� �ʰ� �� ���� ���� ���͸���")]
    [SerializeField] Renderer rd_nonPhysicsHand;
    [SerializeField] float f_showDistance = 0.05f;

    // �տ� �ִ� ��� �ݶ��̴�
    Collider[] col_hands;
    // �����ӿ� ������ �ֱ� ���� ������ٵ� ��Ʈ��
    Rigidbody rb_this;

    void Start()
    {
        rb_this = GetComponent<Rigidbody>();
        col_hands = GetComponentsInChildren<Collider>();
    }

    private void Update()
    {
        float f_distance = Vector3.Distance(transform.position, t_target.position);

        if (f_distance > f_showDistance)
        {
            rd_nonPhysicsHand.enabled = true;
        }
        else
        {
            rd_nonPhysicsHand.enabled = false;
        }
    }

    // �Ʒ� Invoke �Լ� ����
    public void OnSelect_EnableHandCollider()
    {
        foreach(var item in col_hands)
        {
            item.enabled = true;
        }
    }

    // XR �̺�Ʈ �Լ�, ����(Ʈ���� ��ư) �����ϸ� �ݶ��̴� �߻�, �̰��� 0.5�� ���� �߻�
    public void OnSelect_EnableHandCollider_Delay(float delay)
    {
        Invoke("OnSelect_EnableHandCollider", delay);
    }

    // XR �̺�Ʈ �Լ�, ����(Ʈ���� ��ư)�ϸ� �ݶ��̴� ������
    public void OnSelect_DisableHandCollider()
    {
        foreach (var item in col_hands)
        {
            item.enabled = false;
        }
    }
    public void OnSelect_DisableHandCollider_Delay(float delay)
    {
        Invoke("OnSelect_DisableHandCollider", delay);
    }

    void FixedUpdate()
    {
        rb_this.velocity = (t_target.position - transform.position) / Time.fixedDeltaTime;

        Quaternion q_rotationDifference = t_target.rotation*Quaternion.Inverse(transform.rotation);
        q_rotationDifference.ToAngleAxis(out float f_angleInDegree, out Vector3 v3_rotationAxis);

        // �� �ڵ尡 ������ Ư�� �������� ���� 180���� �Ѿ�� �ѹ��� ���ư�
        f_angleInDegree = Mathf.Clamp(f_angleInDegree, -90f, 90f);

        Vector3 v3_rotationDifferenceInDegree = f_angleInDegree * v3_rotationAxis;

        rb_this.angularVelocity = (v3_rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);
    }
}
