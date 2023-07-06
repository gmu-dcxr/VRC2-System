using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VRC2
{

    public class ButtonMaterialController : MonoBehaviour
    {
        [Header("Appearance")] public Material defaultMaterial;

        public Material selectedMaterial;

        private Button _button;
        
        // Start is called before the first frame update
        void Start()
        {
            _button = gameObject.GetComponent<Button>();
            // bind event
            _button.onClick.AddListener(ChangeMaterial);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void ChangeMaterial()
        {
            // get button image
            var image = gameObject.GetComponent<Image>();

            if (image.material == selectedMaterial)
            {
                image.material = defaultMaterial;
            }
            else
            {
                image.material = selectedMaterial;
            }
        }
        
    }
}