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

        internal enum ExcavatorStage
        {
            Stop = 0,
            Forward = 1,
            Backward = 2,
            Dig = 3,
            Rotate = 4,
            RRotate = 5,
        }

        internal enum part
        {
            nextTo = 0,
            into1 = 1,
            into2 = 2,
            
        }

        [Space(30)]  [Header("Recording/Replay")]
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



        [Header("Rock")] public GameObject rock1;
        public GameObject rock2;
        public GameObject rock3;

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

        public Vector3 scaleChange = new Vector3(1.0f, 1.0f, 1.0f);




        private void Start()
        {
            //replay.Start();
            base.Start();
            pt = part.nextTo;
            _stage = ExcavatorStage.Stop;
            // destinationPos = destination.transform.position;
        }

        private void Update()
        {
            var angle = Math.Abs(recording.getRotation() - endAngle);
            //print(angle);
            
            switch (_stage)
            {
                case ExcavatorStage.Stop:
                    //replay.Stop();
                    //print("STOP STOP STOP");
                    //dirt.transform.localScale -= scaleChange;
                    dirt.SetActive(false);
                    if (ReachDestination(digPoint.position) )
                    {
                        if (TurnCheck(rotatePoint))
                        {
                            isDigging = false;
                            // time to dig
                            _stage = ExcavatorStage.Dig;
                        } else
                        {
                            if (replay.DigFinished())
                            {
                                if (TurnCheck(ogPoint))
                                {
                                    break;
                                }
                            }
                            _stage = ExcavatorStage.Rotate;
                            //rotate to correct angle

                        }
                    } else
                    {
                        //print("NOT NOT");
                    }
                    break;

                case ExcavatorStage.Rotate:
                    print("rotaterotate");
                    replay.Turn(true);
                    print("***check turn: ");
                    if (TurnCheck(rotatePoint))
                    {
                        print("time to dig!");
                        _stage = ExcavatorStage.Dig;
                    }
                    break;

                case ExcavatorStage.RRotate:
                    //play replay
                    replay.TurnR(true);
                    if (digDone)
                    {
                        print("Checking turn digdone: ");
                        if (TurnCheck(ogPoint))
                        {
                            print("dig is done and its time to go back");
                            _stage = ExcavatorStage.Backward;
                        }
                    }
                    break;

                    
                case ExcavatorStage.Forward:
                    print("FORWARD FORWARD FORWARD");
                    replay.Forward(true);
                    if(pt == part.into1)
                    {
                        print("into1!!!!!!!!!!!");
                        if (ReachDestination(deep.position))
                        {
                            _stage = ExcavatorStage.Dig;
                        } else
                        {
                            break;
                        }
                    }
                    if(pt == part.into2)
                    {
                        if (ReachDestination(deeper.position))
                        {
                            _stage = ExcavatorStage.Dig;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (ReachDestination(digPoint.position) && pt == part.nextTo)
                    {
                        _stage = ExcavatorStage.Stop;
                    } else
                    {
                        //print("STILL FORWARD");
                    }

                    break;
                case ExcavatorStage.Backward:
                    print("BACK BACK BACK");
                    dirt.SetActive(false);
                    replay.Backward(true);
                    if (ReachDestination(startPoint.position))
                    {
                        _stage = ExcavatorStage.Stop;
                    }

                    break;

                case ExcavatorStage.Dig:
                    //dirt.transform.localScale += scaleChange;
                    dirt.SetActive(true);
                    
                    if (!isDigging)
                    {
                        print("diggin");
                        isDigging = true;
                        replay.Dig();
                    }
                    else
                    {
                        if (replay.DigFinished() && !digDone)
                        {
                            if(pt == part.nextTo)
                            {
                                pt = part.into1;
                                _stage = ExcavatorStage.Forward;
                                isDigging = false;
                                break;
                            }
                            if (pt == part.into1)
                            {
                                pt = part.into2;
                                _stage = ExcavatorStage.Forward;
                                isDigging = false;
                                break;
                            }
                            digDone = true;
                            _stage = ExcavatorStage.RRotate; 
                        }
                    }
                    break;
            }
            }
        

        void UpdateTransforms(Transform bc, GameObject rk, GameObject ds)
        {
            // backHoe.SetActive(false);
            excav.transform.position = bc.position;
            excav.transform.rotation = bc.rotation;

            // backHoe.SetActive(true);

            rk.SetActive(true);
            ds.SetActive(true);
        }

        void EnableSetting(int idx)
        {
        }

        IEnumerator ExcavatorDig()
        {
            Start();
            return null;
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
            if (digDone)
            {
                if(dotProd < 0.20)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            
            
                if (dotProd > 0.30)
                {
                // ObjA is looking mostly towards ObjB
                print("real");
                    return true;
                }
                //print(dotProd);
                return false;
            
            
        } 


        void StopVehicle()
        {
        }

        void StartVehicle()
        {
        }

        void MoveBackward()
        {

            if (ReachDestination(startPoint.position))
            {
                print("Truck reach destination");
                moving = false;
                return;
            }
        }

        #endregion

        #region Accident Events Callbacks

        // normal event
        public override void StartNormalIncident()
        {
            print("Start Normal Incident Baseline S7");

            StopAllCoroutines();

            EnableSetting(0);

            //StartCoroutine(ExcavatorDig());
            _stage = ExcavatorStage.Forward;
            replay.Forward();
        }

        public void On_BaselineS7_1_Start()
        {
            moving = true;
            _stage = ExcavatorStage.Forward;
            replay.Forward();
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

            StopAllCoroutines();

            EnableSetting(1);

            StartCoroutine(ExcavatorDig());
        }

        public void On_BaselineS7_2_Finish()
        {
            // An excavator is digging next to the participants.
            StopAllCoroutines();
        }

        public void On_BaselineS7_3_Start()
        {
            print("On_BaselineS7_3_Start");
            // The excavator is digging into the working zone.
            // get incident
            var incident = GetIncident(3);
            var warning = incident.Warning;
            print(warning);

            StopAllCoroutines();
            EnableSetting(2);

            StartCoroutine(ExcavatorDig());
        }

        public void On_BaselineS7_3_Finish()
        {
            // The excavator is digging into the working zone.
            StopAllCoroutines();
        }

        public void On_BaselineS7_4_Start()
        {
            print("On_BaselineS7_4_Start");
            // The excavator is digging more into the working zone.
            // get incident
            var incident = GetIncident(4);
            var warning = incident.Warning;
            print(warning);

            StopAllCoroutines();

            EnableSetting(3);

            StartCoroutine(ExcavatorDig());
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