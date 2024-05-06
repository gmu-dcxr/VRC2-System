using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRC2.ScenariosV2.Tool;

namespace VRC2
{

    public class ButtonMaterialController : MonoBehaviour
    {
        [ReadOnly] public Color selectedColor = Color.white;

        [Header("Image-Based")] public bool imageBased = false;
        public Sprite selectedImage;
        private Sprite originalImage;

        private Color originalColor;

        private Button _button;

        private Image _image;

        // if it's being selected
        public bool isSelected
        {
            get
            {
                if (imageBased)
                {
                    return _image.sprite == selectedImage;
                }
                else
                {
                    return _image.color == selectedColor;
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            // bind event
            _button.onClick.AddListener(ChangeMaterial);

            _image = GetComponent<Image>();
            originalColor = _image.color;

            originalImage = _image.sprite;
        }

        // Update is called once per frame
        void Update()
        {

        }

        // change material by changing color
        public void ChangeMaterial()
        {
            if (imageBased)
            {
                if (_image.sprite != selectedImage)
                {
                    _image.sprite = selectedImage;
                }
                else
                {
                    _image.sprite = originalImage;
                }
            }
            else
            {
                // color based: only change color
                if (_image.color != selectedColor)
                {
                    _image.color = selectedColor;
                }
                else
                {
                    _image.color = originalColor;
                }
            }
        }
    }
}