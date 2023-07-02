using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VRC2.Events;

namespace VRC2
{
    [RequireComponent(typeof(GeneralGrabber))]
    public class HoveringLabelManager : MonoBehaviour
    {
        private TextMeshPro _textMeshPro;
        private GeneralGrabber _generalGrabber;

        private string _label;

        public string label
        {
            get => _label;
            set => _label = value;
        }

        // Start is called before the first frame update
        public void Start()
        {
            Debug.Log("HoveringLabel Manager");
            _generalGrabber = gameObject.GetComponent<GeneralGrabber>();

            _generalGrabber.OnHover += OnHover;
            _generalGrabber.OnUnhover += OnUnhover;

            // find lable gameobject in children
            _textMeshPro = gameObject.GetComponentInChildren<TextMeshPro>(true);

            if (_textMeshPro == null)
            {
                Debug.LogError("Not found TextMeshPro, did you forget to add one 3D Text child?");
            }


            // set empty when start
            _textMeshPro.text = "";
        }

        private void OnUnhover()
        {
            Debug.Log("OnUnhover");
            _textMeshPro.text = "";
        }

        private void OnHover()
        {
            Debug.Log("OnHover");
            _textMeshPro.text = label;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}