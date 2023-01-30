using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 파티클 시스템이 끝나면 사라지게 하는 스크립트
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
