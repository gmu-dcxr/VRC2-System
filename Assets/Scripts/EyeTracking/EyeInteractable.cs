using System;
using UnityEngine;
using UnityEngine.Events;

namespace VRC2
{
    [RequireComponent(typeof(Collider))]
    // [RequireComponent(typeof(Rigidbody))]
    public class EyeInteractable : MonoBehaviour
    {
        [HideInInspector] public bool IsHovered { get; set; }

        public UnityEvent<GameObject> OnHover;

        [Header("For Debug")] public Material hoverMaterial;
        public Material unhoverMaterial;

        private MeshRenderer _meshRenderer;

        private EyeTrackingLogger _eyeTrackingLogger;

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();

            _eyeTrackingLogger = FindFirstObjectByType<EyeTrackingLogger>();
        }

        private void Update()
        {
            if (IsHovered)
            {
                if (hoverMaterial != null)
                {
                    _meshRenderer.material = hoverMaterial;
                }

                OnHover?.Invoke(gameObject);
                if (_eyeTrackingLogger != null)
                {
                    _eyeTrackingLogger.WriteLog(gameObject);
                }
            }
            else
            {
                if (unhoverMaterial != null)
                {
                    _meshRenderer.material = unhoverMaterial;
                }
            }
        }
    }
}