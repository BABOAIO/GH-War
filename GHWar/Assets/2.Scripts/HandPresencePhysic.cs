using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPresencePhysic : MonoBehaviour
{
    [SerializeField] Transform t_target;
    Rigidbody rb_this;

    void Start()
    {
        rb_this = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb_this.velocity = (t_target.position - this.transform.position) / Time.fixedDeltaTime;

        Quaternion q_rotationDifference = t_target.rotation*Quaternion.Inverse(transform.rotation);
        q_rotationDifference.ToAngleAxis(out float f_angleInDegree, out Vector3 v3_rotationAxis);

        Vector3 v3_rotationDifferenceInDegree = f_angleInDegree * v3_rotationAxis;

        rb_this.angularVelocity = (v3_rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);
    }
}
