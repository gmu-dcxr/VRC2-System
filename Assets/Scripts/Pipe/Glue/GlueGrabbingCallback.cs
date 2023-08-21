using System;
using Fusion;
using Oculus.Interaction;
using UnityEngine;
using VRC2.Hack;

namespace VRC2.Events
{
    [RequireComponent(typeof(PointableUnityEventWrapper))]
    public class GlueGrabbingCallback : NetworkBehaviour
    {
        public GameObject horizontalGameObject;
        public GameObject verticalGameObject;

        private Rigidbody _rigidbody;

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
            _rigidbody = gameObject.GetComponent<Rigidbody>();

            _wrapper = gameObject.GetComponent<PointableUnityEventWrapper>();

            _wrapper.WhenSelect.AddListener(OnSelect);
            _wrapper.WhenRelease.AddListener(OnRelease);
        }

        void OnSelect()
        {
            if (provider == null || !provider.IsValid) return;

            // enable horizontal object and disable vertical object to make it look real
            horizontalGameObject.SetActive(true);
            verticalGameObject.SetActive(false);

            if (Runner != null && Runner.IsRunning)
            {
                RPC_SendMessage(true, false);
            }

            _rigidbody.isKinematic = true;
        }

        public void OnRelease()
        {
            if (provider == null || !provider.IsValid) return;

            // restore it
            horizontalGameObject.SetActive(false);
            verticalGameObject.SetActive(true);

            if (Runner != null && Runner.IsRunning)
            {
                RPC_SendMessage(false, true);
            }

            // make it drop
            _rigidbody.isKinematic = false;

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

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SendMessage(bool horizontal, bool vertical, RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
            {
                // 
            }
            else
            {
                print($"Update glue lid: {horizontal} {vertical}");
                horizontalGameObject.SetActive(horizontal);
                verticalGameObject.SetActive(vertical);
            }
        }
    }
}