using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.UI;
using UniRx;

// 대포에 있는 Button에 넣는다.
// Configurable Joint의 값에 따라 트리거로 인식
public class TurretManager : MonoBehaviourPunCallbacks
{
    [SerializeField] string str_TargetTag = "VRPlayerHead";

    AudioSource as_turret;
    [SerializeField] AudioClip ac_openTurret;
    [SerializeField] AudioClip ac_closeTurret;
    [SerializeField] AudioClip ac_shotTurret;
    // 대포알이 발사되는 지점(대포에 있는 armatur3 > bone > bone_end 에 넣는다.(대포 포신에 따라 움직임))
    [SerializeField] List<Transform> list_tr_firePos = new List<Transform>();
    // 대포에 순간적으로 가할 힘
    [SerializeField] float f_shotPower = 150f;

    // 대포의 쿨타임이 표시될 텍스트
    [SerializeField] Text txt_countDown;
    //[SerializeField] Animation anim_turretOpen;

    // 대포 애니메이션
    [SerializeField] List<Animator> list_A_turrets = new List<Animator>();

    // 버튼 인식에 대한 오차 범위
    [SerializeField] float f_errorRange = 0.025f;
    // 대포 쿨타임
    [SerializeField] float delayTime = 5f;
    float currentTime;

    Transform tr_this;
    // 버튼이 눌린 정도를 인스펙터창에서 조절하며 그값을 가지고옴
    ConfigurableJoint _joint;
    
    // 버튼 인식 체크
    bool b_isPress;
    // 원래 버튼의 위치
    Vector3 v3_originPos;

    // 유니티 이벤트를 직접 넣어주면 그 스크립트 방식대로 작동, 컴포넌트를 보면 이해가 됨.
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
            // RPC를 넣을 경우, 모든 곳에서 동시다발적으로 대포알이 생성됨
            // 차라리 위치동기화를 정확히 잡아 누르면 발사하도록 함
            TurretButtonPress();

            // 이후 참가자들에게 안보이게 하기 위한 장치
            // 로컬 지연없애며, RPC 중단
            PhotonNetwork.SendAllOutgoingCommands();
        }
    }

    IEnumerator TurretActive()
    {
        print("발사");
        txt_countDown.text = "shot!!";

        as_turret.PlayOneShot(ac_openTurret, 0.5f);
        for (int i = 0; i < list_A_turrets.Count; i++)
        {
            list_A_turrets[i].SetBool("Open", true);
        }
        // 애니메이션이 끝까지 가지않아 대포가 중간에 멈춤방지
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
                    // 삼항연산자로 VR없으면 대포따라 움직이고, 있으면 대포쪽으로 발사
                    (GameObject.FindGameObjectWithTag(str_TargetTag) ? list_tr_firePos[i].forward * f_shotPower : list_tr_firePos[i].up * f_shotPower)
                    // 한번에 쏘는 대포같은 느낌을 주기 위해
                    , ForceMode.Impulse);
            }
            //GameObject ball = PhotonNetwork.Instantiate("CannonBall", tr_firePos.position, Quaternion.identity, 0, null);
            //GameObject fireEffect = PhotonNetwork.Instantiate("HitEffect", tr_firePos.position, tr_firePos.rotation);
            //ball.GetComponent<Rigidbody>().AddForce(
            //    // 삼항연산자로 VR없으면 대포따라 움직이고, 있으면 대포쪽으로 발사
            //    (GameObject.FindGameObjectWithTag(str_TargetTag) ? tr_firePos.forward * f_shotPower : tr_firePos.up * f_shotPower)
            //    // 한번에 쏘는 대포같은 느낌을 주기 위해
            //    , ForceMode.Impulse);
        }
        // 대포가 쏘았을 경우, 들어가서 쿨타임을 돌리기까지 시간
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

        // 이미 스크립트에 있어서 안없애도됨
        //PhotonNetwork.Destroy(fireEffect);

        yield return null;
    }

    [PunRPC]
    void TurretButtonPress()
    {
        if (b_isPress)
        {
            // 원래 위치와 올라온 버튼 위치의 차이가 오차보다 작을 경우,
            if (Mathf.Abs(tr_this.localPosition.y - v3_originPos.y) < f_errorRange)
            {
                Debug.Log("Turret 안눌림..");
                b_isPress = true;
                //OnPressButton_Turret.Invoke();

            }
        }
        else if (!b_isPress)
        {
            // 원래 위치와 눌린 버튼 위치의 차이가 오차보다 작을 경우,
            if (Mathf.Abs(tr_this.localPosition.y - _joint.linearLimit.limit) < f_errorRange)
            {
                Debug.Log("Turret 눌림!");
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

    // 애니메이션 이벤트, 특정 애니메이션 도중에 함수를 넣어서 이벤트 발생, 대포를 쏘는 등...
    //[PunRPC]
    //void GetPressOrRelease()
    //{
    //    // 누르기 시작
    //    if (!b_isPress && !anim_turretOpen.isPlaying)
    //    {
    //        if(Mathf.Abs(tr_this.localPosition.y - v3_originPos.y) < f_errorRange)
    //        {
    //            Debug.Log("Turret 안눌림..");
    //            b_isPress = true;
    //            //OnPressButton_Turret.Invoke();
    //        }
    //    }
    //    // 누르기 해제
    //    if (b_isPress)
    //    {
    //        if (Mathf.Abs(tr_this.localPosition.y - _joint.linearLimit.limit) < f_errorRange)
    //        {
    //            Debug.Log("Turret 눌림!");
    //            b_isPress = false;
    //            //OnReleaseButton_Turret.Invoke();

    //            anim_turretOpen.Play();
    //            currentTime = 0f;
    //        }
    //    }
    //}
}
