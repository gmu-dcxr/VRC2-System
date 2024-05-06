using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRC2.Utility
{
    public class PipeStorageManager : MonoBehaviour
    {
        public Transform storageRoot;

        private List<GameObject> pipes;

        private List<MeshCollider> _meshColliders;

        private void Start()
        {
            InitPipes();
            
            SetAsRoot();
            
            InitMeshColliders();
            
            // disable
            DisableMeshColliders();
        }

        #region Pipe management

        void InitPipes()
        {
            pipes = new List<GameObject>();
            var count = storageRoot.transform.childCount;
            for (var i = 0; i < count; i++)
            {
                pipes.Add(storageRoot.GetChild(i).gameObject);
            }
        }

        void InitMeshColliders()
        {
            _meshColliders = new List<MeshCollider>();
            foreach (var pipe in pipes)
            {
                var mcs = pipe.GetComponentsInChildren<MeshCollider>();
                _meshColliders.AddRange(mcs);
            }
        }

        // set as root to prepare
        public void SetAsRoot()
        {
            var idx = storageRoot.GetSiblingIndex();

            foreach (var pipe in pipes)
            {
                idx += 1;
                pipe.transform.parent = null;
                // set idx
                pipe.transform.SetSiblingIndex(idx);
            }
        }

        public void ResetStorage()
        {
            foreach (var pipe in pipes)
            {
                pipe.transform.parent = storageRoot;
            }
        }
        
        // remove mesh colliders to let robot dog pickup

        void SetMeshColliders(bool enable)
        {
            foreach (var mc in _meshColliders)
            {
                mc.enabled = enable;
            }
        }

        public void EnableMeshColliders()
        {
            SetMeshColliders(true);
        }

        public void DisableMeshColliders()
        {
            SetMeshColliders(false);
        }

        #endregion
    }
}