using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using VRC2.Pipe;
using PipeBendAngles = VRC2.Pipe.PipeConstants.PipeBendAngles;
using PipeType = VRC2.Pipe.PipeConstants.PipeType;
using PipeDiameter = VRC2.Pipe.PipeConstants.PipeDiameter;


namespace VRC2
{
    public class AIDroneMenuController : MonoBehaviour
    {
        [Header("Root canvas")] public GameObject rootCanvas;

        // [Header("UIHelper")] public GameObject UIHelper;

        [Header("Reticles")] public GameObject reticleLeft;
        public GameObject reticleRight;

        [Header("Buttons")] [Tooltip("In the order of sewage, gas, water, and electrical wire")]
        public List<Button> pipeTypeButtons;

        [Tooltip("In the order of Magenta, Yellow, Green, and Blue")]
        public List<Button> pipeColorButtons;

        [Tooltip("In the order of diameter 1, 2, 3, and 4")]
        public List<Button> pipeDiameterButtons;

        [Header("Amount")] public InputField amountInputField;

        [Header("Confirm/Reset")] public Button confirmButton;

        public Button resetButton;

        // action
        public System.Action OnConfirmed;

        // reuse the same parameter as bend/cut
        private PipeConstants.PipeParameters _parameters;

        public PipeConstants.PipeParameters result
        {
            get => _parameters;
        }

        private void Start()
        {
            confirmButton.onClick.AddListener(OnConfirm);
            resetButton.onClick.AddListener(OnReset);

            // hide at the beginning
            Hide();
        }

        void OnConfirm()
        {
            // validate first
            var color = GetPipeColor();
            var type = GetPipeType();
            var diameter = GetPipeDiameter();

            var amount = GetAmount();

            if (color != PipeConstants.PipeColor.Default && type != PipeType.Default &&
                diameter != PipeDiameter.Default && amount > 0)
            {
                _parameters.type = type;
                _parameters.color = color;
                _parameters.diameter = diameter;
                // set other default values
                _parameters.a = 0;
                _parameters.b = 0;
                _parameters.angle = PipeBendAngles.Default;
                // add amount
                _parameters.amount = amount;

                // close window
                Hide();
                if (OnConfirmed != null)
                {
                    OnConfirmed();
                }
            }

        }

        void ResetButtonsMaterial(List<Button> buttons)
        {
            foreach (var btn in buttons)
            {
                var bclm = btn.gameObject.GetComponent<ButtonMaterialController>();
                if (bclm.isSelected)
                {
                    bclm.ChangeMaterial();
                }
            }
        }


        void OnReset()
        {
            ResetButtonsMaterial(pipeTypeButtons);
            ResetButtonsMaterial(pipeColorButtons);
            ResetButtonsMaterial(pipeDiameterButtons);
            ResetAmount();
        }

        void ResetAmount()
        {
            amountInputField.text = "";
        }

        public void Show()
        {
            // reset 
            OnReset();

            rootCanvas.SetActive(true);
            GlobalConstants.SetLaserPointer(true);
            reticleLeft.SetActive(false);
            reticleRight.SetActive(false);
        }

        public bool showing
        {
            get => rootCanvas.activeSelf;
        }

        public void Hide()
        {
            rootCanvas.SetActive(false);
            GlobalConstants.SetLaserPointer(false);
            reticleLeft.SetActive(true);
            reticleRight.SetActive(true);
        }

        PipeType GetPipeType()
        {
            for (var i = 0; i < pipeTypeButtons.Count; i++)
            {
                var bclm = pipeTypeButtons[i].gameObject.GetComponent<ButtonMaterialController>();
                if (bclm.isSelected)
                {
                    return (PipeType)i;
                }
            }

            return PipeType.Default;
        }

        PipeConstants.PipeColor GetPipeColor()
        {
            for (var i = 0; i < pipeColorButtons.Count; i++)
            {
                var bclm = pipeColorButtons[i].gameObject.GetComponent<ButtonMaterialController>();
                if (bclm.isSelected)
                {
                    return (PipeConstants.PipeColor)i;
                }
            }

            return PipeConstants.PipeColor.Default;
        }

        PipeDiameter GetPipeDiameter()
        {
            for (var i = 0; i < pipeDiameterButtons.Count; i++)
            {
                var bclm = pipeDiameterButtons[i].gameObject.GetComponent<ButtonMaterialController>();
                if (bclm.isSelected)
                {
                    return (PipeDiameter)i;
                }
            }

            return PipeDiameter.Default;
        }

        int GetAmount()
        {
            try
            {
                return (int)float.Parse(amountInputField.text);
            }
            catch (Exception e)
            {

            }

            return 0;
        }
    }
}