using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScrew : MonoBehaviour
{
    // Start is called before the first frame update

    bool firstCol = true;

    void Start()
    {
        firstCol = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);

        if (collision.gameObject.tag=="cube" && firstCol)
        {
            GameObject.Find("RObotic_arm_screw_auto").GetComponent<RoboticArm_Screw_IK>().startScrew();

            
            firstCol = false;
        }
    }
}
