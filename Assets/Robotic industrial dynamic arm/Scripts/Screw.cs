using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour
{
    //this is the joint used in the package
    public ConfigurableJoint _joint;
    //these angles are used to determine the rotation
    float angle,lastAngle;
    //this is the step of the screw
    public float step = 0.1f;
    //rigidbody attached to the screw
    Rigidbody _thisRB;
    //acumulated angle
    public float acumAngle;
    //anchors and initial anchor of the joint
    public Vector3 anchor, anchor0;
    //new objective to move at
    public Vector3 newPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        acumAngle =0.1f;
        _thisRB = GetComponent<Rigidbody>();

        angle = _thisRB.rotation.eulerAngles[2];
        lastAngle = angle;

       
        _joint.zMotion = ConfigurableJointMotion.Locked;

        StartCoroutine(moveScrew());
    }

    // Update is called once per frame
    IEnumerator moveScrew()
    {
        while (true)
        {
            
            angle = _thisRB.rotation.eulerAngles[2];

            float delta = angle - lastAngle;

            //rigidity of the screw
            _joint.zMotion = ConfigurableJointMotion.Locked;
            _joint.xMotion = ConfigurableJointMotion.Locked;
            _joint.yMotion = ConfigurableJointMotion.Locked;

            //perform screw movement and ulock DOF
            if (Mathf.Abs(delta) < 45 && Mathf.Abs(delta) > 0.5f)
            {
                _joint.zMotion = ConfigurableJointMotion.Free;
                acumAngle -= delta;

                //newPosition = transform.position-transform.TransformDirection(acumAngle * step * transform.forward);
                //_thisRB.MovePosition(newPosition);

                //Debug.Log(newPosition);

                anchor = new Vector3(0, acumAngle * step, 0) + anchor0;
                if (anchor[1]<0)
                {
                    _joint.connectedAnchor = anchor;
                }

            }

            yield return null;

            lastAngle = angle;

        }
   }
    
    
}
