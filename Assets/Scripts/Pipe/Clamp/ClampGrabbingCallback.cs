using System;
using UnityEngine;

namespace VRC2.Events
{
    [RequireComponent(typeof(GeneralGrabbable))]
    public class ClampGrabbingCallback: MonoBehaviour
    {
        public GameObject clampGameObject;
        private GeneralGrabbable _generalGrabbable;

        private void Start()
        {
            _generalGrabbable = gameObject.GetComponent<GeneralGrabbable>();
            
            _generalGrabbable.OnSelect += OnSelect;
            _generalGrabbable.OnUnselect += OnUnselect;
        }

        private void OnSelect()
        {
            
        }
        
        private void OnUnselect()
        {
            
        }
    }
}