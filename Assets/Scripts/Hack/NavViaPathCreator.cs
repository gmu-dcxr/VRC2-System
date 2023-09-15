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


        [Header("NavAgent")] public float speed = 0.3f;
        public float stoppingDistance = 0.1f;

        private NavMeshAgent _agent;
        private PathCreator pathCreator;

        private BezierPath path
        {
            get => pathCreator.bezierPath;
        }

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

            // set the 1st destination
            _agent.SetDestination(anchorPoints[dstIndex]);
        }

        void InitAnchorPoints()
        {
            anchorPoints = new List<Vector3>();
            for (int i = 0; i < path.NumPoints; i += 3)
            {
                var p = path.GetPoint(i);
                anchorPoints.Add(p);
                
                // var p2 = GetPoint(i % 3);
                // print($"{p.ToString("f5")} {p2.ToString("f5")}");
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
        }

        public Vector3 GetPoint(int index)
        {
            return MathUtility.TransformPoint(anchorPoints[index], transform, path.Space);
        }
    }
}