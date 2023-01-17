using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    PlayerController sc_playerController;

    private void Awake()
    {
        sc_playerController= GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == sc_playerController.gameObject) return;
        sc_playerController.SetGroundedState(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == sc_playerController.gameObject) return;
        sc_playerController.SetGroundedState(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == sc_playerController.gameObject) return;
        sc_playerController.SetGroundedState(true);
    }
}
