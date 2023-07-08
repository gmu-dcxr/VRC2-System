using System;
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

        private void Start()
        {
            InitializePokeLocation();
            // pre-load object
            pipeParent = AssetDatabase.LoadAssetAtPath(GlobalConstants.pipePipeConnectorPrefabPath, typeof(GameObject));
        }

        private void Update()
        {

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
            OnTriggerEnterAndStay(other);
        }

        private void OnTriggerExit(Collider other)
        {
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerEnterAndStay(other);
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

            // current interactable pipe (left-hand)
            var cip = gameObject.transform.parent.parent;

            if (cip.parent == null)
            {
                // left is a pipe
                HandlePipePipeConnecting(otherpipe);
            }
            else
            {
                // left is a container
                HandleContainerPipeConnecting(otherpipe);
            }
        }

        // left is a pipe, right is a pipe
        void HandlePipePipeConnecting(GameObject otherpipe)
        {
            Debug.Log($"HandlePipePipeConnecting: {otherpipe.name}");
            var cip = gameObject.transform.parent.parent;
            if (connecting.transform.rotation != cip.rotation)
            {
                // update rotation first, and update position in the next loop
                connecting.transform.rotation = cip.rotation;
            }
            else if (!connected)
            {
                // update position
                // var cbounds = gameObject.GetComponent<Renderer>().bounds;
                // var obounds = otherpipe.GetComponent<Renderer>().bounds;

                var cbounds = gameObject.GetComponent<MeshCollider>().bounds;
                var obounds = otherpipe.GetComponent<MeshCollider>().bounds;

                // expected distance
                var ed = cbounds.extents.x + obounds.extents.x;

                connecting.transform.position = cip.position + connecting.transform.right * ed;

                var pos = GlobalConstants.RightPokeObject.transform.position;

                // connected
                var parentObject = Instantiate(pipeParent, pos, cip.rotation) as GameObject;

                // update bounds information
                var pcm = parentObject.GetComponent<PipeContainerManager>();
                pcm.leftChildBounds = cbounds;
                pcm.rightChildBounds = obounds;

                // update parent to make them move together
                cip.transform.parent = parentObject.transform;
                connecting.transform.parent = parentObject.transform;

                // update local position
                var localCip = cip.transform.localPosition;
                var localOip = connecting.transform.localPosition;

                localCip.y = 0;
                localCip.z = 0;

                localOip.y = 0;
                localOip.z = 0;

                cip.transform.localPosition = localCip;
                connecting.transform.localPosition = localOip;

                // disable children's interactions
                DisableInteraction(cip.gameObject);
                // diable collision detecting for the left one
                DisableCollisionDetector(cip.gameObject);
                DisableInteraction(connecting.gameObject);

                connected = true;
            }
        }

        // left is a container, right is a pipe
        void HandleContainerPipeConnecting(GameObject otherpipe)
        {
            Debug.Log($"HandleContainerPipeConnecting: {otherpipe.name}");
            // interactable pipe container
            var cic = gameObject.transform.parent.parent.parent;
            // get the left connected pipe, the 1st child with name "pipe"
            if (connecting.transform.rotation != cic.rotation)
            {
                // update rotation first, and update position in the next loop
                connecting.transform.rotation = cic.rotation;
            }
            else if (!connected)
            {
                var pcm = cic.gameObject.GetComponent<PipeContainerManager>();
                // right bounds
                var cbounds = pcm.rightChildBounds;
                // var obounds = otherpipe.GetComponent<Renderer>().bounds;
                var obounds = otherpipe.GetComponent<MeshCollider>().bounds;

                // expected distance
                var ed = cbounds.extents.x + obounds.extents.x;

                connecting.transform.position = cic.position + connecting.transform.right * ed;

                var pos = GlobalConstants.RightPokeObject.transform.position;

                // connected
                var parentObject = Instantiate(pipeParent, pos, cic.rotation) as GameObject;

                // update bounds information
                var pcmNew = parentObject.GetComponent<PipeContainerManager>();
                pcmNew.leftChildBounds = cbounds;
                pcmNew.rightChildBounds = obounds;

                // update parent to make them move together
                cic.transform.parent = parentObject.transform;
                connecting.transform.parent = parentObject.transform;

                // update local position
                var localCip = cic.transform.localPosition;
                var localOip = connecting.transform.localPosition;

                localCip.y = 0;
                localCip.z = 0;

                localOip.y = 0;
                localOip.z = 0;

                cic.transform.localPosition = localCip;
                connecting.transform.localPosition = localOip;

                // disable children's interactions
                DisableInteraction(cic.gameObject);
                // diable collision detecting for the left one
                DisableCollisionDetector(cic.gameObject);

                DisableInteraction(connecting.gameObject);

                connected = true;
            }
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