using UnityEngine;
using VRC2.Animations;
using WSMGameStudio.HeavyMachinery;
using WSMGameStudio.Vehicles;
using System;

using static VRC2.Scenarios.ScenarioFactory.BaselineS7;


namespace VRC2.Scenarios.ScenarioFactory
{
    public class Background : Scenario
    {
        [Header("Excavator")] public ExcavatorController excavatorController;

        [Header("Forklift")] public CustomForkLiftController customForkLiftController;
        public Animator anim;

        public GameObject forklift;
        public GameObject good;
        public GameObject good2;


        public Transform destination;

        private bool reachedDes = false;
        private bool reachedStart = false;


        [Header("Hammer")] public HammerController hammerController;

        public GameObject excav;
        public GameObject dirt;
        public Transform endPiece;
        public GameObject hole;
        public Transform spawn;

        public ExcavatorInputRecording recording;


        public ExcavatorInputReplay replay;

        public Transform startPoint;
        public Transform digPoint;
        public Transform rotatePoint;
        public Transform ogPoint;

        public Transform deep;
        public Transform deeper;

        private Vector3 startPos;
        private Quaternion startRotation;

        private Vector3 goodStartPos;
        private Quaternion goodStartRotation;
        private Vector3 goodStartPos2;
        private Quaternion goodStartRotation2;

        private Vector3 destinationPos;

        private bool extendBoom = false;
        private bool extendArm = false;
        private bool extendRearBucket = false;

        private bool retractBoom = false;
        private bool retractArm = false;
        private bool retractRearBucket = false;

        private bool rotateBoom = false;
        private bool centerBoom = false;

        private bool moving = false;

        private bool isDigging = false;

        private float distanceThreshold = 2.0f;

        private ExcavatorStage _stage;

        private part pt;

        public float endAngle = 120f;

        private bool digDone = false;

        private Vector3 scaleChange = new Vector3(0.05f, 0.0f, 0.05f);

        public bool dump = false;

        private bool moved = false;


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



        private void Start()
        {
            base.Start();

            startPos = forklift.transform.position;
            startRotation = forklift.transform.rotation;

            goodStartPos = good.transform.position;
            goodStartRotation = good.transform.rotation;
            goodStartPos2 = good2.transform.position;
            goodStartRotation2 = good2.transform.rotation;

            //Initialize excavator things
            base.Start();
            pt = part.nextTo;
            dirt.transform.SetParent(endPiece);
            dirt.GetComponent<Rigidbody>().useGravity = false;
            dirt.GetComponent<Rigidbody>().isKinematic = false;
            dirt.GetComponent<MeshCollider>().convex = false;
            _stage = ExcavatorStage.Stop;
        }

        private void Update()
        {
            if (ForkliftReachDestination(destinationPos))
            {
                
            }

            if (recording == null) return;
            
            var angle = Math.Abs(recording.getRotation() - endAngle);

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
                    replay.Turn(true);
                    if (TurnCheck(rotatePoint))
                    {
                        _stage = ExcavatorStage.Dig;
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
                            pt = part.into1;
                            _stage = ExcavatorStage.Forward;
                            dump = false;
                            isDigging = false;
                            break;
                        }
                        if (pt == part.into1)
                        {
                            pt = part.into2;
                            _stage = ExcavatorStage.Forward;
                            dump = false;
                            isDigging = false;
                            break;
                        }
                        digDone = true;
                        _stage = ExcavatorStage.Backward;
                    }

                    break;

                case ExcavatorStage.RRotate:
                    //play replay
                    replay.TurnR(true);
                    if (TurnCheck(ogPoint))
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
                        if (ReachDestination(deep.position))
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
                        if (ReachDestination(deeper.position))
                        {
                            _stage = ExcavatorStage.Rotate;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (ReachDestination(digPoint.position) && pt == part.nextTo)
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
            h.transform.position = new Vector3(h.transform.position.x - 0.3f, h.transform.position.y - 0.3f, h.transform.position.z - 0.3f);
        }

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

        bool ForkliftReachDestination(Vector3 des)
        {
            var t = forklift.transform.position;

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

        #region Excav Anim Controls
        void dirtCheck()
        {
            //if piece over x high - drop
            if (endPiece.transform.position.y > 7.5f)
            {
                dirt.transform.SetParent(null);
                dirt.GetComponent<Rigidbody>().useGravity = true;
                dirt.GetComponent<MeshCollider>().convex = true;
                return;
            }

            //if under x - spawn in
            if (pt == part.into1 || pt == part.into2)
            {
                if (endPiece.transform.position.y < 0.50)
                {


                    dirt.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y, spawn.transform.position.z);
                    dirt.transform.rotation = Quaternion.Euler(spawn.transform.rotation.x, spawn.transform.rotation.y, spawn.transform.rotation.z);
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


                    dirt.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y, spawn.transform.position.z);
                    dirt.transform.rotation = Quaternion.Euler(spawn.transform.rotation.x, spawn.transform.rotation.y, spawn.transform.rotation.z);
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

        public void On_Background_1_Start()
        {
            excavatorController.Animate();
        }

        public void On_Background_1_Finish()
        {
            
        }

        public void On_Background_2_Start()
        {
            customForkLiftController.Animate();
            anim.SetTrigger("Moving");
        }

        public void On_Background_2_Finish()
        {
            
        }

        public void On_Background_3_Start()
        {
            hammerController.Animate();
        }

        public void On_Background_3_Finish()
        {

        }

        #endregion
    }
}