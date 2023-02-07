using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 파티클 시스템이 끝나면 사라지게 하는 스크립트
// 파티클 시스템이 있는 오브젝트에 넣어야 포톤서버에서 사라진다.
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
