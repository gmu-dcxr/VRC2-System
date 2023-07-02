using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VRC2;

namespace VRC2
{

    public class WallCollisionDetector : MonoBehaviour
    {
        // pipe's axes are different from wall's 
        [Header("Pipe")] public float pipeYRotationOffset = -90f;

        // for better visualization, it can be updated accordingly.
        public float pipeDistanceOffset = 0.18f;

        [Header("Clamp")] public float clampYRotationOffset = 0;
        public float clampZRotationOffset = -90; // fixed
        public float clampDistanceOffset = 0.15f;

        [Header("Box")]
        public float boxYRotationOffset = -90f;

        public float boxDistanceOffset = 0.25f;

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
            OnTriggerEnterAndStay(other);
        }

        private void OnTriggerExit(Collider other)
        {
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerEnterAndStay(other);
        }

        void OnTriggerEnterAndStay(Collider other)
        {
            // get the game object
            var go = other.gameObject;
            if (go.CompareTag(GlobalConstants.pipeObjectTag))
            {
                HandlePipeCollision(go);
            }
            else if (go.CompareTag(GlobalConstants.clampObjectTag))
            {
                HandleClampCollision(go);
            }
            else if (go.CompareTag(GlobalConstants.boxObjectTag))
            {
                HandleBoxCollision(go);
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
            rot.y = wrot.y + pipeYRotationOffset;
            // update
            ipipe.transform.rotation = Quaternion.Euler(rot);

            // update the pipe's distance to the wall
            pos.x = wpos.x + pipeDistanceOffset;

            ipipe.transform.position = pos;
        }



        #endregion

        #region Handle Clamp's Collision with the Wall

        void HandleClampCollision(GameObject clamp)
        {
            // get the Interactable clamp
            var iclamp = clamp.transform.parent.gameObject;
            Debug.Log(iclamp.name);

            var t = iclamp.transform;
            var pos = t.position;
            var rot = t.rotation.eulerAngles;

            // get the wall transform
            var wt = gameObject.transform;
            var wpos = wt.position;
            var wrot = wt.rotation.eulerAngles;

            // clamp has the same x rotation with the wall
            // rot.x = wrot.x;
            rot.y = wrot.y + clampYRotationOffset;
            rot.z = wrot.z + clampZRotationOffset;

            // update rotation
            iclamp.transform.rotation = Quaternion.Euler(rot);
            // update distance
            pos.x = wpos.x + clampDistanceOffset;

            iclamp.transform.position = pos;
        }



        #endregion

        #region Handle Box's Collision with the Wall

        void HandleBoxCollision(GameObject box)
        {
            // get the Interactable box
            var ibox = box.transform.parent.gameObject;
            Debug.Log(ibox.name);
            
            var t = ibox.transform;
            var pos = t.position;
            var rot = t.rotation.eulerAngles;

            // get the wall transform
            var wt = gameObject.transform;
            var wpos = wt.position;
            var wrot = wt.rotation.eulerAngles;

            // set pipe's x rotation to the wall's x rotation
            rot.x = wrot.x;
            // set pipe's y rotation to the wall's y rotation
            rot.y = wrot.y + boxYRotationOffset;
            // update
            ibox.transform.rotation = Quaternion.Euler(rot);

            // update the pipe's distance to the wall
            pos.x = wpos.x + boxDistanceOffset;

            ibox.transform.position = pos;
        }

        

        #endregion
    }
}