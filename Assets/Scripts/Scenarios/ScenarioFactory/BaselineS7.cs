using Oculus.Platform.Models;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using WSMGameStudio.HeavyMachinery;
using VRC2.Animations;
using WSMGameStudio.Vehicles;
using Random = UnityEngine.Random;
using Timer = UnityTimer.Timer;


namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS7 : Scenario
    {
        public GameObject excav;
        public GameObject dirt;
        public Transform endPiece;
        public GameObject hole;
        public Transform spawn;

        internal enum ExcavatorStage
        {
            Stop = 0,
            Forward = 1,
            Backward = 2,
            Dig = 3,
            Rotate = 4,
            RRotate = 5,
            Dump = 6,
        }

        internal enum part
        {
            nextTo = 0,
            into1 = 1,
            into2 = 2,

        }

        [Space(30)] [Header("Recording/Replay")]
        public ExcavatorInputRecording recording;


        public ExcavatorInputReplay replay;

        public Transform startPoint;
        public Transform digPoint;
        public Transform rotatePoint;
        public Transform ogPoint;

        public Transform deep;
        public Transform deeper;
        public Transform deepR;
        public Transform deeperR;

        // public GameObject excavator;

        // public GameObject destination;

        private Vector3 destinationPos;

        private bool extendBoom = false;
        private bool extendArm = false;
        private bool extendRearBucket = false;

        private bool retractBoom = false;
        private bool retractArm = false;
        private bool retractRearBucket = false;

        private bool rotateBoom = false;
        private bool centerBoom = false;

        private bool lowerStabilizers = false;

        private bool moving = false;

        private bool isDigging = false;

        private float distanceThreshold = 2.0f;

        private GameObject activeSetting;

        private ExcavatorStage _stage;

        private part pt;

        public float endAngle = 120f;

        private bool digDone = false;

        private Vector3 scaleChange = new Vector3(0.05f, 0.0f, 0.05f);

        private bool dump = false;

        private bool moved = false;

        private void Update()
        {
            //var angle = Math.Abs(recording.getRotation() - endAngle);

            switch (_stage)
            {
                case ExcavatorStage.Stop:
                    dirt.SetActive(false);
                    if (ReachDestination(digPoint.position))
                    {
                        if (TurnCheck(rotatePoint))
                        {
                            isDigging = false;
                            // time to dig
                            _stage = ExcavatorStage.Dig;
                        }
                        else
                        {
                            if (replay.DigFinished())
                            {
                                if (TurnCheck(ogPoint))
                                {
                                    break;
                                }
                            }

                            _stage = ExcavatorStage.Rotate;
                        }
                    }

                    break;

                case ExcavatorStage.Rotate:
                    //print("rotaterotate");
                    replay.Turn(true);
                    //print("***check turn: ");
                    if (TurnCheck(rotatePoint))
                    {
                        //print("time to dig!");
                        _stage = ExcavatorStage.Dig;
                    }

                    break;

                case ExcavatorStage.Dump:
                    //print("DUMPERRR");
                    dirtCheck();
                    moved = false;
                    replay.Dump(true);
                    if (replay.DumpDone())
                    {
                        if (pt == part.nextTo)
                        {
                            print("dump1 done");
                            pt = part.into1;
                            _stage = ExcavatorStage.Forward;
                            dump = false;
                            isDigging = false;
                            break;
                        }

                        if (pt == part.into1)
                        {
                            print("dump2 done");
                            pt = part.into2;
                            _stage = ExcavatorStage.Forward;
                            dump = false;
                            isDigging = false;
                            break;
                        }

                        print("dump3 done");
                        digDone = true;
                        _stage = ExcavatorStage.Backward;
                    }

                    break;

                case ExcavatorStage.RRotate:
                    //play replay
                    replay.TurnR(true);
                    //print("Checking turn digdone: ");
                    if (TurnCheck(ogPoint))
                    {
                        if (digDone)
                        {
                            //print("dig is done and its time to go back");
                            _stage = ExcavatorStage.Backward;
                        }
                        else
                        {
                            _stage = ExcavatorStage.Dump;
                            //dump time
                        }
                    }

                    break;

                case ExcavatorStage.Forward:
                    //print("FORWARD FORWARD FORWARD");
                    replay.Forward(true);
                    if (pt == part.into1)
                    {
                        //print("into1!!!!!!!!!!!");
                        if (ReachDestination(deep.position))
                        {
                            _stage = ExcavatorStage.Rotate;
                            //_stage = ExcavatorStage.Dig;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (pt == part.into2)
                    {
                        if (ReachDestination(deeper.position))
                        {
                            //_stage = ExcavatorStage.Dig;
                            _stage = ExcavatorStage.Rotate;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (ReachDestination(digPoint.position) && pt == part.nextTo)
                    {
                        print("forward end");
                        _stage = ExcavatorStage.Stop;
                    }

                    break;

                case ExcavatorStage.Backward:
                    //print("BACK BACK BACK");
                    //dirt.SetActive(false);
                    //replay.Backward(true);
                    if (ReachDestination(startPoint.position))
                    {
                        _stage = ExcavatorStage.Stop;
                    }

                    break;

                case ExcavatorStage.Dig:
                    //dirt.SetActive(true);
                    dirtCheck();
                    if (!isDigging)
                    {
                        //print("diggin");
                        isDigging = true;
                        replay.Dig();

                    }
                    else
                    {
                        if (replay.DigFinished() && !digDone)
                        {
                            //done digging- rotate to dump
                            dump = true;
                            //make hole bigger
                            //UpdateHole(hole);
                            _stage = ExcavatorStage.RRotate;

                        }
                    }

                    break;
            }
        }

        void UpdateHole(GameObject h)
        {
            h.transform.localScale -= scaleChange;
            h.transform.position = new Vector3(h.transform.position.x - 0.2f, h.transform.position.y - 0.2f,
                h.transform.position.z - 0.2f);

        }

        #region Driving Control

        bool ReachDestination(Vector3 des)
        {
            var t = excav.transform.position;

            // use the same y
            des.y = 0;
            t.y = 0;
            var distance = Vector3.Distance(t, des);

            if (distance < distanceThreshold)
            {
                return true;
            }

            return false;
        }

        bool TurnCheck(Transform des)
        {

            Vector3 dirFromAtoB = (endPiece.position - des.position).normalized;
            float dotProd = Vector3.Dot(dirFromAtoB, endPiece.forward);

            print(dotProd);
            if (digDone || dump)
            {
                //straight ahead angle
                if (dotProd < 0.20)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }


            if (dotProd > 0.30)
            {
                //dig spot
                // ObjA is looking mostly towards ObjB
                print("real");
                return true;
            }

            //print(dotProd);
            return false;


        }

        void dirtCheck()
        {
            //if piece over x high - drop
            if (endPiece.transform.position.y > 7.5f)
            {
                dirt.transform.SetParent(null);
                dirt.GetComponent<Rigidbody>().useGravity = true;
                dirt.GetComponent<MeshCollider>().convex = true;
                //dirt.GetComponent<Rigidbody>().isKinematic = true;

                return;
            }

            //if under x - spawn in
            if (pt == part.into1 || pt == part.into2)
            {
                if (endPiece.transform.position.y < 0.80)
                {
                    dirt.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y,
                        spawn.transform.position.z);
                    dirt.transform.rotation = Quaternion.Euler(spawn.transform.rotation.x, spawn.transform.rotation.y,
                        spawn.transform.rotation.z);
                    //dirt.transform.rotation = Quaternion.Euler(75.0f, -100.0f, 175.0f);
                    dirt.transform.SetParent(endPiece);
                    dirt.GetComponent<Rigidbody>().useGravity = false;
                    dirt.GetComponent<Rigidbody>().isKinematic = false;
                    dirt.GetComponent<MeshCollider>().convex = false;
                    dirt.GetComponent<MeshCollider>().convex = false;
                    //make hole bigger
                    if (!moved)
                    {
                        UpdateHole(hole);
                        moved = true;
                    }

                    dirt.SetActive(true);
                    return;
                }
            }
            else
            {
                if (endPiece.transform.position.y < -0.90f)
                {


                    dirt.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y,
                        spawn.transform.position.z);
                    dirt.transform.rotation = Quaternion.Euler(spawn.transform.rotation.x, spawn.transform.rotation.y,
                        spawn.transform.rotation.z);
                    //dirt.transform.rotation = Quaternion.Euler(-254.0f, 85.0f, 2.0f);
                    dirt.transform.SetParent(endPiece);
                    dirt.GetComponent<Rigidbody>().useGravity = false;
                    dirt.GetComponent<Rigidbody>().isKinematic = false;
                    dirt.GetComponent<MeshCollider>().convex = false;
                    dirt.GetComponent<MeshCollider>().convex = false;
                    //make hole bigger
                    if (!moved)
                    {
                        UpdateHole(hole);
                        moved = true;
                    }

                    dirt.SetActive(true);
                    return;
                }
            }

        }

        #endregion

        #region Accident Events Callbacks

        // normal event
        public override void StartNormalIncident()
        {
            print("Start Normal Incident Baseline S7");

            dirt.transform.SetParent(endPiece);
            dirt.GetComponent<Rigidbody>().useGravity = false;
            dirt.GetComponent<Rigidbody>().isKinematic = false;
            dirt.GetComponent<MeshCollider>().convex = false;
            _stage = ExcavatorStage.Forward;
            replay.Forward();
        }

        public void On_BaselineS7_1_Start()
        {
            // moving = true;
            // _stage = ExcavatorStage.Forward;
            // replay.Forward();
        }

        public void On_BaselineS7_1_Finish()
        {

        }

        public void On_BaselineS7_2_Start()
        {
            print("On_BaselineS7_2_Start");
            // An excavator is digging next to the participants.
            // get incident
            var incident = GetIncident(2);
            var warning = incident.Warning;
            print(warning);
            // TODO 
        }

        public void On_BaselineS7_2_Finish()
        {
            // An excavator is digging next to the participants.
        }

        public void On_BaselineS7_3_Start()
        {
            print("On_BaselineS7_3_Start");
            // The excavator is digging into the working zone.
            // get incident
            var incident = GetIncident(3);
            var warning = incident.Warning;
            print(warning);
            // TODO
        }

        public void On_BaselineS7_3_Finish()
        {
            // The excavator is digging into the working zone.
        }

        public void On_BaselineS7_4_Start()
        {
            print("On_BaselineS7_4_Start");
            // The excavator is digging more into the working zone.
            // get incident
            var incident = GetIncident(4);
            var warning = incident.Warning;
            print(warning);
            // TODO
        }

        public void On_BaselineS7_4_Finish()
        {
            // The excavator is digging more into the working zone.
        }


        public void On_BaselineS7_5_Start()
        {
            print("On_BaselineS7_5_Start");
            // SAGAT query
            ShowSAGAT();
        }

        public void On_BaselineS7_5_Finish()
        {
            // SAGAT query
            HideSAGAT();
        }

        #endregion

    }
}