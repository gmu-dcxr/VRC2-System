using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC2;

public class WallCollisionDetector : MonoBehaviour
{
    // pipe's axes are different from wall's 
    [Header("Pipe")] public float yRotationOffset = -90f;

    // for better visualization, it can be updated accordingly.
    public float distanceOffset = 0.18f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // get the game object
        var go = other.gameObject;
        if (go.CompareTag(GlobalConstants.pipeObjectTag))
        {
            HandlePipeCollision(go);
        }
    }

    private void OnTriggerExit(Collider other)
    {
    }

    private void OnTriggerStay(Collider other)
    {
        var go = other.gameObject;
        if (go.CompareTag(GlobalConstants.pipeObjectTag))
        {
            HandlePipeCollision(go);
        }
    }

    #region Handle Pipe's Collision with the Wall

    void HandlePipeCollision(GameObject pipe)
    {
        // get the Interactable pipe
        var ipipe = pipe.transform.parent.gameObject;
        Debug.Log(ipipe.name);

        var t = ipipe.transform;
        var pos = t.position;
        var rot = t.rotation.eulerAngles;

        // get the wall transform
        var wt = gameObject.transform;
        var wpos = wt.position;
        var wrot = wt.rotation.eulerAngles;

        // set pipe's x rotation to the wall's x rotation
        rot.x = wrot.x;
        // set pipe's y rotation to the wall's y rotation
        rot.y = wrot.y + yRotationOffset;
        // update
        ipipe.transform.rotation = Quaternion.Euler(rot);

        // update the pipe's distance to the wall
        pos.x = wpos.x + distanceOffset;

        ipipe.transform.position = pos;
    }




    #endregion
}
