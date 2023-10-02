using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcavAnimPlayer : MonoBehaviour
{
    private Animator anim;
    public Transform digPoint1;
    public Transform digPoint2;
    public Transform digPoint3;
    private Transform destination;
    internal enum part
    {
        nextTo = 0,
        into1 = 1,
        into2 = 2,

    }
    private part pt;


    // Start is called before the first frame update
    void Start()
    {
        destination = digPoint1;
        anim = GetComponent<Animator>();
        //anim.SetBool("Dig", true);
    }

    // Update is called once per frame
    void Update()
    {
        //if(pt == part.nextTo)
        //{
            if (ReachDestination(destination.position))
            {
                anim.SetBool("Forward", false);
                //anim.enabled = false;
                anim.SetBool("Dig", true);
                //anim.enabled = true;

            }
        //}
    }

    bool ReachDestination(Vector3 des)
    {
        var t = transform.position;
        // use the same y
        des.y = 0;
        t.y = 0;
        var distance = Vector3.Distance(t, des);

        if (distance < 1.0f)
        {
            return true;
        }
        return false;
    }

    public void start_2()
    {
        //go forward til its at location 2
        //then dig
        //then dump
        pt = part.nextTo;
        destination = digPoint1;
        anim.SetBool("Forward", true);
    }

    public void start_3()
    {
        //go forward til its at location 3
        //then dig
        //then dump
        anim.SetBool("Dig",false);
        pt = part.into1;
        destination = digPoint2;
        anim.SetBool("Forward", true);
    }

    public void start_4()
    {
        //go forward til its at location 4
        //then dig
        //then dump
        anim.SetBool("Dig", false);
        pt = part.into2;
        destination = digPoint3;
        anim.SetBool("Forward", true);
    }

}
