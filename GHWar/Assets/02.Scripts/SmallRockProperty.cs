using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallRockProperty : MonoBehaviour
{
    AudioSource as_smallRock;
    [SerializeField] AudioClip ac_smallRockHit;

    private void Start()
    {
        as_smallRock = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GetComponent<Rigidbody>().velocity.magnitude >= 5f)
        {
            as_smallRock.Play();
        }
    }
}
