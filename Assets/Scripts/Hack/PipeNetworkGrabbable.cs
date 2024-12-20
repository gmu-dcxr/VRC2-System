﻿using System.Collections;
using Fusion;
using Oculus.Interaction;
using UnityEngine;

using VRC2.Events;
using VRC2.Network;
using VRC2.Pipe;

namespace VRC2.Hack
{
    public class PipeNetworkGrabbable : NetworkGrabbable
    {

        [HideInInspector] public PointerEvent lastPointerEvent;

        [HideInInspector] public bool simulateReleased = false;

        private RPCMessager _messager;

        public RPCMessager messager
        {
            get
            {
                if (_messager == null)
                {
                    _messager = FindObjectOfType<RPCMessager>();
                }

                return _messager;
            }
        }

        protected override void Start()
        {
            this.BeginStart(ref _started, () => base.Start());

            // remove the OneGrabFreeTransformer
            var ogft = gameObject.GetComponent<OneGrabFreeTransformer>();
            GameObject.Destroy(ogft);

            // force update the OneGrabTransformer
            PipeGrabFreeTransformer defaultTransformer = gameObject.AddComponent<PipeGrabFreeTransformer>();
            _oneGrabTransformer = defaultTransformer;
            OneGrabTransformer = defaultTransformer;
            OneGrabTransformer.Initialize(this);

            this.EndStart(ref _started);
        }

        // sync last spawned pipe via rpc
        void UpdateLastSpawnedPipe(GameObject go)
        {
            var no = go.GetComponent<NetworkObject>();
            messager.UpdateLastSpawnedPipe(no.Id);
        }

        void ResetLastSpawnedPipe()
        {
            messager.ResetLastSpawnedPipe();
        }


        void SyncPipeTransform()
        {
            var nid = gameObject.GetComponent<NetworkObject>().Id;
            var t = gameObject.transform;
            messager.SyncPipeTransform(nid, t.position, t.rotation);
        }

        IEnumerator EnableNetworkTransform()
        {
            var go = gameObject;
            var rb = go.GetComponent<Rigidbody>();
            // make it fall
            rb.isKinematic = false;
            
            yield return new WaitForSeconds(0.1f);
            
            yield return new WaitWhile(() => { return rb.velocity.magnitude > 1e-3f; });

            // send to sync transform
            SyncPipeTransform();
            yield return new WaitForSeconds(1f);

            // enable network transform
            var nt = go.GetComponent<NetworkTransform>();
            nt.enabled = true;
        }


        public override bool NetworkedPointerEvent(PointerEvent evt)
        {
            simulateReleased = false;
            // update
            lastPointerEvent = evt;

            // process pointer event for networked pipe objects.
            // state authority and input authority are all at P1 side,
            // if game not started, directly return false to let parent handle it
            if (!gameStarted) return false;

            // if game started, scene object or spawned object 
            if ((IsSceneObject && hasStateAuthority) || (!IsSceneObject && hasInputAuthority))
            {
                // p1 side, call the parent method
                return false;
            }

            // return false if it is not a basic pipe
            var root = PipeHelper.GetRoot(gameObject);
            if (root != gameObject) return false;

            // // p2 can only manipulate straight non-cut pipe
            // var pm = gameObject.GetComponent<PipeManipulation>();
            //
            // // if it is bent or cut, directly return
            // if (!pm.IsStraight || !pm.NotBeingCut) return false;

            // p2 side
            // in order to make p2 can manipulate the pipe, it should disable networkTransform component
            // when it is select action; and enable networkTransform component when it is unselect action;
            var nt = gameObject.GetComponent<NetworkTransform>();

            switch (evt.Type)
            {
                case PointerEventType.Select:
                    // let p2 to manipulate it
                    nt.enabled = false;

                    // update last spawned pipe to enable it to be able to be picked up by the robot dog
                    var pm = gameObject.GetComponent<PipeManipulation>();

                    // only update if it's straight and not being cut
                    if (pm != null && pm.IsStraight && pm.NotBeingCut)
                    {
                        Debug.LogWarning("Last spawned pipe was updated.");
                        GlobalConstants.lastSpawnedPipe = gameObject;
                        UpdateLastSpawnedPipe(gameObject);
                    }
                    else
                    {
                        // clear it
                        GlobalConstants.lastSpawnedPipe = null;
                        ResetLastSpawnedPipe();
                    }

                    EndTransform();
                    break;
                case PointerEventType.Unselect:

                    StartCoroutine(EnableNetworkTransform());

                    // restore the networkTransform
                    // nt.enabled = true;

                    EndTransform();
                    break;
                case PointerEventType.Cancel:
                    EndTransform();
                    break;
            }

            BaseProcessPointerEvent(evt);

            switch (evt.Type)
            {
                case PointerEventType.Select:
                    BeginTransform();
                    break;
                case PointerEventType.Unselect:
                    BeginTransform();
                    break;
                case PointerEventType.Move:
                    UpdateTransform();
                    break;
            }

            return true;
        }

        public void OriginalProcessPointerEvent(PointerEvent evt)
        {
            base.OriginalProcessPointerEvent(evt);
        }
    }
}