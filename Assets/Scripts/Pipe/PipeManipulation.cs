using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

using PipeMaterialColor = VRC2.Pipe.PipeConstants.PipeMaterialColor;
using PipeType = VRC2.Pipe.PipeConstants.PipeType;
using PipeBendAngles = VRC2.Pipe.PipeConstants.PipeBendAngles;
using PipeDiameter = VRC2.Pipe.PipeConstants.PipeDiameter;

namespace VRC2
{
    public class PipeManipulation : MonoBehaviour
    {
        [SerializeField] private Material _magentaMaterial;
        [SerializeField] private Material _blueMaterial;
        [SerializeField] private Material _yellowMaterial;
        [SerializeField] private Material _greenMaterial;

        // current color
        public PipeMaterialColor pipeColor = PipeMaterialColor.Green;
        public PipeType pipeType = PipeType.Sewage;

        [Header("Pipes")] [SerializeField] private List<GameObject> pipes;

        private Renderer _renderer;
        private GameObject _pipe;

        [HideInInspector]
        public GameObject pipe
        {
            get => _pipe;
        }

        public Renderer renderer
        {
            get { return _pipe.GetComponent<Renderer>(); }
        }

        // default material
        private Material _defaultMaterial;
        [HideInInspector] public float pipeLength = 1.0f;
        [HideInInspector] public PipeBendAngles angle = PipeBendAngles.Default;

        public PipeDiameter diameter
        {
            get
            {
                if (_pipe == null) return PipeDiameter.Default;
                else
                {
                    var name = _pipe.name;
                    // get diameter from the name
                    int value = int.Parse(name.Substring(0, 1));
                    // Diameter_1 = 0 and so forth
                    var v = (PipeDiameter)(value - 1);
                    return v;
                }
            }
        }

        private IDictionary<PipeBendAngles, GameObject> _anglesObjects;

        [HideInInspector]
        public IDictionary<PipeBendAngles, GameObject> anglesObjects
        {
            get
            {
                if (_anglesObjects == null)
                {
                    InitAnglesObjects();
                }

                return _anglesObjects;
            }
        }


        // Start is called before the first frame update
        void Start()
        {

            // default angle
            angle = PipeBendAngles.Angle_0;

            // default is the straight one
            _pipe = anglesObjects[angle];

            // only enable the straigh one
            EnableOnly(angle);

            _defaultMaterial = renderer.material;

            // whether it's the cloned object
            var no = gameObject.GetComponent<NetworkObject>();
            if (no.IsSceneObject)
            {
                // not spawned object
                SetMaterial(pipeColor);
                // TODO: disabled for debugging connecting pipes
                SetLength(0.1f);
            }
        }

        void InitAnglesObjects()
        {
            _anglesObjects = new Dictionary<PipeBendAngles, GameObject>();
            foreach (var go in pipes)
            {
                var name = go.name;

                Debug.Log($"name: {name}");

                var key = PipeBendAngles.Default;

                if (name.Contains("90"))
                {
                    key = PipeBendAngles.Angle_90;
                }
                else if (name.Contains("45"))
                {
                    key = PipeBendAngles.Angle_45;
                }
                else if (name.Contains("135"))
                {
                    key = PipeBendAngles.Angle_135;
                }
                else if (name.Contains("straight"))
                {
                    key = PipeBendAngles.Angle_0;
                }

                if (key != PipeBendAngles.Default)
                {
                    _anglesObjects.Add(key, go);
                }
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

        public void SetLength(float length)
        {
            // TODO: Only change x (length)
            _pipe.transform.localScale = new Vector3(length, 1, 1);
        }

        public void SimulateStraightCut(float feet)
        {
            // the default lenght is 10 feet
            // set the scale
            _pipe.transform.localScale = new Vector3(feet / 10.0f, 1, 1);
        }

        #region Pointable Event

        public void OnSelect()
        {
            Debug.Log("Pipe OnSelect");
            // update current select pipe
            GlobalConstants.selectedPipe = gameObject;
        }

        public void OnUnselect()
        {
            Debug.Log("Pipe OnUnselect");
        }

        #endregion

        #region Enable/Disable

        public void EnableOnly(PipeBendAngles angle)
        {
            foreach (var kvp in anglesObjects)
            {
                var k = kvp.Key;
                if (k == angle)
                {
                    kvp.Value.SetActive(true);
                    // update current pipe
                    angle = k;
                    _pipe = kvp.Value;
                }
                else
                {
                    kvp.Value.SetActive(false);
                }
            }
        }

        #endregion
    }
}