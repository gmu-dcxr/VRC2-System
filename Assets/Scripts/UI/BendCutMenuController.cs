using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRC2.Pipe;
using PipeBendAngles = VRC2.Pipe.PipeConstants.PipeBendAngles;
using PipeBendCutParameters = VRC2.Pipe.PipeConstants.PipeBendCutParameters;
using PipeMaterialColor = VRC2.Pipe.PipeConstants.PipeMaterialColor;
using PipeType = VRC2.Pipe.PipeConstants.PipeType;

namespace VRC2
{
    public class BendCutMenuController : MonoBehaviour
    {
        [Header("Root canvas")] public GameObject rootCanvas;

        [Header("UIHelper")] public GameObject UIHelper;

        [Header("Reticles")] public GameObject reticleLeft;
        public GameObject reticleRight;
        
        [Header("Angle Buttons")] public List<Button> buttons;

        [Header("Confirm/Reset")]
        public Button confirmButton;

        public Button resetButton;

        [Header("Length")] public InputField aInputField;

        public InputField bInputField;

        private PipeBendAngles _bendAngles = PipeBendAngles.Default;
        // action
        public System.Action OnConfirmed;
        
        // result
        private PipeBendCutParameters _parameters;

        private PipeMaterialColor _pipeColor
        {
            get
            {
                if (GlobalConstants.selectedPipe == null)
                {
                    return PipeMaterialColor.Default;
                }

                var pipe = GlobalConstants.selectedPipe;
                return pipe.GetComponent<PipeManipulation>().pipeColor;
            }
        }

        private PipeType _pipeType
        {
            get
            {
                if (GlobalConstants.selectedPipe == null)
                {
                    return PipeType.Default;
                }

                var pipe = GlobalConstants.selectedPipe;
                return pipe.GetComponent<PipeManipulation>().pipeType;
            }
        }

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

            _parameters.angle = PipeBendAngles.Default;
            _parameters.a = 0;
            _parameters.b = 0;
            _parameters.type = PipeType.Default;
            _parameters.color = PipeMaterialColor.Default;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Show()
        {
            // reset 
            OnReset();
            
            rootCanvas.SetActive(true);
            UIHelper.SetActive(true);
            reticleLeft.SetActive(false);
            reticleRight.SetActive(false);
        }

        public void Hide()
        {
            rootCanvas.SetActive(false);
            UIHelper.SetActive(false);
            reticleLeft.SetActive(true);
            reticleRight.SetActive(true);
        }

        void OnConfirm()
        {
            // validate
            float a = 0, b = 0;
            var aflag = float.TryParse(aInputField.text, out a);
            var bflag = float.TryParse(bInputField.text, out b);
            var angle = GetAngle();
            if (aflag && bflag && angle != PipeBendAngles.Default)
            {
                // set value
                _parameters.angle = angle;
                _parameters.a = a;
                _parameters.b = b;
                _parameters.type = _pipeType;
                _parameters.color = _pipeColor;
                // close window
                Hide();
                if (OnConfirmed != null)
                {
                    OnConfirmed();
                }
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

            return PipeBendAngles.Default;
        }

    }
}