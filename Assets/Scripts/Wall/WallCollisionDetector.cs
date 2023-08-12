using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VRC2;
using VRC2.Events;
using VRC2.Pipe;
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

        private Vector3 _wallExtends;

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

        float GetClampExtendsZ(GameObject clamp)
        {
            if (_clampExtendsZ == null)
            {
                _clampExtendsZ = new Dictionary<int, float>();

                _clampExtendsZ.Add(1, 0.00836f);
                _clampExtendsZ.Add(2, 0.02786f);
                _clampExtendsZ.Add(3, 0.03901f);
                _clampExtendsZ.Add(4, 0.05015f);
            }

            // BUG: Dynamically getting the bounds doesn't work in VR, use the predefined size instead.

            var size = clamp.GetComponent<ClampScaleInitializer>().clampSize;
            //
            // float res = 0f;
            // if (_clampExtendsZ.ContainsKey(size))
            // {
            //     res = _clampExtendsZ[size];
            // }
            // else
            // {
            //     var mc = clamp.GetComponentInChildren<BoxCollider>().bounds.extents;
            //     _clampExtendsZ.Add(size, mc.z);
            //
            //     res = mc.z;
            // }
            //
            // print($"clamp {size} - {res.ToString("F5")}");

            return _clampExtendsZ[size];
        }

        private void OnTriggerEnter(Collider other)
        {
            _mainThreadWorkQueue.Enqueue(() => { OnTriggerEnterAndStay(other); });

        }

        private void OnTriggerExit(Collider other)
        {
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
                HandlePipeCollision(go);
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

        bool ShouldPipeFall(GameObject root)
        {
            // return false if there is at least one ClampHintManager.Clamped == true
            var children = Utils.GetChildren<ClampHintManager>(root);

            foreach (var child in children)
            {
                var chm = child.GetComponent<ClampHintManager>();
                if (chm.Clamped) return false;
            }
            
            return true;
        }

        void HandlePipeCollision(GameObject pipe)
        {
            // here the pipe may belong to a pipe container
            // find the root object
            var root = pipe.transform;
            while (true)
            {
                if (root.parent == null) break;
                root = root.parent;
            }

            var ipipe = pipe.transform.parent.gameObject;

            var rootObject = root.gameObject;

            // get diameter
            var diameter = ipipe.GetComponent<PipeManipulation>().diameter;

            // get real diameter
            var pipez = _pipeDiameters[diameter];

            // disable gravity
            var rb = rootObject.GetComponent<Rigidbody>();
            GameObject.Destroy(rb);

            var t = rootObject.transform;
            var pos = t.position;
            var rot = t.rotation.eulerAngles;

            // get the wall transform
            var wt = gameObject.transform;
            var wpos = wt.position;
            var wrot = wt.rotation.eulerAngles;

            // set pipe's x rotation to the wall's x rotation
            rot.x = wrot.x;
            // set pipe's y rotation to the wall's y rotation
            rot.y = wrot.y + pipeYRotationOffset;
            rootObject.transform.rotation = Quaternion.Euler(rot);

            // update the pipe's distance to the wall
            pos.x = wpos.x + _wallExtends.x + pipez;

            rootObject.transform.position = pos;

            UpdateAllClampHints(rootObject, true);
        }

        Vector3 GetPipeRotation(GameObject pipe, GameObject wall)
        {
            var root = pipe.transform;
            while (true)
            {
                if (root.parent == null) break;
                root = root.parent;
            }

            var rot = pipe.transform.rotation.eulerAngles;
            var wrot = wall.transform.rotation.eulerAngles;

            rot.x = wrot.x;
            // set pipe's y rotation to the wall's y rotation
            rot.y = wrot.y + pipeYRotationOffset;
            root.transform.rotation = Quaternion.Euler(rot);

            return rot;
        }

        void UpdateAllClampHints(GameObject rootObject, bool onthewall)
        {
            var children = Utils.GetChildren<ClampHintManager>(rootObject);

            foreach (var child in children)
            {
                var chm = child.GetComponent<ClampHintManager>();
                chm.OnTheWall = onthewall;
            }
        }



        #endregion

        #region Handle Clamp's Collision with the Wall

        void HandleClampCollision(GameObject clamp)
        {
            // get the Interactable clamp
            var iclamp = clamp.transform.parent.gameObject;

            // enable kinematic to make it not fall
            Rigidbody rb = null;
            if (iclamp.TryGetComponent<Rigidbody>(out rb))
            {
                rb.isKinematic = true;
            }

            // get clamp z
            var clampz = GetClampExtendsZ(clamp);

            var t = iclamp.transform;
            var pos = t.position;
            var rot = t.rotation.eulerAngles;

            // get the wall transform
            var wt = gameObject.transform;
            var wpos = wt.position;
            var wrot = wt.rotation.eulerAngles;

            // clamp has the same x rotation with the wall
            // rot.x = wrot.x;
            rot.y = wrot.y + clampYRotationOffset;
            rot.z = wrot.z + clampZRotationOffset;

            // update rotation
            iclamp.transform.rotation = Quaternion.Euler(rot);
            // update distance
            pos.x = wpos.x + _wallExtends.x + clampz * 2;

            iclamp.transform.position = pos;
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