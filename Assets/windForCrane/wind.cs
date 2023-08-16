using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Wind : MonoBehaviour
{

    public float windForce = 0f;

    void OnTriggerStay(Collider other)
    {
        var hitObj = other.gameObject;
        if (hitObj != null)
        {
            var dir = Vector3.zero;
            dir.x = Random.Range(-90, 90);
            dir.z = Random.Range(-90, 90);
            var rb = hitObj.GetComponent<Rigidbody>();
            rb.AddForce(dir * windForce);
        }
    }
}
