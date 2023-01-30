using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObjDelete : MonoBehaviour
{
    ParticleSystem ps_this;

    void Start()
    {
        ps_this= GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if(!ps_this.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
