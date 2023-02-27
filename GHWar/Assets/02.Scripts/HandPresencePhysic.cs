using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;

// VR �÷��̾��� ��(�ݶ��̴��� �ִ�)�� �־��ش�.
// ���� �ٴڿ� ����� ���, ������� ���ϰ� ����
public class HandPresencePhysic : MonoBehaviourPun
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
            PhotonNetwork.Destroy(collision.gameObject);
            as_parent.Stop();
            as_parent.Play();
            StartCoroutine(StopHandScript());
        }
    }

    IEnumerator StopHandScript()
    {
        IsHit = true;

        // ������ �ֱ����� DoTween �Լ�, OnStart�� ���� ���� �� �߻��� �Լ�
        transform.DOShakeRotation(0.5f, 30.0f).OnStart(() =>
        {
            transform.DOShakePosition(0.5f, 0.05f);
        }
        );

        // 3�ʵ��� �� �����
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
        // ���ʹϾ��� ���� : 180���� �ʰ��ϴ� ȸ���� ��Ÿ���� ����
        // ���Ϸ����� ���� : ������, �� ���� ������ ǥ���ϴ��� ���Ϸ��� �� �ٸ� �࿡ ���� ������ �ٸ� ������ �ؼ���.

        if (IsHit) return;
        if (photonView.IsMine)
        {
            // �ذ�å 1 : ���� ȸ������ �״�� ����
            // �� �κ��̸� �ѹ��� �ذ������, ���� ��� �κп��� ������� ������
            rb_this.velocity = (t_target.position - transform.position) / (Time.fixedDeltaTime);
            transform.rotation = t_target.rotation;
        }
        // �տ� ��ġ�� ������� �ϴ� �κ�(���⺤��)
        rb_this.velocity = (t_target.position - transform.position) / (Time.fixedDeltaTime);

        // �ذ�å 2 : ���� �����ӿ��� �����Ͽ� ������
        // ���� : ����䰡 ���� �ڿ������� ������ �߻�, ���� ��Ƶ� ����� �κ��� ����(�ٸ� ������� ������ ���������)
        // ���� : ���ʹϾ��� ������ �״�� �����´�, �� �־��� �࿡�� 180���� �Ѿ�� ���� ���ư�����

        //// ���ʹϾ��� 3x3����� ��ȯ��Ų �� >> ������ ������ �Ϸ���, ���� ����� �κ��� �ڿ� �־����.
        //// ex) BAv != ABv, A��ŭ ȸ���� �� B��ŭ ȸ���Ѵ�. != B��ŭ ȸ���� �� A��ŭ ȸ���Ѵ�. �⺻������ ȸ���� ���� �޶����� ����
        //// ������ �⺻������ ���� ���� + (- ���� ����) (-1�� ���ʹϾ��� ��� �����)
        //// �������� �ű� �� ���ϴ� �������� ȸ���Ѵٴ� �ǹ�
        //// ���͵� ���������� ���ϴ� ���͸� ���ϱ� ���� �������� �����ϴ� �� ������ ���� �̿��Ѵ�. �̿� ������
        //Quaternion q_rotationDifference = t_target.rotation * Quaternion.Inverse(transform.rotation);
        //// ���ʹϾ��� ȸ����� ȸ�������� ��ȯ��Ŵ, identity�� �� 1,0,0�� �� 0���� �з���
        //q_rotationDifference.ToAngleAxis(out float f_angleInDegree, out Vector3 v3_rotationAxis);

        //// ���ʹϾ��� �״�� ä���� ���� ���Ϸ��� �ٽ� ��ȯ >> ���� ��꿡 �ǹ̰� ������
        //// �ٸ�, ���ӵ��� ���ϱ� ���ؼ��� ���� �����̴� ���� �Ǻ��ϱ� ���ؼ� ����
        //// ���� ������ �ð�ٴ��� ��� �� ���� ��������, ª���� �� ���� �������� �߻��Ѵٰ� �����ϸ� �ȴ�.
        //Vector3 v3_rotationDifferenceInDegree = f_angleInDegree * v3_rotationAxis;

        //// ���Ϸ��� ��ȯ�� ���� �������� �����Ͽ� �����ð��� ���� ���ӵ��� ��ȯ
        //rb_this.angularVelocity = (v3_rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);
    }
}
