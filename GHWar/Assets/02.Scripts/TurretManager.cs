using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.UI;
using UniRx;

// ������ �ִ� Button�� �ִ´�.
// Configurable Joint�� ���� ���� Ʈ���ŷ� �ν�
public class TurretManager : MonoBehaviourPunCallbacks
{
    [SerializeField] string str_TargetTag = "VRPlayerHead";

    AudioSource as_turret;
    [SerializeField] AudioClip ac_openTurret;
    [SerializeField] AudioClip ac_closeTurret;
    [SerializeField] AudioClip ac_shotTurret;
    // �������� �߻�Ǵ� ����(������ �ִ� armatur3 > bone > bone_end �� �ִ´�.(���� ���ſ� ���� ������))
    [SerializeField] List<Transform> list_tr_firePos = new List<Transform>();
    // ������ ���������� ���� ��
    [SerializeField] float f_shotPower = 150f;

    // ������ ��Ÿ���� ǥ�õ� �ؽ�Ʈ
    [SerializeField] Text txt_countDown;
    //[SerializeField] Animation anim_turretOpen;

    // ���� �ִϸ��̼�
    [SerializeField] List<Animator> list_A_turrets = new List<Animator>();

    // ��ư �νĿ� ���� ���� ����
    [SerializeField] float f_errorRange = 0.025f;
    // ���� ��Ÿ��
    [SerializeField] float delayTime = 5f;
    float currentTime;

    Transform tr_this;
    // ��ư�� ���� ������ �ν�����â���� �����ϸ� �װ��� �������
    ConfigurableJoint _joint;
    
    // ��ư �ν� üũ
    bool b_isPress;
    // ���� ��ư�� ��ġ
    Vector3 v3_originPos;

    // ����Ƽ �̺�Ʈ�� ���� �־��ָ� �� ��ũ��Ʈ ��Ĵ�� �۵�, ������Ʈ�� ���� ���ذ� ��.
    //public UnityEvent OnPressButton_Turret, OnReleaseButton_Turret;

    void Start()
    {
        as_turret = GetComponent<AudioSource>();
        currentTime = delayTime;
        b_isPress = false;
        tr_this = GetComponent<Transform>();
        v3_originPos= tr_this.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
    }

    private void FixedUpdate()
    {
        for(int i =0; i< list_A_turrets.Count; i++)
        {
            if (!list_A_turrets[i].GetBool("Open"))
            {

                currentTime += Time.fixedDeltaTime;

                int i_currTime = (int)currentTime;

                txt_countDown.text = i_currTime.ToString();
            }
        }

        if (currentTime > delayTime)
        {
            txt_countDown.text = "O.K.";
            // photonView.RPC("TurretButtonPress", RpcTarget.All);
            // RPC�� ���� ���, ��� ������ ���ôٹ������� �������� ������
            // ���� ��ġ����ȭ�� ��Ȯ�� ��� ������ �߻��ϵ��� ��
            TurretButtonPress();

            // ���� �����ڵ鿡�� �Ⱥ��̰� �ϱ� ���� ��ġ
            // ���� �������ָ�, RPC �ߴ�
            PhotonNetwork.SendAllOutgoingCommands();
        }
    }

