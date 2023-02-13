using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;
using DG.Tweening;
using UnityEngine.Experimental.AI;

// VR �÷��̾��� ��(�ݶ��̴��� �ִ�)�� �־��ش�.
// ���� �ٴڿ� ����� ���, ������� ���ϰ� ����
public class HandPresencePhysic : MonoBehaviour
{
    public AudioSource as_parent;

    [Header("���� �̵��� ��Ʈ�ѷ� ��ġ")]
    [SerializeField] Transform t_target;
    [Header("������ ���� �ʰ� �� ���� ���� ���͸���")]
    [SerializeField] Renderer rd_nonPhysicsHand;
    [Header("�� �� �ִ� �Ÿ�(?)")]
    [SerializeField] float f_showDistance = 0.05f;

    [SerializeField] Vector2 v2_rangeOfHandRotation;

    // �տ� �ִ� ��� �ݶ��̴�
    Collider[] col_hands;
    // �����ӿ� ������ �ֱ� ���� ������ٵ� ��Ʈ��
    Rigidbody rb_this;

    // ������ �ε���
    public bool IsHit;

    void Start()
    {
        rb_this = GetComponent<Rigidbody>();
        col_hands = GetComponentsInChildren<Collider>();

        IsHit = false;
        //foreach(var col in col_hands)
        //{
        //    col.AddComponent<VRPlayerHandHit>();
        //    col.GetComponent<VRPlayerHandHit>().ParentHand = this;
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CannonBall"))
        {
            as_parent.Stop();
            as_parent.Play();
            StartCoroutine(StopHandScript());
        }
    }

    IEnumerator StopHandScript()
    {
        IsHit = true;

        transform.DOShakeRotation(0.5f, 30.0f).OnStart(() =>
        {
            transform.DOShakePosition(0.5f, 0.05f);
        }
        );

        yield return new WaitForSeconds(3.0f);
        IsHit = false;
    }

    private void Update()
    {
        if (IsHit) return;
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

    // XR �̺�Ʈ �Լ�, ����(Ʈ���� ��ư)�ϸ� �ݶ��̴� ������, �浹 �� ������ ���� �����ϱ� ����
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
        if (IsHit) return;
        // �տ� ��ġ�� ������� �ϴ� �κ�(���⺤��)
        rb_this.velocity = (t_target.position - transform.position) / (Time.fixedDeltaTime);

        // ���� ������ �°� ���������ִ� �κ�, inverse ���� ���� ������ �νĸ���
        Quaternion q_rotationDifference = t_target.rotation * Quaternion.Inverse(transform.rotation);
        q_rotationDifference.ToAngleAxis(out float f_angleInDegree, out Vector3 v3_rotationAxis);

        // �� �ڵ尡 ������ Ư�� �������� ���� 180���� �Ѿ�� �ѹ��� ���ư� > ���� ���ư����� ���� ��Ʋ������ �ƴϸ� ��..., �޼�, ������ �񱳵� �ʼ� �Ұ���, �ϴ� �ڵ� �ؼ� ���� �ؾ���
        f_angleInDegree = Mathf.Clamp(f_angleInDegree, v2_rangeOfHandRotation.x, v2_rangeOfHandRotation.y);
        Vector3 v3_rotationDifferenceInDegree = f_angleInDegree * v3_rotationAxis;

        rb_this.angularVelocity = (v3_rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);

    }
}
