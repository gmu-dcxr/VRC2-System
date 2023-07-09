using System;
using System.Collections.Concurrent;
using System.Linq;
using NodeCanvas.Tasks.Actions;
using Oculus.Interaction.DistanceReticles;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VRC2.Events
{
    public class PipeCollisionDetector : MonoBehaviour
    {
        [HideInInspector] public GameObject connecting;
        private Object pipeParent;

        private bool connected = false;

        private ConcurrentQueue<Action> _mainThreadWorkQueue = new ConcurrentQueue<Action>();

        private void Start()
        {
            InitializePokeLocation();
            // pre-load object
            pipeParent = AssetDatabase.LoadAssetAtPath(GlobalConstants.pipePipeConnectorPrefabPath, typeof(GameObject));
        }

        private void Update()
        {
            while (_mainThreadWorkQueue.TryDequeue(out Action workload))
            {
                workload();
            }
        }

        void InitializePokeLocation()
        {
            try
            {
                if (GlobalConstants.LeftPokeObject == null)
                {
                    // make it only set once
                    var name = "PokeLocation";
                    var objects = VRC2.Utils.FindAll(name);
                    foreach (var obj in objects)
                    {
                        var ppp = obj.transform.parent.parent.parent.gameObject;
                        // LeftController
                        if (ppp.name.StartsWith("Left"))
                        {
                            GlobalConstants.LeftPokeObject = obj;

                            Debug.Log("Set LeftPokeObject");
                        }
                        else if (ppp.name.StartsWith("Right")) // RightController
                        {
                            GlobalConstants.RightPokeObject = obj;
                            Debug.Log("Set RightPokeObject");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Exception in InitializePokeLocation(): {e.ToString()}");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            _mainThreadWorkQueue.Enqueue(() => {
                OnTriggerEnterAndStay(other);
            });
        }

        private void OnTriggerExit(Collider other)
        {
        }

        private void OnTriggerStay(Collider other)
        {
            _mainThreadWorkQueue.Enqueue(() => {
                OnTriggerEnterAndStay(other);
            });
        }

        void OnTriggerEnterAndStay(Collider other)
        {
            var go = other.gameObject;
            if (go.CompareTag(GlobalConstants.pipeObjectTag))
            {
                HandlePipeCollision(go);
            }

            // collision with clamp
            if (go.CompareTag(GlobalConstants.clampObjectTag))
            {
                HandleClampCollision(go);
            }
        }

        void HandleClampCollision(GameObject clamp)
        {
            // get the Interactable clamp
            var iclamp = clamp.transform.parent.gameObject;

            // delete its rigid body
            Rigidbody rb = null;
            if (iclamp.TryGetComponent<Rigidbody>(out rb))
            {
                // clamp doesn't collide with the wall
                Debug.Log("Clamp doesn't collide with the wall.");
                return;
            }

            // remove pipe's rigid boby
            // current interactable pipe
            var cip = gameObject.transform.parent.parent;

            // find root parent (this is caused by the pipe merging)
            var root = cip;
            while (true)
            {
                if (root.parent == null) break;

                root = root.parent;
            }

            // remove the rigid body of the root object
            if (root.gameObject.TryGetComponent<Rigidbody>(out rb))
            {
                // remove its rigid body
                Debug.Log($"Remove rigid body for pipe {root.gameObject.name}");
                GameObject.Destroy(rb);
            }
        }

        (Vector3, Vector3) GetIndexedPoint(GameObject go)
        {
            // TODO: select index by different pipe models
            var index1 = GlobalConstants.PipeStraight1InchIndex1;
            var index2 = GlobalConstants.PipeStraight1InchIndex2;
            // get it in the world space
            // refer: https://stackoverflow.com/questions/61351923/unity-get-mesh-after-scaling
            var mesh = go.GetComponent<MeshFilter>().mesh;
            var vertices = mesh.vertices;
            var t = go.transform;
            // left and right
            var left = vertices[index1];
            var right = vertices[index2];
            // since the model x points +x direction, smaller x is left
            if (left.x < right.x)
            {
                return (t.TransformPoint(left), t.TransformPoint(right));
            }
            else
            {
                return (t.TransformPoint(right), t.TransformPoint(left));
            }
        }

        void HandlePipeCollisionV2(GameObject otherpipe)
        {
            if (connected) return;

            Debug.Log($"HandlePipeCollisionV2: {otherpipe.name}");
            var cip = gameObject.transform.parent.parent;

            // get root cip
            while (true)
            {
                if (cip.parent == null) break;
                cip = cip.parent;
            }

            var oip = otherpipe.transform.parent.parent.gameObject; // Interactable pipe

            var (p1Left, p1Right) = GetIndexedPoint(gameObject);
            var (p2Left, p2Right) = GetIndexedPoint(otherpipe);

            // second, calculate the target distance
            var d1 = Vector3.Distance(p1Left, p1Right);
            var d2 = Vector3.Distance(p2Left, p2Right);

            var offset = Vector3.zero;
            offset.x = (d1 + d2) / 2.0f;

            // make a dummy object to calculate the target position
            var dummy = new GameObject();
            dummy.transform.position = cip.transform.position;
            dummy.transform.rotation = cip.transform.rotation;
            dummy.transform.Translate(offset, Space.Self);
            var targetPos = dummy.transform.position;

            // destroy dummy
            GameObject.Destroy(dummy);

            // update oip
            oip.transform.position = targetPos;

            var pos = GlobalConstants.RightPokeObject.transform.position;

            // connected
            var parentObject = Instantiate(pipeParent, pos, cip.rotation) as GameObject;

            // update parent to make them move together
            cip.transform.parent = parentObject.transform;
            oip.transform.parent = parentObject.transform;

            // update local position
            var localCip = cip.transform.localPosition;
            var localOip = oip.transform.localPosition;

            localCip.y = 0;
            localCip.z = 0;

            localOip.y = 0;
            localOip.z = 0;

            cip.transform.localPosition = localCip;
            oip.transform.localPosition = localOip;


            // fix local rotation
            var rot = Quaternion.Euler(0, 0, 0);
            cip.transform.localRotation = rot;
            oip.transform.localRotation = rot;

            // disable children's interactions
            DisableInteraction(cip.gameObject);
            // disable collision detecting for the left one
            DisableCollisionDetector(cip.gameObject);
            DisableInteraction(oip.gameObject);

            connected = true;
        }

        void HandlePipeCollision(GameObject otherpipe)
        {
            // update connecting
            connecting = otherpipe.transform.parent.parent.gameObject; // Interactable pipe

            // only move the pipe held by the right hand to right
            var leftHandPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            var rightHandPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

            // other pipe distance should be left hand > right hand
            var otherIpipePos = connecting.transform.position;
            if (Vector3.Distance(otherIpipePos, leftHandPos) <
                Vector3.Distance(otherIpipePos, rightHandPos))
            {
                // Other pipe is held by the left hand
                return;
            }
            // now, otherpipe is on the right-hand

            // new version
            HandlePipeCollisionV2(otherpipe);
        }

        void DisableRigidBody(GameObject interactable)
        {
            Rigidbody rb = null;
            if (interactable.TryGetComponent<Rigidbody>(out rb))
            {
                // delete its rigid body
                GameObject.Destroy(rb);
            }
        }

        void DisableInteraction(GameObject interactable)
        {
            // disable GrabInteractable who own ReticleDataIcon
            var go = interactable.GetComponentInChildren<ReticleDataIcon>();
            if (go != null)
            {
                go.gameObject.SetActive(false);
                DisableRigidBody(interactable);
            }
        }

        void DisableCollisionDetector(GameObject obj)
        {
            var pcd = obj.GetComponentInChildren<PipeCollisionDetector>(false);
            pcd.enabled = false;
        }
    }
}