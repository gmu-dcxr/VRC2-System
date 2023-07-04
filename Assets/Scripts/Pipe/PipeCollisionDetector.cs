using System;
using Unity.VisualScripting;
using UnityEngine;

namespace VRC2.Events
{
    public class PipeCollisionDetector: MonoBehaviour
    {
        [HideInInspector]
        public GameObject connecting;

        private void Start()
        {
        }

        private void Update()
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
            var go = other.gameObject;
            if (go.CompareTag(GlobalConstants.pipeObjectTag))
            {
                HandlePipeCollision(go);
            }
        }

        void HandlePipeCollision(GameObject otherpipe)
        {
            // update connecting
            connecting = otherpipe.transform.parent.gameObject; // Interactable pipe

            var opt = otherpipe.transform;
            
            // current pipe
            var cpt = gameObject.transform;

            // update rotation
            connecting.transform.rotation = gameObject.transform.parent.rotation;
            
            // // update position
            // // var position = ctp.transform.position;
            // // add right vector
            // var cmax = gameObject.GetComponent<Renderer>().bounds.max;
            // var omin = otherpipe.GetComponent<Renderer>().bounds.min;
            //
            // Debug.Log($"cmax: {cmax.ToString("f5")}");
            // Debug.Log($"omin: {omin.ToString("f5")}");
            //
            // // var position = cpt.transform.position;
            //
            // var offset = omin - cmax;
            // connecting.transform.position = cpt.transform.position + offset;
            
            // calculate offset

            // position = position + ctp.transform.right * (cmax);

            // connecting.transform.position = position;
        }
    }
}