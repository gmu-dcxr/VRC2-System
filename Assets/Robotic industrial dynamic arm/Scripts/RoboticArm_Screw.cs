using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboticArm_Screw : MonoBehaviour
{
    // Start is called before the first frame update

    public bool gravityON;
    public float[] torque;

    public float drag = 0.1f;
    public float angDrag = 0.01f;


    public Rigidbody part0;
    public Rigidbody part1;
    public Rigidbody part2;
    public Rigidbody part3;
    public Rigidbody rotatingScrew;

    Rigidbody[] rbs;


    void Start()
    {
        //create array of rigidbodies for future use
        rbs = new Rigidbody[5];
        rbs[0] = part0;
        rbs[1] = part1;
        rbs[2] = part2;
        rbs[3] = part3;
        rbs[4] = rotatingScrew;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //set gravity if changed
        if (gravityON)
        {
            for (int ii = 0; ii < rbs.Length; ii++)
            {
                rbs[ii].useGravity = true;
            }

        }
        else
        {
            for (int ii = 0; ii < rbs.Length; ii++)
            {
                rbs[ii].useGravity = false;
            }
        }


        //set drags

        for (int ii = 0; ii < rbs.Length; ii++)
        {
            rbs[ii].drag = drag;
            rbs[ii].angularDrag = angDrag;
        }


        //setting forces to zero
        //setFrictionToJoints();

        //moving part 0
        if (Input.GetKey("a"))
        {
            part0.AddTorque(-torque[0] * part0.mass * part0.transform.forward);
        }
        if (Input.GetKey("d"))
        {
            part0.AddTorque(torque[0] * part0.mass * part0.transform.forward);
        }

        //moving part 1
        if (Input.GetKey("w"))
        {
            part1.AddTorque(-torque[1] * part1.mass * part1.transform.forward);
        }
        if (Input.GetKey("s"))
        {
            part1.AddTorque(torque[1] * part1.mass * part1.transform.forward);
        }

        //moving part 2
        if (Input.GetKey("q"))
        {
            part2.AddTorque(-torque[2] * part2.mass * part2.transform.forward);

        }
        if (Input.GetKey("e"))
        {
            part2.AddTorque(torque[2] * part2.mass * part2.transform.forward);
        }

        //moving part 3
        if (Input.GetKey("z"))
        {
            part3.AddTorque(-torque[3] * part3.mass * part3.transform.right);
        }
        if (Input.GetKey("c"))
        {
            part3.AddTorque(torque[3] * part3.mass * part3.transform.right);
        }


        //moving grips
        if (Input.GetKey("x"))
        {
            rotatingScrew.AddTorque(torque[4] * rotatingScrew.mass * rotatingScrew.transform.up);

        }
        





    }


}
