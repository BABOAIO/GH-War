using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWalkStateMachine : StateMachineBehaviour
{
    public AudioSource as_PCPlayer;
    public AudioClip ac_aimWalk;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        as_PCPlayer.PlayOneShot(ac_aimWalk);
    }
}
