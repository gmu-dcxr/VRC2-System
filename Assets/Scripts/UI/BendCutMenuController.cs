using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC2.Pipe;
using PipeBendAngles = VRC2.Pipe.PipeConstants.PipeBendAngles;
using PipeType = VRC2.Pipe.PipeConstants.PipeType;
using PipeDiameter = VRC2.Pipe.PipeConstants.PipeDiameter;

namespace VRC2
{
    public class BendCutMenuController : MonoBehaviour
    {
        [Header("Root canvas")] public GameObject rootCanvas;

        // [Header("UIHelper")] public GameObject UIHelper;

        [Header("Reticles")] public GameObject reticleLeft;
        public GameObject reticleRight;

        [Header("Angle Buttons")] public List<Button> buttons;

        [Header("Confirm/Reset")] public Button confirmButton;

        public Button resetButton;

        [Header("Length")] public InputField aInputField;

        public InputField bInputField;

        [Space(30)] [Header("New Design")] public bool newDesign = false;

        public List<Button> diameterButtons;
        public InputField pipeLength;
        public InputField pipeAmount;
        public InputField connectorAmount;
        public Button confirmButtonNew;
        public Button resetButtonNew;

        [Space(30)] [Header("Debug")] public bool enableDebug = false;
        public GameObject spawnedPipe;

        #region Input pipe UI

        public Sprite sprite;
        public Sprite magenta;
        public Sprite green;
        public Sprite yellow;
        public Sprite blue;

        public TextMeshProUGUI inputPipeInfo;
        public Image pipeColorImage;


        #endregion

        public bool IsNewDesign
        {
            get => newDesign;
        }

        private PipeBendAngles _bendAngles = PipeBendAngles.Default;

        // action
        public System.Action OnConfirmed;

        // result
        private PipeConstants.PipeParameters _parameters;

        private PipeConstants.PipeColor _pipeColor
        {
            get
            {
                if (GlobalConstants.lastSpawnedPipe == null)
                {
                    return PipeConstants.PipeColor.Default;
                }

                var pipe = GlobalConstants.lastSpawnedPipe;
                return pipe.GetComponent<PipeManipulation>().pipeColor;
            }
        }

        private PipeType _pipeType
        {
            get
            {
                if (GlobalConstants.lastSpawnedPipe == null)
                {
                    return PipeType.Default;
                }

                var pipe = GlobalConstants.lastSpawnedPipe;
                return pipe.GetComponent<PipeManipulation>().pipeType;
            }
        }

        public PipeConstants.PipeParameters result
        {
            get => _parameters;
        }

        public bool showing
        {
            get => rootCanvas.activeSelf;
        }

