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

        public Bounds _wallBounds
        {
            get { return wall.GetComponent<MeshCollider>().bounds; }
        }

        private Bounds _bounds
        {
            get { return gameObject.GetComponent<MeshCollider>().bounds; }
        }

        private PipeManipulation _pipeManipulation;
        private PipesContainerManager _pipesContainerManager;

        // show it only when it's on the wall
        private bool OnTheWall
        {
            get
            {
                var b1 = _wallBounds.Intersects(_bounds);
                if (transformer.isSimplePipe)
                {
                    if (_pipeManipulation == null)
                    {
                        _pipeManipulation = transformer.gameObject.GetComponent<PipeManipulation>();
                    }

                    return b1 | _pipeManipulation.collidingWall;
                }
                else
                {
                    if (_pipesContainerManager == null)
                    {
                        _pipesContainerManager = transform.gameObject.GetComponent<PipesContainerManager>();
                    }

                    return b1 | _pipesContainerManager.collidingWall;
                }
            }
        }

        [HideInInspector] public bool Clamped = false;

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
                if (hint.activeSelf)
                {
                    hint.SetActive(false);
                }
            }
            else
            {
                if (!positioned)
                {
                    MoveHint();
                }

                if (!hint.activeSelf)
                {
                    hint.SetActive(true);
                }
            }
        }

        public void Hide()
        {
            hint.SetActive(false);
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