using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public LineRenderer linR;
    Vector3 origin, destination;
    public ParticleSystem partCS;
    // Start is called before the first frame update
    void Start()
    {
        laserBeam();
    }

    // Update is called once per frame
    void Update()
    {
        laserBeam();
    }

    public void laserBeam()
    {
        RaycastHit hit;
        origin = transform.position;


        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position,- transform.up, out hit, Mathf.Infinity))
        {
            destination = transform.position - transform.up * hit.distance;
            partCS.transform.position = destination;
        }

        linR.SetPosition(0, origin);
        linR.SetPosition(1, destination);
    }
}
