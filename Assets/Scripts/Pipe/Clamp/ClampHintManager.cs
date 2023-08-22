using System;
using UnityEngine;
using VRC2.Hack;
using VRC2.Pipe;

namespace VRC2.Pipe
{
    public class ClampHintManager : MonoBehaviour
    {
        public GameObject hint;

        private bool positioned = false;

        // whether to show it, the right part of the connected pipe will be set to false
        [HideInInspector] public bool CanShow = true;

        private PipeManipulation _pipeManipulation;
        private PipesContainerManager _pipesContainerManager;

        // show it only when it's on the wall
        [HideInInspector] public bool OnTheWall;

        [HideInInspector] public bool Clamped = false;

        private MeshRenderer _meshRenderer;

        private MeshRenderer meshRenderer
        {
            get
            {
                if (_meshRenderer == null)
                {
                    _meshRenderer = hint.GetComponent<MeshRenderer>();
                }

                return _meshRenderer;
            }
        }

        #region Wall

        private GameObject _wall;

        private GameObject wall
        {
            get
            {
                if (_wall == null)
                {
                    print("_wall is set");
                    _wall = GameObject.FindGameObjectWithTag(GlobalConstants.wallTag);
                }

                return _wall;
            }
        }

        private PipeGrabFreeTransformer _transformer;

        private PipeGrabFreeTransformer transformer
        {
            get
            {
                if (_transformer == null)
                {
                    _transformer = PipeHelper.GetRoot(gameObject).GetComponent<PipeGrabFreeTransformer>();
                }

                return _transformer;
            }
        }

        #endregion

        private void Start()
        {
            // Show();
            Hide();
        }

        public void Update()
        {
            // show only when CanShow is true and only when pipe is on the wall
            if (!CanShow || !OnTheWall || Clamped)
            {
                if (meshRenderer.enabled)
                {
                    UpdateMeshRenderer(false);
                }
            }
            else
            {
                if (!positioned)
                {
                    MoveHint();
                }

                if (!meshRenderer.enabled)
                {
                    UpdateMeshRenderer(true);
                }
            }
        }

        public void Hide()
        {
            UpdateMeshRenderer(false);
        }

        void UpdateMeshRenderer(bool enabled)
        {
            meshRenderer.enabled = enabled;
        }

        void MoveHint()
        {
            var (cc, cr) = PipeHelper.GetRightMostCenter(gameObject);

            var rot = gameObject.transform.rotation;
            var up = gameObject.transform.up;
            var forward = Vector3.Cross(cr, up);

            rot = Quaternion.LookRotation(forward, up);

            hint.transform.position = cc;
            hint.transform.rotation = rot;

            positioned = true;
        }
    }
}