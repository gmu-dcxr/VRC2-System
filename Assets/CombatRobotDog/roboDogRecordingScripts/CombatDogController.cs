using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDogController : MonoBehaviour
{

    //Animator
    public Animator anim;
    //Movement
    public float moveSpeed = 1f;
    public float rotateSpeed = 20f;
    //Scripts
    public Actions act;
    //bools
    private bool walk;
    private bool turn;

    // Start is called before the first frame update
    void Start()
    {
        walk = false;
        turn = false;
    }

    // Update is called once per frame
    void Update()
    {
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //walking foward
        if (Input.GetKey(KeyCode.W))
        {
            if (walk == false)
            {
                act.Walk();
                walk = true;
            }
            //transform.Translate(dir * moveSpeed * Time.deltaTime);
            transform.Translate(dir * moveSpeed * Time.deltaTime);

        }
        // turn left
        if (Input.GetKey(KeyCode.Q))
        {
            if (turn == false)
            {
                act.TurnLeft();
                turn = true;
            }
            transform.Rotate(-1 * Vector3.up * Time.deltaTime * rotateSpeed, Space.Self);
        }
        // turn right
        if (Input.GetKey(KeyCode.E))
        {
            if (turn == false)
            {
                act.TurnRight();
                turn = true;
            }
            transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed, Space.Self);
        }
        // move backwards
        if (Input.GetKey(KeyCode.S))
        {
            if (walk == false)
            {
                act.Walk();
                walk = true;
            }
            transform.Translate(dir * moveSpeed * Time.deltaTime);
        }
        // Strafe left
        if (Input.GetKey(KeyCode.A))
        {
            if (walk == false)
            {
                act.StrafeLeft();
                walk = true;
            }
            transform.Translate(dir * moveSpeed * Time.deltaTime);
        }// Strafe right
        if (Input.GetKey(KeyCode.D))
        {
            if (walk == false)
            {
                act.StrafeRight();
                walk = true;
            }
            transform.Translate(dir * moveSpeed * Time.deltaTime);
        }

        //No button, go idle
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)
            || Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        {
            walk = false;
            turn = false;
            act.Idle1();
        }

        //Use to look like picking up 
        if (Input.GetKey(KeyCode.R)) 
        {
            act.Hit1();
        } 

    }
}
