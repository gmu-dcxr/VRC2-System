using System;
using Oculus.Interaction;
using UnityEngine;
using VRC2.Hack;

namespace VRC2.Events
{
    [RequireComponent(typeof(PointableUnityEventWrapper))]
    public class GlueGrabbingCallback : MonoBehaviour
    {
        public GameObject horizontalGameObject;
        public GameObject verticalGameObject;


        private PointableUnityEventWrapper _wrapper;
        
        private DistanceLimitedAutoMoveTowardsTargetProvider _provider;

        [HideInInspector]
        public DistanceLimitedAutoMoveTowardsTargetProvider provider
        {
            get
            {
                if (_provider == null)
                {
                    _provider = gameObject.GetComponentInChildren<DistanceLimitedAutoMoveTowardsTargetProvider>();
                }

                return _provider;
            }
        }

        void Start()
        {
            _wrapper = gameObject.GetComponent<PointableUnityEventWrapper>();

            _wrapper.WhenSelect.AddListener(OnSelect);
            _wrapper.WhenRelease.AddListener(OnRelease);
        }

        void OnSelect()
        {
            if(provider == null || !provider.IsValid) return;
            
            // enable horizontal object and disable vertical object to make it look real
            horizontalGameObject.SetActive(true);
            verticalGameObject.SetActive(false);
        }

        public void OnRelease()
        {
            if(provider == null || !provider.IsValid) return;
            
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