using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboticArm_physics : MonoBehaviour
{
    // Start is called before the first frame update

    public bool gravityON;
    public float torque = 80;
    public float friction = 0.5f;
    public float drag=0.1f;
    public float angDrag=0.01f;


    public Rigidbody part0;
    public Rigidbody part1;
    public Rigidbody part2;
    public Rigidbody part3;
    public Rigidbody gripLeft;
    public Rigidbody gripRight;

    public bool grip = false;

    Rigidbody[] rbs;


    void Start()
    {
        //create array of rigidbodies for future use
        rbs = new Rigidbody[6];
        rbs[0] = part0;
        rbs[1] = part1;
        rbs[2] = part2;
        rbs[3] = part3;
        rbs[4] = gripLeft;
        rbs[5] = gripRight;

            

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //set gravity if changed
        if (gravityON)
        {
            for(int ii=0; ii<rbs.Length;ii++)
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
            part0.AddTorque(-torque*part0.mass*part0.transform.forward);
        }
        if (Input.GetKey("d"))
        {
            part0.AddTorque(torque * part0.mass * part0.transform.forward);
        }

        //moving part 1
        if (Input.GetKey("w"))
        {
            part1.AddTorque(-torque * part1.mass * part1.transform.forward);
        }
        if (Input.GetKey("s"))
        {
            part1.AddTorque(torque * part1.mass * part1.transform.forward);
        }

        //moving part 2
        if (Input.GetKey("q"))
        {
            part2.AddTorque(-torque * part2.mass * part2.transform.forward);

        }
        if (Input.GetKey("e"))
        {
            part2.AddTorque(torque * part2.mass * part2.transform.forward);
        }

        //moving part 3
        if (Input.GetKey("z"))
        {
            part3.AddTorque(-torque * part3.mass * part3.transform.right);
        }
        if (Input.GetKey("c"))
        {
            part3.AddTorque(torque * part3.mass * part3.transform.right);
        }


        //moving grips
        if(Input.GetKeyDown("x"))
        {
            grip = !grip;
            
        }

        
        if (grip)
        {
            gripLeft.AddTorque(torque * gripLeft.mass * gripLeft.transform.forward);
            gripRight.AddTorque(torque * gripRight.mass * gripRight.transform.forward);

        }
        else
        {
            gripLeft.AddTorque(-torque * gripLeft.mass * gripLeft.transform.forward);
            gripRight.AddTorque(-torque * gripRight.mass * gripRight.transform.forward);

        }
        




    }


}
