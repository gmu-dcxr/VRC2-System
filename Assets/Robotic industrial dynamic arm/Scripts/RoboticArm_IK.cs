using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this is a prototype script

public class RoboticArm_IK : MonoBehaviour
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


    //part fo the robot
    public Rigidbody part0;
    public Rigidbody part1;
    public Rigidbody part2;
    public Rigidbody part3;
    public Rigidbody gripLeft;
    public Rigidbody gripRight;

    public bool grip = false;

    //rigidbodies
    Rigidbody[] rbs;

    //used to instantiate a cube
    public GameObject cubePrefab;
    public Transform cubePos;

    public float aT, aQ1, aQ2, aQ3;

    //quaternions that are used for pick position and drop position
    public Quaternion Tpick, Q1pick, Q2pick, Q3pick;
    public Quaternion Tdrop, Q1drop, Q2drop, Q3drop;

    //quaternions that are used for displating the arm rotation
    public Quaternion t_arm, q1_arm, q2_arm, q3_arm;
    Quaternion t_arm0, q1_arm0, q2_arm0, q3_arm0;

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

        //show reative rotation on the inspector and set the initial rotations
        t_arm = part0.transform.rotation;
        q1_arm = part1.transform.rotation;
        q2_arm = part2.transform.rotation;
        q3_arm = part3.transform.rotation;

        //
        t_arm0 = part0.transform.rotation;
        q1_arm0 = part1.transform.rotation;
        q2_arm0 = part2.transform.rotation;
        q3_arm0 = part3.transform.rotation;


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


            //moving grips
            if (Input.GetKeyDown("x"))
            {
                grip = !grip;

            }



        }
        if (grip)
        {
            gripLeft.AddTorque(torque[4] * gripLeft.mass * gripLeft.transform.forward);
            gripRight.AddTorque(torque[4] * gripRight.mass * gripRight.transform.forward);

        }
        else
        {
            gripLeft.AddTorque(-torque[4] * gripLeft.mass * gripLeft.transform.forward);
            gripRight.AddTorque(-torque[4] * gripRight.mass * gripRight.transform.forward);

        }



        //show reative rotation on the inspector
        t_arm = part0.transform.rotation;
        q1_arm = part1.transform.rotation;
        q2_arm = part2.transform.rotation;
        q3_arm = part3.transform.rotation;


    }

    IEnumerator movement()
    {
        while (true)
        {

            //move to pick object
            yield return moveArm(Tpick, Q1pick, Q2pick, Q3pick);
            grip = true;
            yield return new WaitForSeconds(1);

            //move to original position
            yield return moveArm(t_arm0, q1_arm0, q2_arm0, q3_arm0);

            //move to drop object
            yield return moveArm(Tdrop, Q1drop, Q2drop, Q3drop);
            yield return new WaitForSeconds(1);
            grip = false;
            yield return new WaitForSeconds(1);

            Instantiate(cubePrefab, cubePos.position, Quaternion.Euler(0,0,0));

            //move to original position
            yield return moveArm(t_arm0, q1_arm0, q2_arm0, q3_arm0);

        }
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


            part0.MoveRotation(Quaternion.RotateTowards(t_arm,T, stepMovement));
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


