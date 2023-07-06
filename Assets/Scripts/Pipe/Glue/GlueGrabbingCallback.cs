using System;
using UnityEngine;

namespace VRC2.Events
{
    public class GlueGrabbingCallback: MonoBehaviour
    {
        public GameObject horizontalGameObject;
        public GameObject verticalGameObject;

        public void OnSelect()
        {
            // enable horizontal object and disable vertical object to make it look real
            horizontalGameObject.SetActive(true);
            verticalGameObject.SetActive(false);
        }

        public void OnUnselect()
        {
            // restore it
            horizontalGameObject.SetActive(false);
            verticalGameObject.SetActive(true);
        }
    }
}