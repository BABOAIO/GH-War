using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVelocityValue : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print($"{this.gameObject.name} Speed : {this.GetComponent<Rigidbody>().velocity.magnitude}");
        
    }
}
