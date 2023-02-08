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
    // ��� ������ ���漭���� �ƴҶ� ���� ������
    [SerializeField] GameObject cannonBall;
    // �������� �߻�Ǵ� ����(������ �ִ� armatur3 > bone > bone_end �� �ִ´�.(���� ���ſ� ���� ������))
    [SerializeField] Transform tr_firePos;
    // �����ϴ� ����Ʈ�� ��� ������Ʈ(�Ǵ� ��ƼŬ)
    [SerializeField] GameObject o_exp;
    // ������ ���������� ���� ��
    [SerializeField] float f_shotPower = 150f;

    // ������ ��Ÿ���� ǥ�õ� �ؽ�Ʈ
    [SerializeField] Text txt_countDown;
    //[SerializeField] Animation anim_turretOpen;

    // ���� �ִϸ��̼�
    [SerializeField] Animator a_turret;

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
        currentTime = delayTime;
        b_isPress = false;
        tr_this = GetComponent<Transform>();
        v3_originPos= tr_this.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
    }

    private void FixedUpdate()
    {
        // ���� �ִϸ��̼� �� ���� ������ ������ �ʾ��� ���, ��Ÿ�� ����
        if (!a_turret.GetBool("Open"))
        {
            currentTime += Time.fixedDeltaTime;

            int i_currTime = (int)currentTime;

            txt_countDown.text = i_currTime.ToString();
        }

        if (currentTime > delayTime)
        {
            txt_countDown.text = "O.K.";

            
            if (photonView.IsMine)
            {
                if (PhotonNetwork.CountOfPlayersInRooms >= 2)
                {
                    photonView.RPC("TurretButtonPress", RpcTarget.All);
                }
                else
                {
                    TurretButtonPress();
                }
                //GetPressOrRelease();
            }
        }
    }

    IEnumerator TurretActive()
    {
        txt_countDown.text = "shot!!";

        a_turret.SetBool("Open", true);
        // �ִϸ��̼��� ������ �����ʾ� ������ �߰��� �������
        yield return new WaitForSeconds(1.0f);
        a_turret.SetBool("Fire", true);

        if (GameObject.FindGameObjectWithTag("VRPlayerHead"))
        {
            tr_firePos.LookAt(GameObject.FindGameObjectWithTag("VRPlayerHead").transform.position);
        }

        GameObject ball = PhotonNetwork.Instantiate("CannonBall", tr_firePos.position, Quaternion.identity);
        GameObject fireEffect = PhotonNetwork.Instantiate("HitEffect", tr_firePos.position, tr_firePos.rotation);
        ball.GetComponent<Rigidbody>().AddForce(
            // ���׿����ڷ� VR������ �������� �����̰�, ������ ���������� �߻�
            (GameObject.FindGameObjectWithTag("VRPlayerHead") ? tr_firePos.forward * f_shotPower : tr_firePos.up * f_shotPower)
            , ForceMode.Impulse);
        // ������ ����� ���, ������� �ʹ� ª���� ����
        yield return new WaitForSeconds(1.0f);

        Observable.NextFrame().Subscribe(_ => a_turret.SetBool("Fire", false));
        // ��Ÿ�� ������ ���� ����
        yield return new WaitForSeconds(0.5f);
        a_turret.SetBool("Open", false);

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
