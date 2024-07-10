using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VRC2;
using VRC2.Events;
using VRC2.Pipe;
using VRC2.Pipe.Clamp;
using PipeDiameter = VRC2.Pipe.PipeConstants.PipeDiameter;

namespace VRC2
{

    [RequireComponent(typeof(MeshCollider))]
    public class WallCollisionDetector : MonoBehaviour
    {
        // pipe's axes are different from wall's 
        [Header("Pipe")] private float pipeYRotationOffset = -90f;

        [Header("Clamp")] private float clampYRotationOffset = 90;
        private float clampZRotationOffset = -90; // fixed

        [Header("Box")] private float boxYRotationOffset = -90f;

        private float boxDistanceOffset = 0.25f;

        [HideInInspector] public Bounds _wallBounds;
        [HideInInspector] public Vector3 _wallExtents;

        private IDictionary<PipeDiameter, float> _pipeDiameters;
        private IDictionary<int, float> _clampExtentsZ;

        private ConcurrentQueue<Action> _mainThreadWorkQueue = new ConcurrentQueue<Action>();

        private Plane _wallPlane;

        // when the distance is less than this value, start compensation
        [HideInInspector] public float compensationThreshold = 0.1f;


        // Start is called before the first frame update
        void Start()
        {
            InitPipeDiameters();
            InitClampExtentZ();
            _wallBounds = gameObject.GetComponent<BoxCollider>().bounds;
            _wallExtents = gameObject.GetComponent<BoxCollider>().bounds.extents;

            // init wall plane, this is to calculate the distance between object and the wall
            _wallPlane = new Plane(transform.right, transform.position);
        }

        void Update()
        {
            while (_mainThreadWorkQueue.TryDequeue(out Action workload))
            {
                workload();
            }
        }

        void InitPipeDiameters()
        {
            if (_pipeDiameters == null)
            {
                _pipeDiameters = new Dictionary<PipeDiameter, float>();

                var diameters = new List<PipeDiameter>()
                {
                    PipeDiameter.Diameter_1, PipeDiameter.Diameter_2, PipeDiameter.Diameter_3, PipeDiameter.Diameter_4
                };

                foreach (var diameter in diameters)
                {
                    // instantiate with the prefab and then get the diameter
                    var prefab = PipeHelper.GetStraightPipePrefab(diameter);
                    var go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                    var pipe = go.GetComponentInChildren<PipeCollisionDetector>().gameObject;
                    var z = pipe.GetComponent<MeshCollider>().bounds.extents.z;

                    print($"{diameter} - {z.ToString("f5")}");

                    _pipeDiameters.Add(diameter, z);

                    // destroy it
                    GameObject.Destroy(go);
                }
            }
        }

