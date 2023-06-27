using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace VRC2
{

    public enum PipeMaterialColor
    {
        Default = 0,
        Magenta = 1,
        Blue = 2,
        Yellow = 3,
        Green = 4
    }

    public class PipeManipulation : MonoBehaviour
    {
        [SerializeField] private GameObject _pipe;
        [SerializeField] private Material _magentaMaterial;
        [SerializeField] private Material _blueMaterial;
        [SerializeField] private Material _yellowMaterial;
        [SerializeField] private Material _greenMaterial;

        private Renderer _renderer;

        [HideInInspector]
        public GameObject pipe
        {
            get => _pipe;
        }

        [HideInInspector]
        public Renderer renderer
        {
            get
            {
                if (_renderer == null)
                {
                    _renderer = _pipe.GetComponent<Renderer>();
                }

                return _renderer;
            }
        }

        // default material
        private Material _defaultMaterial;

        // current color
        public PipeMaterialColor pipeColor = PipeMaterialColor.Green;
        public float pipeSize = 0.5f;


        // Start is called before the first frame update
        void Start()
        {
            _defaultMaterial = renderer.material;

            // whether it's the cloned object
            var no = gameObject.GetComponent<NetworkObject>();
            if (no.IsSceneObject)
            {
                // not spawned object
                SetMaterial(pipeColor);
                SetSize(pipeSize);
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void SetMaterial(PipeMaterialColor color)
        {
            Material material = null;
            switch (color)
            {
                case PipeMaterialColor.Magenta:
                    material = _magentaMaterial;
                    break;
                case PipeMaterialColor.Blue:
                    material = _blueMaterial;
                    break;
                case PipeMaterialColor.Yellow:
                    material = _yellowMaterial;
                    break;
                case PipeMaterialColor.Green:
                    material = _greenMaterial;
                    break;
                default:
                    break;
            }

            if (material != null)
            {
                renderer.material = material;
            }
        }

        void RestoreMaterial()
        {
            renderer.material = _defaultMaterial;
        }

        public void SetSize(float size)
        {
            Debug.Log($"Set Size: {size}");
            Debug.Log($"Set Size: {_pipe.transform.localScale}");
            // TODO: size mapping
            _pipe.transform.localScale = new Vector3(size, size, size);
            Debug.Log($"Set Size: {_pipe.transform.localScale}");
        }
    }
}