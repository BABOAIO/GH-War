using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PCPlayerMoveSound : MonoBehaviourPun
{
    AudioSource as_PCPlayerMoveSound;

    [SerializeField] AudioClip ac_AimWalk;
    [SerializeField] AudioClip ac_Run;
    [SerializeField] AudioClip ac_Shot;
    [SerializeField] AudioClip ac_Roll;
    [SerializeField] AudioClip ac_Jump;


    private void Start()
    {
        as_PCPlayerMoveSound = GetComponent<AudioSource>();
    }

    public void SoundOfAimWalk()
    {
        as_PCPlayerMoveSound.PlayOneShot(ac_AimWalk);
    }

    public void SoundOfRun()
    {
        as_PCPlayerMoveSound.PlayOneShot(ac_Run);
    }
    public void SoundOfShot()
    {
        as_PCPlayerMoveSound.PlayOneShot(ac_Shot);
    }
    public void SoundOfRoll()
    {
        as_PCPlayerMoveSound.PlayOneShot(ac_Roll);
    }
    public void SoundOfJump()
    {
        as_PCPlayerMoveSound.PlayOneShot(ac_Jump);
    }
}
