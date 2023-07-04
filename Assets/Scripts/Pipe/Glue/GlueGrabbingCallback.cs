using System;
using UnityEngine;

namespace VRC2.Events
{
    [RequireComponent(typeof(GeneralGrabbable))]
    public class GlueGrabbingCallback: MonoBehaviour
    {
        public GameObject horizontalGameObject;
        public GameObject verticalGameObject;

        private GeneralGrabbable _generalGrabbable;

        private void Start()
        {
            _generalGrabbable = gameObject.GetComponent<GeneralGrabbable>();
            
            _generalGrabbable.OnSelect += OnSelect;
            _generalGrabbable.OnUnselect += OnUnselect;
            
            // default
            horizontalGameObject.SetActive(false);
            verticalGameObject.SetActive(true);
        }

        private void OnSelect()
        {
            // enable horizontal object and disable vertical object to make it look real
            horizontalGameObject.SetActive(true);
            verticalGameObject.SetActive(false);
        }

        private void OnUnselect()
        {
            // restore it
            horizontalGameObject.SetActive(false);
            verticalGameObject.SetActive(true);
        }
    }
}