using System;
using Fusion;
using Oculus.Interaction;
using UnityEngine;

namespace VRC2.Pipe
{
    [RequireComponent(typeof(PointableUnityEventWrapper))]
    public class PipeConnectorManipulation : NetworkBehaviour
    {
        private PointableUnityEventWrapper _wrapper;

        private PipeManipulation _pipeManipulation;

        public PipeManipulation pipeManipulation
        {
            get
            {
                if (_pipeManipulation == null)
                {
                    _pipeManipulation = GetComponent<PipeManipulation>();
                }

                return _pipeManipulation;
            }
        }

        public GameObject Segment1 => pipeManipulation.SegA;
        public GameObject Segment2 => pipeManipulation.SegB;
        public GameObject SegmentM => pipeManipulation.SegM;

        [HideInInspector] public bool Flipped = false;

        [HideInInspector] public bool Selected = false;

        private Rigidbody _rigidbody
        {
            get => GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _wrapper = gameObject.GetComponent<PointableUnityEventWrapper>();
            _wrapper.WhenSelect.AddListener(OnSelect);
            _wrapper.WhenRelease.AddListener(OnRelease);
        }

        public void OnSelect()
        {
            // enable kinematic
            _rigidbody.isKinematic = true;
            Selected = true;
        }

        public void OnRelease()
        {
            // disable kinematic
            _rigidbody.isKinematic = false;
            Selected = false;
        }

        private (Vector3, Vector3, Vector3) GetRelTransform(GameObject from, GameObject to)
        {
            var t = from.transform;
            var p = t.InverseTransformPoint(to.transform.position);
            var f = t.InverseTransformVector(to.transform.forward);
            var u = t.InverseTransformVector(to.transform.up);

            return (p, f, u);
        }

        private (Vector3, Quaternion) GetAbsTransform(GameObject from, Vector3 p, Vector3 f, Vector3 u)
        {
            var t = from.transform;
            p = t.TransformPoint(p);
            f = t.TransformVector(f);
            u = t.TransformVector(u);

            return (p, Quaternion.LookRotation(f, u));
        }

        public void Flip()
        {
            Flipped = !Flipped;
            
            print("before");
            print(Segment2.transform.localRotation.eulerAngles);
            print(Segment1.transform.localRotation.eulerAngles);
            // rotate around x of seg 2
            var (p1, f1, u1) = GetRelTransform(Segment2, Segment1);
            var (pm, fm, um) = GetRelTransform(Segment2, SegmentM);

            // rotate
            Segment2.transform.Rotate(Vector3.right, 180f, Space.Self);

            // restore
            var (pos1, rot1) = GetAbsTransform(Segment2, p1, f1, u1);
            var (posm, rotm) = GetAbsTransform(Segment2, pm, fm, um);
            // update
            Segment1.transform.position = pos1;
            Segment1.transform.rotation = rot1;

            SegmentM.transform.position = posm;
            SegmentM.transform.rotation = rotm;
            
            // after
            print("after");
            print(Segment2.transform.localRotation.eulerAngles);
            print(Segment1.transform.localRotation.eulerAngles);

            // use rpc to sync
            var l1 = Segment1.transform.localPosition;
            var r1 = Segment1.transform.localRotation;
            // use rpc to sync
            var l2 = Segment2.transform.localPosition;
            var r2 = Segment2.transform.localRotation;
            var lm = SegmentM.transform.localPosition;
            var rm = SegmentM.transform.localRotation;

            SyncRotation(l1, r1, l2, r2, lm, rm);
        }

        void SyncRotation(Vector3 l1, Quaternion r1, Vector3 l2, Quaternion r2, Vector3 lm, Quaternion rm)
        {
            var no = GetComponent<NetworkObject>();
            if (no != null && no.Runner != null && no.Runner.IsRunning)
            {
                RPC_SendMessage(no.Id, l1, r1, l2, r2, lm, rm);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SendMessage(NetworkId nid, Vector3 l1, Quaternion r1, Vector3 l2, Quaternion r2, Vector3 lm,
            Quaternion rm,
            RpcInfo info = default)
        {
            // sync local rotation of other pipe (the last connected pipe/connector)
            if (info.IsInvokeLocal)
            {
                print($"Sync connector rotation {nid}");
            }
            else
            {
                print($"Sync connector rotation {nid}");
                var Runner = GetComponent<NetworkObject>().Runner;
                var pcm = Runner.FindObject(nid).gameObject.GetComponent<PipeConnectorManipulation>();

                pcm.Segment1.transform.localPosition = l1;
                pcm.Segment1.transform.localRotation = r1;

                pcm.Segment2.transform.localPosition = l2;
                pcm.Segment2.transform.localRotation = r2;

                pcm.SegmentM.transform.localPosition = lm;
                pcm.SegmentM.transform.localRotation = rm;

                var flipped = pcm.Flipped;
                pcm.Flipped = !flipped;
            }
        }
    }
}