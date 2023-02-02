using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoChildrenDestroy : MonoBehaviour
{
    ParticleSystem ps_child;

    void Start()
    {
        ps_child = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (!ps_child.isPlaying || !ps_child)
        {
            Destroy(gameObject);
        }
    }
}
