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
            Wait = 7,
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

        [Space(30)]  [Header("Locations + Angles")]
        public Transform startPoint;
        public Transform digPoint1;
        public Transform digAngle;
        public Transform straightAngle;
        public Transform digPoint2;
        public Transform digPoint3;
        
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
        private Vector3 initScale = new Vector3(0.05f, 0.05f, 0.05f);

        private bool dump = false;

        private bool moved = false;

        private bool dirtSpawned = false;

        private void Update()
        {
            switch (_stage)
            {
                case ExcavatorStage.Stop:
                    dirt.SetActive(false);
                    if (ReachDestination(digPoint1.position))
                    {
                        if (TurnCheck(digAngle))
                        {
                            isDigging = false;
                            // time to dig
                            _stage = ExcavatorStage.Dig;
                        }
                        else
                        {
                            if (replay.DigFinished())
                            {
                                if (TurnCheck(straightAngle))
                                {
                                    break;
                                }
                            }
                            _stage = ExcavatorStage.Rotate;
                        }
                    }
                    break;

                case ExcavatorStage.Rotate:
                    replay.Turn(true);
                    if (TurnCheck(digAngle))
                    {
                        _stage = ExcavatorStage.Dig;
                    }
                    break;

                case ExcavatorStage.Wait:
                    if(pt == part.into1)
                    {
                        _stage = ExcavatorStage.Forward;
                        dump = false;
                        isDigging = false;
                        break;
                    }
                    if(pt == part.into2)
                    {
                        _stage = ExcavatorStage.Forward;
                        dump = false;
                        isDigging = false;
                    }
                    break;

                case ExcavatorStage.Dump:
                    dirtCheck();
                    moved = false;
                    replay.Dump(true);
                    if (replay.DumpDone())
                    {
                        if (pt == part.nextTo)
                        {
                            print("dump1 done");
                            //end of part 2
                            _stage = ExcavatorStage.Wait;
                            break;
                        }

                        if (pt == part.into1)
                        {
                            print("dump2 done");
                            //end of part 3
                            pt = part.nextTo;
                            _stage = ExcavatorStage.Wait;
                            break;
                        }

                        print("dump3 done");
                        //end of part 4
                        digDone = true;
                        _stage = ExcavatorStage.Backward;
                    }
                    break;

                case ExcavatorStage.RRotate:
                    //play replay
                    replay.TurnR(true);
                    if (TurnCheck(straightAngle))
                    {
                        if (digDone)
                        {
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
                    replay.Forward(true);
                    if (pt == part.into1)
                    {
                        if (ReachDestination(digPoint2.position))
                        {
                            _stage = ExcavatorStage.Rotate;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (pt == part.into2)
                    {
                        if (ReachDestination(digPoint3.position))
                        {
                            _stage = ExcavatorStage.Rotate;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (ReachDestination(digPoint1.position) && pt == part.nextTo)
                    {
                        _stage = ExcavatorStage.Stop;
                    }
                    break;

                case ExcavatorStage.Backward:
                    if (ReachDestination(startPoint.position))
                    {
                        _stage = ExcavatorStage.Stop;
                    }
                    break;

                case ExcavatorStage.Dig:
                    dirtCheck();
                    if (!isDigging)
                    {
                        isDigging = true;
                        replay.Dig();
                    }
                    else
                    {
                        if (replay.DigFinished() && !digDone)
                        {
                            //done digging- rotate to dump
                            dump = true;
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
                if (!dirtSpawned)
                {
                    GameObject dupe = Instantiate(dirt);
                    dirtSpawned = true;

                    dupe.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y, spawn.transform.position.z);
                    dupe.transform.SetParent(endPiece);
                    dupe.transform.rotation = Quaternion.Euler(spawn.transform.rotation.x + (Random.Range(0.0f, 40.0f)), spawn.transform.rotation.y + (Random.Range(0.0f, 40.0f)), spawn.transform.rotation.z + (Random.Range(0.0f, 40.0f)));
                    dupe.GetComponent<Rigidbody>().useGravity = false;
                    dupe.GetComponent<Rigidbody>().isKinematic = false;
                    dupe.GetComponent<MeshCollider>().convex = false;
                    dupe.transform.localScale = initScale;


                    dupe.transform.SetParent(null);
                    dupe.GetComponent<Rigidbody>().useGravity = true;
                    dupe.GetComponent<MeshCollider>().convex = true;
                    dirt.SetActive(false);

                    dirt.transform.SetParent(null);
                    dirt.GetComponent<Rigidbody>().useGravity = true;
                    dirt.GetComponent<MeshCollider>().convex = true;
                }
                return;
            }

            //if under x - spawn in
            if (pt == part.into1 || pt == part.into2)
            {
                if (endPiece.transform.position.y < 0.75)
                {
                    dirtSpawned = false;
                    dirt.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y, spawn.transform.position.z);
                    dirt.transform.rotation = Quaternion.Euler(spawn.transform.rotation.x, spawn.transform.rotation.y, spawn.transform.rotation.z);

                    dirt.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y,
                        spawn.transform.position.z);
                    dirt.transform.rotation = Quaternion.Euler(spawn.transform.rotation.x, spawn.transform.rotation.y,
                        spawn.transform.rotation.z);
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
                if (endPiece.transform.position.y < -1.50f)
                {
                    dirtSpawned = false;
                    dirt.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y, spawn.transform.position.z);
                    dirt.transform.rotation = Quaternion.Euler(spawn.transform.rotation.x, spawn.transform.rotation.y, spawn.transform.rotation.z);
                    dirt.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y,
                        spawn.transform.position.z);
                    dirt.transform.rotation = Quaternion.Euler(spawn.transform.rotation.x, spawn.transform.rotation.y,
                        spawn.transform.rotation.z);
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

            dirt.transform.SetParent(endPiece);
            dirt.GetComponent<Rigidbody>().useGravity = false;
            dirt.GetComponent<Rigidbody>().isKinematic = false;
            dirt.GetComponent<MeshCollider>().convex = false;
            _stage = ExcavatorStage.Forward;
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
            pt = part.into1;
            _stage = ExcavatorStage.Wait;
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
            pt = part.into2;
            _stage = ExcavatorStage.Wait;
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