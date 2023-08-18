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
        [Header("Pipe")] public float pipeYRotationOffset = -90f;

        [Header("Clamp")] public float clampYRotationOffset = 90;
        public float clampZRotationOffset = -90; // fixed

        [Header("Box")] public float boxYRotationOffset = -90f;

        public float boxDistanceOffset = 0.25f;

        [HideInInspector] public Vector3 _wallExtends;

        private IDictionary<PipeDiameter, float> _pipeDiameters;
        private IDictionary<int, float> _clampExtendsZ;

        private ConcurrentQueue<Action> _mainThreadWorkQueue = new ConcurrentQueue<Action>();


        // Start is called before the first frame update
        void Start()
        {
            InitPipeDiameters();
            _wallExtends = gameObject.GetComponent<MeshCollider>().bounds.extents;
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

        private void OnTriggerEnter(Collider other)
        {
            _mainThreadWorkQueue.Enqueue(() => { OnTriggerEnterAndStay(other); });

        }

        private void OnTriggerExit(Collider other)
        {
            var go = other.gameObject;
            if (go.CompareTag(GlobalConstants.pipeObjectTag))
            {
                HandlePipeCollision(go, false);
            }
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
                HandleClampCollision(go);
            }
            else if (go.CompareTag(GlobalConstants.boxObjectTag))
            {
                HandleBoxCollision(go);
            }
        }

        #region Handle Pipe's Collision with the Wall

        public float GetPipeZByDiameter(PipeDiameter diameter)
        {
            return _pipeDiameters[diameter];
        }

        void HandlePipeCollision(GameObject pipe, bool enter)
        {
            // here the pipe may belong to a pipe container
            // find the root object
            var root = PipeHelper.GetRoot(pipe);

            var ipipe = pipe.transform.parent.gameObject;

            if (root != ipipe)
            {
                // root is a pipe container
                var pcm = root.GetComponent<PipesContainerManager>();
                pcm.collidingWall = enter;
            }
            else
            {
                // it's a simple pipe
                var pm = ipipe.GetComponent<PipeManipulation>();
                pm.collidingWall = enter;
            }
        }


        #endregion

        #region Handle Clamp's Collision with the Wall

        void HandleClampCollision(GameObject clamp)
        {
            // get the Interactable clamp
            var iclamp = clamp.transform.parent.gameObject;

            var cm = iclamp.GetComponent<ClampManipulation>();
            cm.collidingWall = true;

            // enable kinematic to make it not fall
            Rigidbody rb = null;
            if (iclamp.TryGetComponent<Rigidbody>(out rb))
            {
                rb.isKinematic = true;
            }

            // // get clamp z
            // var clampz = GetClampExtendsZ(clamp);
            //
            // var t = iclamp.transform;
            // var pos = t.position;
            // var rot = t.rotation.eulerAngles;
            //
            // // get the wall transform
            // var wt = gameObject.transform;
            // var wpos = wt.position;
            // var wrot = wt.rotation.eulerAngles;
            //
            // // clamp has the same x rotation with the wall
            // // rot.x = wrot.x;
            // rot.y = wrot.y + clampYRotationOffset;
            // rot.z = wrot.z + clampZRotationOffset;
            //
            // // update rotation
            // iclamp.transform.rotation = Quaternion.Euler(rot);
            // // update distance
            // pos.x = wpos.x + _wallExtends.x + clampz * 2;
            //
            // iclamp.transform.position = pos;
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
    }
}