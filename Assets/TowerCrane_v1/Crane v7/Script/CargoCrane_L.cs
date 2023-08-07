using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoCrane_L : MonoBehaviour
{
    public Transform pointLineCargo1;
    public Transform pointLineCargo2;
    public Transform pointLineCargo3;
    public Transform pointLineCargo4;
    public float mass = 100;
    public bool destroyRigidbody = true;
    public float timeDestroy = 10;

    IEnumerator DestroyComponent()
    {
        yield return new WaitForSeconds(timeDestroy);
        if (gameObject.GetComponent<ConstantForce>() != null)
        {
            DestroyImmediate(gameObject.GetComponent<ConstantForce>());
        }
        if (destroyRigidbody == true)
        {
            if (gameObject.GetComponent<HingeJoint>() == null && gameObject.GetComponent<Rigidbody>() != null)
            {
                DestroyImmediate(gameObject.GetComponent<Rigidbody>());
            }
        }
    }
}
