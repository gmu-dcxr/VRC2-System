using System;
using System.Collections.Generic;
using UnityEngine;
using VRC2.Animations;
using VRC2.Pipe;

namespace VRC2.Utility
{
    public class PipeStorageManager : MonoBehaviour
    {
        [Header("Storage")] public Transform storageRoot;

        private List<GameObject> pipes;

        private List<MeshCollider> _meshColliders;

        [Header("RobotDog Debug")] public GameObject debugPipe;

        private void Start()
        {
            InitPipes();

            SetAsRoot();

            InitMeshColliders();

            DisableRigidBody();
            // disable
            // DisableMeshColliders();
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

        void DisableRigidBody()
        {
            foreach (var pipe in pipes)
            {
                var p = pipe;
                PipeHelper.BeforeMove(ref p);
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

        #region Debug

        void DebugRobotDog()
        {
            var go = GameObject.Find(GlobalConstants.BendCutRobot);
            var rdc = go.GetComponent<RobotDogController>();
            // update current pipe
            rdc.currentPipe = debugPipe;

            rdc.InitParameters(PipeConstants.PipeBendAngles.Angle_0, 1.0f, 1.0f, 2,
                PipeConstants.PipeDiameter.Diameter_1, 3);
            rdc.Execute();
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("RobotDog"))
            {
                DebugRobotDog();
            }

            GUILayout.EndVertical();
        }

        #endregion
    }
}