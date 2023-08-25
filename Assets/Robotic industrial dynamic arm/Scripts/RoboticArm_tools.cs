using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboticArm_tools : MonoBehaviour
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
    public Rigidbody gripL;
    public Rigidbody gripR;
    public Rigidbody rotatingScrew;
    public GameObject platform;
    public GameObject laser;

    Rigidbody[] rbs;


    public bool grip;

    public int tool=1;

    void Start()
    {
        //create array of rigidbodies for future use
        rbs = new Rigidbody[7];
        rbs[0] = part0;
        rbs[1] = part1;
        rbs[2] = part2;
        rbs[3] = part3;
        rbs[4] = rotatingScrew;
        rbs[5] = gripL;
        rbs[6] = gripR;

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


        //moving tools
        if (Input.GetKey("x"))
        {
         
            if(Input.GetKeyDown("x") && tool==1)
            {
                grip = !grip;
            }
            if (tool == 2)
            {
                rotatingScrew.AddTorque(torque[4] * rotatingScrew.mass * rotatingScrew.transform.up);
            }

            if (Input.GetKeyDown("x") && tool == 4)
            {
                grip = !grip;
            }
        }




        if (grip)
        {
            gripL.AddTorque(torque[4] * gripL.mass * gripL.transform.forward);
            gripR.AddTorque(torque[4] * gripR.mass * gripR.transform.forward);

        }
        else
        {
            gripL.AddTorque(-torque[4] * gripL.mass * gripL.transform.forward);
            gripR.AddTorque(-torque[4] * gripR.mass * gripR.transform.forward);

        }



        //set tools
        if(Input.GetKeyDown("1"))
        {
            tool = 1;
        }
        else if(Input.GetKeyDown("2"))
        {
            tool = 2;
        }
        else if(Input.GetKeyDown("3"))
        {
            tool = 3;
        }
        else if(Input.GetKeyDown("4"))
        {
            tool = 4;
        }

            switch (tool)
        {
            case 1:
                hideAll();
                gripL.GetComponent<Renderer>().enabled = (true);
                gripR.GetComponent<Renderer>().enabled = (true);
                break;
            case 2:
                hideAll();
                rotatingScrew.GetComponent<Renderer>().enabled = (true);

                break;
            case 3:
                hideAll();
                platform.SetActive (true);

                break;
            case 4:
                hideAll();
                laser.SetActive(true);

                break;
        }



        }

    //to hide all objects
    public void hideAll()
    {
        gripL.gameObject.GetComponent<Renderer>().enabled=(false);
        gripR.gameObject.GetComponent<Renderer>().enabled = (false);

        rotatingScrew.GetComponent<Renderer>().enabled = (false);

        platform.SetActive(false);

        laser.SetActive(false);
    }

}