    IEnumerator TurretActive()
    {
        print("�߻�");
        txt_countDown.text = "shot!!";

        as_turret.PlayOneShot(ac_openTurret, 0.5f);
        for (int i = 0; i < list_A_turrets.Count; i++)
        {
            list_A_turrets[i].SetBool("Open", true);
        }
        // �ִϸ��̼��� ������ �����ʾ� ������ �߰��� �������
        yield return new WaitForSeconds(1.0f);
        as_turret.Stop();
        as_turret.PlayOneShot(ac_shotTurret, 0.5f);
        for (int i = 0; i < list_A_turrets.Count; i++)
        {
            list_A_turrets[i].SetBool("Fire", true);
        }

        if (GameObject.FindGameObjectWithTag(str_TargetTag))
        {
            Vector3 v3_TargetPos = GameObject.FindGameObjectWithTag(str_TargetTag).transform.position;
            for (int i = 0; i<list_tr_firePos.Count; i++)
            {
                list_tr_firePos[i].LookAt(v3_TargetPos);
            }
        }

        if (photonView.IsMine)
        {
            GameObject ball;
            GameObject fireEffect;
            for (int i = 0; i < list_tr_firePos.Count; i++)
            {
                ball = PhotonNetwork.Instantiate("CannonBall", list_tr_firePos[i].position, Quaternion.identity, 0, null);
                fireEffect = PhotonNetwork.Instantiate("HitEffect", list_tr_firePos[i].position, list_tr_firePos[i].rotation);
                ball.GetComponent<Rigidbody>().AddForce(
                    // ���׿����ڷ� VR������ �������� �����̰�, ������ ���������� �߻�
                    (GameObject.FindGameObjectWithTag(str_TargetTag) ? list_tr_firePos[i].forward * f_shotPower : list_tr_firePos[i].up * f_shotPower)
                    // �ѹ��� ��� �������� ������ �ֱ� ����
                    , ForceMode.Impulse);
            }
            //GameObject ball = PhotonNetwork.Instantiate("CannonBall", tr_firePos.position, Quaternion.identity, 0, null);
            //GameObject fireEffect = PhotonNetwork.Instantiate("HitEffect", tr_firePos.position, tr_firePos.rotation);
            //ball.GetComponent<Rigidbody>().AddForce(
            //    // ���׿����ڷ� VR������ �������� �����̰�, ������ ���������� �߻�
            //    (GameObject.FindGameObjectWithTag(str_TargetTag) ? tr_firePos.forward * f_shotPower : tr_firePos.up * f_shotPower)
            //    // �ѹ��� ��� �������� ������ �ֱ� ����
            //    , ForceMode.Impulse);
        }
        // ������ ����� ���, ���� ��Ÿ���� ��������� �ð�
        yield return new WaitForSeconds(0.8f);
        as_turret.Stop();

        Observable.NextFrame().Subscribe(_ =>
        {
            for (int i = 0; i < list_A_turrets.Count; i++)
            {
                list_A_turrets[i].SetBool("Fire", false);
            }
        });
        as_turret.Stop();
        as_turret.PlayOneShot(ac_closeTurret, 0.5f);
        for (int i = 0; i < list_A_turrets.Count; i++)
        {
            list_A_turrets[i].SetBool("Open", false);
        }

        // �̹� ��ũ��Ʈ�� �־ �Ⱦ��ֵ���
        //PhotonNetwork.Destroy(fireEffect);

        yield return null;
    }

    [PunRPC]
    void TurretButtonPress()
    {
        if (b_isPress)
        {
            // ���� ��ġ�� �ö�� ��ư ��ġ�� ���̰� �������� ���� ���,
            if (Mathf.Abs(tr_this.localPosition.y - v3_originPos.y) < f_errorRange)
            {
                Debug.Log("Turret �ȴ���..");
                b_isPress = true;
                //OnPressButton_Turret.Invoke();

            }
        }
        else if (!b_isPress)
        {
            // ���� ��ġ�� ���� ��ư ��ġ�� ���̰� �������� ���� ���,
            if (Mathf.Abs(tr_this.localPosition.y - _joint.linearLimit.limit) < f_errorRange)
            {
                Debug.Log("Turret ����!");
                b_isPress = false;
                //OnReleaseButton_Turret.Invoke();

                //StopCoroutine(TurretActive());
                currentTime = 0f;

                StartCoroutine(TurretActive());
            }

        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("PC_Player"))
    //    {
    //        photonView.RequestOwnership();
    //    }
    //}

    // �ִϸ��̼� �̺�Ʈ, Ư�� �ִϸ��̼� ���߿� �Լ��� �־ �̺�Ʈ �߻�, ������ ��� ��...
    //[PunRPC]
    //void GetPressOrRelease()
    //{
    //    // ������ ����
    //    if (!b_isPress && !anim_turretOpen.isPlaying)
    //    {
    //        if(Mathf.Abs(tr_this.localPosition.y - v3_originPos.y) < f_errorRange)
    //        {
    //            Debug.Log("Turret �ȴ���..");
    //            b_isPress = true;
    //            //OnPressButton_Turret.Invoke();
    //        }
    //    }
    //    // ������ ����
    //    if (b_isPress)
    //    {
    //        if (Mathf.Abs(tr_this.localPosition.y - _joint.linearLimit.limit) < f_errorRange)
    //        {
    //            Debug.Log("Turret ����!");
    //            b_isPress = false;
    //            //OnReleaseButton_Turret.Invoke();

    //            anim_turretOpen.Play();
    //            currentTime = 0f;
    //        }
    //    }
    //}
}
