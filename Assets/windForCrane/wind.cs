using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind : MonoBehaviour
{

    public float windForce = 0f;

    void OnTriggerStay(Collider other)
    {
        var hitObj = other.gameObject;
        if (hitObj != null)
        {
            var rb = hitObj.GetComponent<Rigidbody>();
            var dir = transform.right;
            rb.AddForce(dir * windForce);
        }
    }
}
