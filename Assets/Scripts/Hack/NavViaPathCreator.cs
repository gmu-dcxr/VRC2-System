using System;
using System.Collections.Generic;
using PathCreation;
using PathCreation.Utility;
using UnityEngine;
using UnityEngine.AI;
using VRC2.Agent;

namespace VRC2.Hack
{
    public class NavViaPathCreator : MonoBehaviour
    {

        [Header("Model")] public GameObject model;
        // public bool forceZRotation = true;
        // public bool forceRelRotation = true;


        [Header("NavAgent")] public float speed = 1f;
        public float stoppingDistance = 0.5f;
        public float radius = 0.5f;

        private NavMeshAgent _agent;
        private PathCreator pathCreator;

        private BezierPath path
        {
            get => pathCreator.bezierPath;
        }

        private GameObject realModel;

        private List<Vector3> anchorPoints;

        private int dstIndex = 0;

        private void Start()
        {
            pathCreator = GetComponent<PathCreator>();

            InitAnchorPoints();

            _agent = model.GetComponent<NavMeshAgent>();
            if (_agent == null)
            {
                _agent = model.AddComponent<NavMeshAgent>();
            }

            // update speed
            _agent.speed = speed;

            // update stopping distance
            _agent.stoppingDistance = stoppingDistance;

            // update radius
            _agent.radius = radius;


            // set the 1st destination
            _agent.SetDestination(anchorPoints[dstIndex]);
        }

        void InitAnchorPoints()
        {
            anchorPoints = new List<Vector3>();
            for (int i = 0; i < path.NumPoints; i += 3)
            {
                var p = path.GetPoint(i);

                // transform it
                p = MathUtility.TransformPoint(path.GetPoint(i), pathCreator.transform, path.Space);
                anchorPoints.Add(p);
            }
        }

        private void Update()
        {
            if (AgentHelper.ReachDestination(_agent))
            {
                // change to next destination
                dstIndex = (dstIndex + 1) % anchorPoints.Count;
                _agent.SetDestination(anchorPoints[dstIndex]);
                print("change destination");
            }

            // if (forceZRotation)
            // {
            //     var rot = model.transform.rotation.eulerAngles;
            //     rot.z = 0;
            //     model.transform.rotation = Quaternion.Euler(rot);
            // }

            // if (forceRelRotation)
            // {
            //     var rot = realModel.transform.localRotation.eulerAngles;
            //     rot.z = 0;
            //     realModel.transform.localRotation = Quaternion.Euler(rot);
            // }
        }
    }
}