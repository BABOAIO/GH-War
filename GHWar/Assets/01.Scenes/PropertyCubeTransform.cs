using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyCubeTransform : MonoBehaviour
{
    [SerializeField]
    bool moveReset = false;

    public Quaternion angleX50 = Quaternion.Euler(new Vector3(50, 0, 0));
    public Quaternion angleY45 = Quaternion.Euler(new Vector3(0, 45, 0));
    public Quaternion angleZ40 = Quaternion.Euler(new Vector3(0, 0, 40));

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
        //if(Input.GetKeyDown(KeyCode.Alpha1)) { moveReset = !moveReset; MoveReset = moveReset; }

        //print($"X + Y = {angleY45 * angleX50}");
        //print($"Y + X = {angleX50 * angleY45}");
        //print($"X + Y? = {Quaternion.Euler(new Vector3(50, 45, 0))}");
        //print($"X + Y? = {Quaternion.Inverse(Quaternion.Euler(new Vector3(50, 45, 0)))}");
        //print($"Y + Z = {angleZ40 * angleY45}");
        //print($"Z + Y = {angleY45 * angleZ40}");
        //print($"X + Z = {angleZ40 * angleX50}");
        //print($"Z + X = {angleX50 * angleZ40}");

        Quaternion q = transform.rotation;
        q.ToAngleAxis(out float a, out Vector3 v3);
        print($"angle {a} axis {v3}");
    }
}
