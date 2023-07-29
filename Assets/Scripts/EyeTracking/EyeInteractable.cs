using System;
using UnityEngine;
using UnityEngine.Events;

namespace VRC2
{
    [RequireComponent(typeof(Collider))]
    // [RequireComponent(typeof(Rigidbody))]
    public class EyeInteractable : MonoBehaviour
    {
        public bool IsHovered { get; set; }

        public UnityEvent<GameObject> OnHover;

        public Material hoverMaterial;
        public Material unHoverMaterial;

        private MeshRenderer _meshRenderer;

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            if (IsHovered)
            {
                _meshRenderer.material = hoverMaterial;
                OnHover?.Invoke(gameObject);
            }
            else
            {
                _meshRenderer.material = unHoverMaterial;
            }
        }
    }
}