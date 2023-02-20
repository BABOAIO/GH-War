using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// Resource ������ �ִ� Arrow�� �ִ´�.
[RequireComponent(typeof(AudioSource))]
public class ArrowProperty_PowerShot : MonoBehaviour
{
    // ���� �� ���󰡴� �ӵ�
    [SerializeField] float shotSpeed = 20.0f;
    Rigidbody rb_this;
    Transform tr_this;

    [SerializeField] Transform tr_COM;

    // �ε������� ��� ������ ����
    bool isHit = false;

    // �Ҹ��κ� //
    AudioSource as_arrow;
    //[SerializeField] AudioClip ac_shotArrow;
    [SerializeField] AudioClip ac_shotHit;
    // �Ҹ��κ� //

    void Awake()
    {
        //rb_this.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        isHit = false;
    }

    private void Start()
    {
        tr_this = GetComponent<Transform>();
        as_arrow= GetComponent<AudioSource>();
        rb_this = GetComponent<Rigidbody>();
        rb_this.centerOfMass = tr_COM.position;
        //rb_this.AddForce(new Vector3(0, 0, shotPower));
    }

    void FixedUpdate()
    {
        // �ε����� �ʾ��� ���, ������ ���󰣴�.(�߷��� ������ ������� �޴°����� ���δ�.)
        // ���� ȭ�쳢�� �ε����� �ʰ� ����(���̾� �������� ��������)
        if (!isHit)
        {
            tr_this.Translate(Vector3.forward * shotSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LeftHandPhysics") || collision.gameObject.layer == LayerMask.NameToLayer("RightHandPhysics"))
        {
            isHit = true;

            GameObject o_ps =
            PhotonNetwork.Instantiate("HitEffectWithEXP", collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].point, collision.contacts[0].normal));

            PhotonNetwork.Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("VRPlayerHead"))
        {
            isHit = true;

            GameObject o_ps =
            PhotonNetwork.Instantiate("HitEffectWithEXP", collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].point, collision.contacts[0].normal));

            PhotonNetwork.Destroy(this.gameObject);
            //StartCoroutine(DestroyDelayed(gameObject, 0.1f));
        }

        // ���� �޽��� �Ⱥ����� �̷��� ���ϸ� ���� ����
        else if (!collision.gameObject.CompareTag("Ground"))
        {
        }

        else if(!collision.gameObject.CompareTag("PC_Player"))
        {
            isHit = true;

            GameObject o_ps = 
            PhotonNetwork.Instantiate("HitEffectWithEXP", collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].point, collision.contacts[0].normal));

            PhotonNetwork.Destroy(this.gameObject);
            //StartCoroutine(DestroyDelayed(gameObject, 0.1f));
        }
    }

    IEnumerator DestroyDelayed(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (obj != null) 
        {
            PhotonNetwork.Destroy(obj);
        }
    }
}
