using System;
using Oculus.Interaction;
using UnityEngine;

namespace VRC2.Events
{
    [RequireComponent(typeof(PointableUnityEventWrapper))]
    public class GlueGrabbingCallback : MonoBehaviour
    {
        public GameObject horizontalGameObject;
        public GameObject verticalGameObject;


        private PointableUnityEventWrapper _wrapper;

        void Start()
        {
            _wrapper = gameObject.GetComponent<PointableUnityEventWrapper>();

            _wrapper.WhenSelect.AddListener(OnSelect);
            _wrapper.WhenUnselect.AddListener(OnUnselect);
        }

        void OnSelect()
        {
            // enable horizontal object and disable vertical object to make it look real
            horizontalGameObject.SetActive(true);
            verticalGameObject.SetActive(false);
        }

        public void OnUnselect()
        {
            // restore it
            horizontalGameObject.SetActive(false);
            verticalGameObject.SetActive(true);

            UseGlue();
        }

        void UseGlue()
        {
            if (GlobalConstants.UseGlue())
            {
                // succeed
            }
            else
            {
                // fail because of used out
            }
        }
    }
}