        void InitClampExtentZ()
        {
            if (_clampExtentsZ == null)
            {
                _clampExtentsZ = new Dictionary<int, float>();
                for (var i = 1; i <= 4; i++)
                {
                    var prefab = PipeHelper.GetClampPrefab(i);
                    var go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                    var csi = go.GetComponentInChildren<ClampScaleInitializer>();
                    var z = csi.GetComponent<MeshCollider>().bounds.extents.z;

                    print($"Clamp {i} - {z.ToString("f5")}");
                    _clampExtentsZ.Add(i, z);

                    GameObject.Destroy(go);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            _mainThreadWorkQueue.Enqueue(() => { OnTriggerEnterAndStay(other); });

        }

        private void OnTriggerExit(Collider other)
        {
            _mainThreadWorkQueue.Enqueue(() =>
            {
                var go = other.gameObject;
                if (go.CompareTag(GlobalConstants.pipeObjectTag))
                {
                    HandlePipeCollision(go, false);
                }
                else if (go.CompareTag(GlobalConstants.clampObjectTag))
                {
                    HandleClampCollision(go, false);
                }
            });
        }

        private void OnTriggerStay(Collider other)
        {
            _mainThreadWorkQueue.Enqueue(() => { OnTriggerEnterAndStay(other); });
        }

        void OnTriggerEnterAndStay(Collider other)
        {
            // get the game object
            var go = other.gameObject;
            if (go.CompareTag(GlobalConstants.pipeObjectTag))
            {
                HandlePipeCollision(go, true);
            }
            else if (go.CompareTag(GlobalConstants.clampObjectTag))
            {
                HandleClampCollision(go, true);
            }
            // box is deprecated
            // else if (go.CompareTag(GlobalConstants.boxObjectTag))
            // {
            //     HandleBoxCollision(go);
            // }
        }

        #region Hanle clamp's collision with the wall

        public float GetClampZBySize(int size)
        {
            return _clampExtentsZ[size];
        }

        #endregion

        #region Handle pipe's collision with the wall

        public float GetPipeZByDiameter(PipeDiameter diameter)
        {
            return _pipeDiameters[diameter];
        }

        public bool PipeFullyLeft(GameObject root)
        {
            var gos = Utils.GetChildren<PipeCollisionDetector>(root);
            foreach (var go in gos)
            {
                if (_wallBounds.Intersects(go.GetComponent<MeshCollider>().bounds)) return false;
            }

            return true;
        }

        void HandlePipeCollision(GameObject pipe, bool enter)
        {
            // here the pipe may belong to a pipe container
            // find the root object
            var root = PipeHelper.GetRoot(pipe);

            var ipipe = pipe.transform.parent.gameObject;

            // distance between the wall plane
            // distance < 0 means it is inside the wall
            var distance = GetSignedDistance(root.transform.position);

            if (root != ipipe)
            {
                // root is a pipe container
                var pcm = root.GetComponent<PipesContainerManager>();

                // return if collidingwall is true and it is inside the wall
                if (pcm == null || (pcm.collidingWall && distance < 0)) return;

                if (!enter)
                {
                    // check whether the pipe is fully away from the wall
                    var left = PipeFullyLeft(root);
                    enter = !left;
                }

                pcm.collidingWall = enter;

                // update clamp hint managers
                foreach (var chm in pcm.clampHintsManagers)
                {
                    chm.SetOnTheWall(enter);
                }
            }
            else
            {
                // it's a simple pipe
                var pm = ipipe.GetComponent<PipeManipulation>();

                // return if collidingwall is true and it is inside the wall
                if (pm.collidingWall && distance < 0) return;

                pm.collidingWall = enter;

                // update clamp hint managers
                foreach (var chm in pm.clampHintsManagers)
                {
                    chm.SetOnTheWall(enter);
                }
            }
        }


        #endregion

        #region Handle Clamp's Collision with the Wall

        void HandleClampCollision(GameObject clamp, bool enter)
        {
            // get the Interactable clamp
            var iclamp = clamp.transform.parent.gameObject;

            var distance = GetSignedDistance(iclamp.transform.position);
            var cm = iclamp.GetComponent<ClampManipulation>();

            // do nothing when colliding with the wall and inside the wall
            if (cm.collidingWall && distance < 0) return;

            cm.collidingWall = enter;
        }



        #endregion

        #region Handle Box's Collision with the Wall

        void HandleBoxCollision(GameObject box)
        {
            // get the Interactable box
            var ibox = box.transform.parent.gameObject;

            var t = ibox.transform;
            var pos = t.position;
            var rot = t.rotation.eulerAngles;

            // get the wall transform
            var wt = gameObject.transform;
            var wpos = wt.position;
            var wrot = wt.rotation.eulerAngles;

            // set pipe's x rotation to the wall's x rotation
            rot.x = wrot.x;
            // set pipe's y rotation to the wall's y rotation
            rot.y = wrot.y + boxYRotationOffset;
            // update
            ibox.transform.rotation = Quaternion.Euler(rot);

            // update the pipe's distance to the wall
            pos.x = wpos.x + boxDistanceOffset;

            ibox.transform.position = pos;
        }



        #endregion

        // distance between the wall and the position
        public float GetSignedDistance(Vector3 position)
        {
            return _wallPlane.GetDistanceToPoint(position);
        }

        public float GetAbsDistance(Vector3 position)
        {
            return Mathf.Abs(GetSignedDistance(position));
        }

        public bool ShouldCompensate(Vector3 position)
        {
            // it compensates even inside the wall
            var d = GetSignedDistance(position);
            return d < compensationThreshold;
        }
    }
}