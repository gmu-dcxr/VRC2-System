using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PipeBendAngles = VRC2.GlobalConstants.PipeBendAngles;
using PipeBendCutParameters = VRC2.GlobalConstants.PipeBendCutParameters;

namespace VRC2
{
    public class BendCutMenuController : MonoBehaviour
    {
        [Header("Root canvas")] public GameObject rootCanvas;
        [Header("Angle Buttons")] public List<Button> buttons;

        [Header("Confirm/Reset")]
        public Button confirmButton;

        public Button resetButton;

        [Header("Length")] public InputField aInputField;

        public InputField bInputField;

        private PipeBendAngles _bendAngles = PipeBendAngles.Empty;
        
        // result
        private PipeBendCutParameters _parameters;

        public PipeBendCutParameters result
        {
            get => _parameters;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            // init events
            confirmButton.onClick.AddListener(OnConfirm);
            resetButton.onClick.AddListener(OnReset);

            _parameters.angle = PipeBendAngles.Empty;
            _parameters.a = 0;
            _parameters.b = 0;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnConfirm()
        {
            // validate
            float a = 0, b = 0;
            var aflag = float.TryParse(aInputField.text, out a);
            var bflag = float.TryParse(bInputField.text, out b);
            var angle = GetAngle();
            if (aflag && bflag && angle != PipeBendAngles.Empty)
            {
                // set value
                _parameters.angle = angle;
                _parameters.a = a;
                _parameters.b = b;
                // close window
                rootCanvas.SetActive(false);
            }
        }

        void OnReset()
        {
            // clear input
            aInputField.text = "";
            bInputField.text = "";
            // reset material
            for (int i = 0; i < buttons.Count; i++)
            {
                var button = buttons[i];
                var bclm = button.gameObject.GetComponent<ButtonMaterialController>();
                if (bclm.currentMaterial == bclm.selectedMaterial)
                {
                    bclm.ChangeMaterial();
                }
            }
        }

        PipeBendAngles GetAngle()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                var button = buttons[i];
                var bclm = button.gameObject.GetComponent<ButtonMaterialController>();
                if (bclm.currentMaterial == bclm.selectedMaterial)
                {
                    return (PipeBendAngles)i;
                }
            }

            return PipeBendAngles.Empty;
        }

    }
}