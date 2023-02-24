using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyArrayStudy : MonoBehaviour
{
    int[] myInt = new int[5];

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            for(int i = 0; i < myInt.Length; i++)
            {
                myInt[i] = i;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
        }
    }
}
