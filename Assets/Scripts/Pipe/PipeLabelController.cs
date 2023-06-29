using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VRC2
{

    [RequireComponent(typeof(PipeManipulation))]
    public class PipeLabelController : MonoBehaviour
    {
        [SerializeField] private GameObject label;
        public bool showWhenHover { set; get; }
        private TextMeshPro _textMeshPro;

        private PipeManipulation _pipeManipulation;


        // Start is called before the first frame update
        void Start()
        {
            _textMeshPro = label.GetComponent<TextMeshPro>();
            _pipeManipulation = gameObject.GetComponent<PipeManipulation>();
            UpdateLabel();
            // hide at the beginning
            Show(false);
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Show(bool flag)
        {
            if (showWhenHover)
            {
                label.SetActive(flag);
            }
            else
            {
                label.SetActive(false);
            }
        }

        void UpdateLabel()
        {
            var l = _pipeManipulation.pipeLength;
            var d = _pipeManipulation.diameter;
            // update text
            // _textMeshPro.text = $"Length: {l} diameter: {d}";
            _textMeshPro.text = $"Diameter: {d}";
        }
    }
}