        // Start is called before the first frame update
        void Start()
        {
            _parameters.angle = PipeBendAngles.Default;
            _parameters.a = 0;
            _parameters.b = 0;
            _parameters.type = PipeType.Default;
            _parameters.color = PipeConstants.PipeColor.Default;

            // new design
            if (newDesign)
            {
                // add connector
                _parameters.connectorAmount = 0;
                _parameters.connectorDiamter = PipeDiameter.Default;

                confirmButtonNew.onClick.AddListener(OnConfirmNew);
                resetButtonNew.onClick.AddListener(OnResetNew);
            }
            else
            {
                // init events
                confirmButton.onClick.AddListener(OnConfirm);
                resetButton.onClick.AddListener(OnReset);
            }

            // debug mode
            if (enableDebug)
            {
                GlobalConstants.lastSpawnedPipe = spawnedPipe;
            }

            // hide on start
            Hide();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Awake()
        {
            UpdateInputPipe();
        }

        private void OnEnable()
        {
            UpdateInputPipe();
        }

        void SetInputField(InputField f, bool enable)
        {
            var ifd = f.gameObject.GetComponent<InputFieldDetection>();
            if (ifd != null)
            {
                ifd.enabled = enable;
            }

            var text = enable ? "Enter text..." : "Disabled";
            // update placeholder
            f.placeholder.GetComponent<Text>().text = text;
            f.enabled = enable;
        }

        void UpdateInputPipe()
        {
            pipeColorImage.sprite = sprite;
            var pipe = GlobalConstants.lastSpawnedPipe;
            if (pipe == null)
            {
                // reset
                inputPipeInfo.text = "";
                // disable length and amount
                SetInputField(pipeLength, false);
                SetInputField(pipeAmount, false);
            }
            else
            {
                SetInputField(pipeLength, true);
                SetInputField(pipeAmount, true);

                var pm = pipe.GetComponent<PipeManipulation>();
                var color = pm.pipeColor;
                var type = Enum.GetName(typeof(PipeType), pm.pipeType);
                var diameter = (int)pm.diameter + 1;
                var length = pm.segmentALength + pm.segmentBLength;
                // update color
                switch (color)
                {
                    case PipeConstants.PipeColor.Magenta:
                        pipeColorImage.sprite = magenta;
                        break;
                    case PipeConstants.PipeColor.Green:
                        pipeColorImage.sprite = green;
                        break;
                    case PipeConstants.PipeColor.Yellow:
                        pipeColorImage.sprite = yellow;
                        break;
                    case PipeConstants.PipeColor.Blue:
                        pipeColorImage.sprite = blue;
                        break;
                    default:
                        break;
                }

                // update info: Type (diameter, length')
                inputPipeInfo.text = $"{type} (ϕ{diameter}, {PipeHelper.FormatLength(length)}')";
            }
        }

        public void Show()
        {
            rootCanvas.SetActive(true);
            // GlobalConstants.SetLaserPointer(true);
            reticleLeft.SetActive(false);
            reticleRight.SetActive(false);
            UpdateInputPipe();
            // reset after shown
            OnReset();
        }

        public void Hide()
        {
            rootCanvas.SetActive(false);
            // GlobalConstants.SetLaserPointer(false);
            reticleLeft.SetActive(true);
            reticleRight.SetActive(true);
        }

        public void Refresh()
        {
            // do nothing if not visible
            if(!rootCanvas.activeSelf) return;
            UpdateInputPipe();
        }

        void OnConfirm()
        {
            // validate
            float a = 0, b = 0;
            var aflag = float.TryParse(aInputField.text, out a);
            var bflag = float.TryParse(bInputField.text, out b);
            var angle = GetAngle();
            // (a && angle == 0)  || (a && b && angle != 0)
            if ((aflag && angle == PipeBendAngles.Angle_0) || (aflag && bflag && angle != PipeBendAngles.Default))
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
            if (newDesign)
            {
                pipeLength.text = "";
                pipeAmount.text = "";
                connectorAmount.text = "";
            }
            else
            {
                // clear input
                aInputField.text = "";
                bInputField.text = "";
            }

            // reset material
            for (int i = 0; i < buttons.Count; i++)
            {
                var button = buttons[i];
                var bclm = button.gameObject.GetComponent<ButtonMaterialController>();
                if (bclm.isSelected)
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
                if (bclm.isSelected)
                {
                    return (PipeBendAngles)i;
                }
            }

            return PipeBendAngles.Default;
        }

        #region New Design

        PipeDiameter GetConnectorDiameter()
        {
            for (var i = 0; i < diameterButtons.Count; i++)
            {
                var bclm = diameterButtons[i].gameObject.GetComponent<ButtonMaterialController>();
                if (bclm.isSelected)
                {
                    return (PipeDiameter)i;
                }
            }

            return PipeDiameter.Default;
        }

        float GetStraightPipeLength()
        {
            try
            {
                return float.Parse(pipeLength.text);
            }
            catch (Exception e)
            {

            }

            return -1.0f;
        }

        int GetPipeAmount()
        {
            try
            {
                return int.Parse(pipeAmount.text);
            }
            catch (Exception e)
            {
            }

            // default is 0
            return 0;
        }

        int GetConnectorAmount()
        {
            try
            {
                return int.Parse(connectorAmount.text);
            }
            catch (Exception e)
            {
            }

            // default is 0
            return 0;
        }

        void OnConfirmNew()
        {
            // validate
            var length = GetStraightPipeLength();
            var amount = GetPipeAmount();
            var cDiameter = GetConnectorDiameter();
            var cAmount = GetConnectorAmount();

            float a = 0, b = 0;

            bool valid = false;

            if (length > 0 && amount > 0)
            {
                a = length / 2.0f;
                b = length / 2.0f;
                valid = true;
            }

            if (cDiameter != PipeDiameter.Default && cAmount > 0)
            {
                valid = true;
            }

            if (!valid)
            {
                Debug.LogWarning("Invalid selection for BendCut.");
                return;
            }

            // update parameters
            // always 0 
            _parameters.angle = PipeBendAngles.Angle_0;
            _parameters.a = a;
            _parameters.b = b;
            _parameters.type = _pipeType;
            _parameters.color = _pipeColor;
            // update amount
            _parameters.amount = amount;
            // update connector
            _parameters.connectorDiamter = cDiameter;
            _parameters.connectorAmount = cAmount;

            // close window
            Hide();
            if (OnConfirmed != null)
            {
                OnConfirmed();
            }
        }

        void OnResetNew()
        {
            ResetButtonsMaterial(diameterButtons);
            ResetInputField(pipeLength);
            ResetInputField(pipeAmount);
            ResetInputField(connectorAmount);
        }

        void ResetInputField(InputField field)
        {
            field.text = "";
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



        #endregion

    }
}