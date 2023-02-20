using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyCubeTransform : MonoBehaviour
{
    [SerializeField]
    bool moveReset = false;

    public bool MoveReset
    {
        get { return moveReset; }
        set 
        {
            moveReset = value;
            if(value)
            {
                print(gameObject.name + " " + value);
            }
            else
            {
                print(gameObject.name + " " + value);
            }
        }
    }


    void Start()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) { moveReset = !moveReset; MoveReset = moveReset; }
    }
}
