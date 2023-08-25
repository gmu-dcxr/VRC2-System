using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboticArm_Screw_IK : MonoBehaviour
{
    // Start is called before the first frame update

    // gravity will work if this is enabled
    public bool gravityON;

    //toque applied to each part of the robot
    public float[] torque;
    
    //error in quaternion
    public float error = 0.001f;

    //drags
    public float drag = 0.1f;
    public float angDrag = 0.01f;
    
    //movement of the robot
    public float stepMovement;
    public bool automovement = false;
    
    //parts of the robotic arm
    public Rigidbody part0;
    public Rigidbody part1;
    public Rigidbody part2;
    public Rigidbody part3;
    public Rigidbody screw;

    public bool turn = false;

    Rigidbody[] rbs;

    // angle errors
    public float aT, aQ1, aQ2, aQ3;

    //quaternion for the screwing position
    public Quaternion Tscrew, Q1screw, Q2screw, Q3screw;
    //quaternion to display rotation of the arm
    public Quaternion t_arm, q1_arm, q2_arm, q3_arm;

    Quaternion t_arm0, q1_arm0, q2_arm0, q3_arm0;

    void Start()
    {
        //create array of rigidbodies for future use
        rbs = new Rigidbody[5];
        rbs[0] = part0;
        rbs[1] = part1;
        rbs[2] = part2;
        rbs[3] = part3;
        rbs[4] = screw;

        //show reative rotation on the inspector and set the initial rotations
        t_arm = part0.transform.rotation;
        q1_arm = part1.transform.rotation;
        q2_arm = part2.transform.rotation;
        q3_arm = part3.transform.rotation;

        // initial angles of the arm
        t_arm0 = part0.transform.rotation;
        q1_arm0 = part1.transform.rotation;
        q2_arm0 = part2.transform.rotation;
        q3_arm0 = part3.transform.rotation;


    }

    public void startScrew()
    {
        if (automovement)
        { StartCoroutine(movement()); }

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

        if (automovement == false)
        {
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
            

        }


        //moving part
        if (turn)
        {
            screw.AddTorque(torque[4] * screw.mass * screw.transform.up);

        }
        else
        {
            screw.MoveRotation(Quaternion.Euler(90, 0, 0));
        }
       

        //show reative rotation on the inspector
        t_arm = part0.transform.rotation;
        q1_arm = part1.transform.rotation;
        q2_arm = part2.transform.rotation;
        q3_arm = part3.transform.rotation;


    }

    IEnumerator movement()
    {
              //move to screw
            turn = true;
            yield return moveArm(Tscrew, Q1screw, Q2screw, Q3screw);
            turn = false;
            yield return new WaitForSeconds(1);

            //move to original position
            yield return moveArm(t_arm0, q1_arm0, q2_arm0, q3_arm0);
    }


    //this is the funciton used to move to a specific arm position
    IEnumerator moveArm(Quaternion T, Quaternion q1, Quaternion q2, Quaternion q3)
    {


        aT = quatError(t_arm, T);
        aQ1 = quatError(q1_arm, q1);
        aQ2 = quatError(q2_arm, q2);
        aQ3 = quatError(q3_arm, q3);


        while (Mathf.Abs(aT) < 1 - error || Mathf.Abs(aQ1) < 1 - error || Mathf.Abs(aQ2) < 1 - error || Mathf.Abs(aQ3) < 1 - error)
        {

            aT = quatError(t_arm, T);
            aQ1 = quatError(q1_arm, q1);
            aQ2 = quatError(q2_arm, q2);
            aQ3 = quatError(q3_arm, q3);


            part0.MoveRotation(Quaternion.RotateTowards(t_arm, T, stepMovement));
            yield return null;
            part1.MoveRotation(Quaternion.RotateTowards(q1_arm, q1, stepMovement));
            yield return null;
            part2.MoveRotation(Quaternion.RotateTowards(q2_arm, q2, stepMovement));
            yield return null;
            part3.MoveRotation(Quaternion.RotateTowards(q3_arm, q3, stepMovement));
            yield return null;

        }


    }
    //quaternion relative error
    public float quatError(Quaternion q, Quaternion p)
    {
        return (Quaternion.Dot(p, q));

    }

}
