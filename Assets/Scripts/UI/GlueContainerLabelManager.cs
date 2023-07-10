using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VRC2
{
    public class GlueContainerLabelManager : MonoBehaviour
    {
        private TextMeshPro _textMeshPro;
        // Start is called before the first frame update
        void Start()
        {
            _textMeshPro = gameObject.GetComponentInChildren<TextMeshPro>(true);
        }

        // Update is called once per frame
        void Update()
        {
            // update the label
            _textMeshPro.text = $"Glue: {GlobalConstants.currentGlueCapacitiy * 100}%";
        }
    }
}