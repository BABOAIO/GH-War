using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class VRPlayerHit : MonoBehaviourPunCallbacks
{
    [Header("Max HP")]
    public float MaxHP = 2.0f;
    [Header("HP")]
    public float HP = 2.0f;

    float invincibilityTime = 2.0f;
    public float currentTime = 2.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow") && currentTime>=invincibilityTime)
        {
            print("VR È÷Æ®");
            Hit_VRPlayer(1);
            currentTime = 0.0f;
        }
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
    }

    [PunRPC]
    public void Hit_VRPlayer(float damage)
    {
        HP -= damage;
        Debug.Log($"VR Player {photonView.Controller} is Damaged : Dmg {damage}");

        if (HP <= 0)
        {
            HP = MaxHP;
            GameManager.instance.CheckRebirthVRPlayer();
        }
    }
}
