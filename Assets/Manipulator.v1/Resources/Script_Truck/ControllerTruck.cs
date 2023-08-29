using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerTruck : MonoBehaviour
{
	[Header("If provided FPSController")] public KeyCode exitEnterCar_Key = KeyCode.Tab;
	public Transform m_fPSController;
	private bool exitEnterCar_Bool = true;
	[HideInInspector] public bool OnFps = false;
	[HideInInspector] public bool fps_Bool = false;
	[HideInInspector] public Rigidbody rig;
	[HideInInspector] public bool startTruck = false;

	[HideInInspector] public Transform centerOffMass;

	//Wheel Transform
	[HideInInspector] public Transform wheelTransformFL;
	[HideInInspector] public Transform wheelTransformFR;
	[HideInInspector] public Transform wheelTransformBL_For;
	[HideInInspector] public Transform wheelTransformBR_For;
	[HideInInspector] public Transform wheelTransformBL_Back;

	[HideInInspector] public Transform wheelTransformBR_Back;

	//Wheel Colleder
	[HideInInspector] public WheelCollider wheelCol_FL;
	[HideInInspector] public WheelCollider wheelCol_FR;
	[HideInInspector] public WheelCollider wheelCol_BL_For;
	[HideInInspector] public WheelCollider wheelCol_BR_For;
	[HideInInspector] public WheelCollider wheelCol_BL_Back;
	[HideInInspector] public WheelCollider wheelCol_BR_Back;
	[Header("Motor")] public float speedEngine = 200f;
	public float maxSpeedEngine = 20f;
	private float m_Vertical = 0f;
	public KeyCode lowGear = KeyCode.LeftShift;
	public float lowGearPower = 890f;
	private int speedI = 0;
	private bool lowGear_Bool = true;
	public float massTruck = 1600;
	private float checkRevers = 0;
	[Header("Steering Angle")] public float steer = 33f;
	private float m_Horizontal = 0f;
	[Header("Brace")] public float brace = 650f;
	public float coastBraking = 320f;
	public float inertiaBrake = 430f;
	[Header("Hand Brake")] public KeyCode parkingBrake = KeyCode.Space;
	public float parkingBrakePower = 1000f;
	[HideInInspector] public bool parkingBrake_Bool = true;
	[Header("Sound")] public float volumeEngine = 0.544f;
	public float volumeRevers = 0.560f;
	public float volumeLight = 1f;
	[HideInInspector] public AudioSource s_Engine;
	[HideInInspector] public float pitchSoundTruck = 0.6f;
	private float maxEngineRPM = 6000.0f;
	private float minEngineRPM = 1000.0f;
	private float engineRPM = 0f;
	private float gearShiftRate = 12.0f;
	private int currentGear = 0;
	[HideInInspector] public AudioSource s_Revers;
	[HideInInspector] public AudioSource s_Turn;
	[HideInInspector] public AudioSource s_StartStop;
	private AudioClip S_start;
	private AudioClip S_stop;
	[HideInInspector] public Light revers_L;
	[HideInInspector] public Light revers_R;
	[HideInInspector] public Light stop_L;
	[HideInInspector] public Light stop_R;
	[HideInInspector] public Light turnSignal_L;
	[HideInInspector] public Light turnSignal_R;
	[HideInInspector] public Light turnSignalPanel_L;
	[HideInInspector] public Light turnSignalPanel_R;
	public KeyCode leftTurn = KeyCode.Q;
	public KeyCode RightTurn = KeyCode.E;
	public KeyCode turnSignal = KeyCode.G;
	[HideInInspector] public bool leftTurn_Bool = true;
	[HideInInspector] public bool RightTurn_Bool = true;
	[HideInInspector] public bool turnSignal_Bool = true;
	[Header("Manipulator")] public KeyCode powrManipulator = KeyCode.F;
	public KeyCode rotationArrowLeft = KeyCode.A;
	public KeyCode rotationArrowRight = KeyCode.D;
	public KeyCode forwardArrow = KeyCode.Q;
	public KeyCode backArrow = KeyCode.E;
	public KeyCode UpArrow = KeyCode.W;
	public KeyCode DownArrow = KeyCode.S;
	public KeyCode hook = KeyCode.LeftShift;
	public KeyCode DownHook = KeyCode.Mouse0;
	public KeyCode UpHook = KeyCode.Mouse1;
	public float speedForward = 0.3f;
	public float speedRotation = 8.2f;
	public float speedHook = 0.3f;
	public float speedUpArrow = 5f;
	[HideInInspector] public bool blockOnManip = true;
	[HideInInspector] public bool blockOnManip_Cargo = true;
	[HideInInspector] public bool blockManip_Bool = false;
	[Header("Lift Truck Manipulator")] public KeyCode horizontalLiftTruck = KeyCode.Alpha1;
	public KeyCode verticalLiftTruck = KeyCode.Alpha2;
	public KeyCode forwardLift = KeyCode.Mouse0;
	public KeyCode backLift = KeyCode.Mouse1;
	public float speedLiftTruck = 0.17f;
	[HideInInspector] public bool onManip = true;
	[HideInInspector] public Canvas truckCanvas;
	[HideInInspector] public Canvas EnterCanvas;
	public Image connectedCargoIm;
	[HideInInspector] public Image EnterExitCar_Im;
	private Image panelTruckQ1;
	private Image panelTruckQ2;
	private Image panelTruckQ3;
	private Image panelTruck_TurnS_L;
	private Image panelTruck_TurnS_R;
	private Image panelTruck_G_ABS;
	private Image panelTruck_G_Handbrake;
	private Image panelTruck_G_OD;
	private Image panelTruck_G_OnManip;
	private Image panelTruck_Q_Tachometer;
	private float tachometer_Float = 0.17f;
	private Image panel_Q_Fuel;
	private Image panel_Q_Temperature;
	private Text speedCar_Text;
	[HideInInspector] public Crable m_ScriptCrable;
	[HideInInspector] public HookManip m_ScriptHook;

	// hack
	private bool craneTriggered = false;

	void Start()
	{
		if (m_fPSController != null)
		{
			fps_Bool = true;
			transform.GetChild(62).gameObject.SetActive(false);
			var canEnter = new GameObject();
			canEnter.name = "Canvas EnterExitCar";
			canEnter.AddComponent<Canvas>();
			canEnter.AddComponent<CanvasScaler>();
			canEnter.AddComponent<GraphicRaycaster>();
			EnterCanvas = canEnter.GetComponent<Canvas>();
			EnterCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			var imCargoEnter = new GameObject();
			imCargoEnter.name = "EnterExitCar";
			imCargoEnter.AddComponent<Image>();
			imCargoEnter.transform.SetParent(EnterCanvas.transform, false);
			EnterExitCar_Im = imCargoEnter.GetComponent<Image>();
			EnterExitCar_Im.sprite = Resources.Load<Sprite>("UI_Truck/EnterCar");
			EnterExitCar_Im.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 58.4f);
			EnterExitCar_Im.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
			EnterExitCar_Im.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
			EnterExitCar_Im.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0);
			EnterExitCar_Im.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			EnterExitCar_Im.enabled = false;
			S_start = Resources.Load("Sound/Start_DV", typeof(AudioClip)) as AudioClip;
			S_stop = Resources.Load("Sound/Stop_DV", typeof(AudioClip)) as AudioClip;
		}

		// hack
		startTruck = true;
		
		if (startTruck == true)
		{
			rig = gameObject.GetComponent<Rigidbody>();
			if (rig == null)
			{
				rig = gameObject.AddComponent<Rigidbody>();
			}
			rig.mass = massTruck;
			
			if (fps_Bool == false)
			{
				s_Engine.Play();
				s_Revers.Play();
			}

			s_Engine.pitch = 0.69f;
			s_Engine.volume = volumeEngine;
			s_Revers.volume = volumeRevers;
			//Add Canvas
			var can = new GameObject();
			can.name = "Canvas Truck";
			can.AddComponent<Canvas>();
			can.AddComponent<CanvasScaler>();
			can.AddComponent<GraphicRaycaster>();
			truckCanvas = can.GetComponent<Canvas>();
			truckCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			if (fps_Bool == true)
			{
				truckCanvas.enabled = false;
			}
			// hack
			truckCanvas.enabled = false;

			//Image Cargo
			var imCargo = new GameObject();
			imCargo.name = "ConnectedCargo";
			imCargo.AddComponent<Image>();
			connectedCargoIm = imCargo.GetComponent<Image>();
			imCargo.transform.SetParent(truckCanvas.transform, false);
			connectedCargoIm.sprite = Resources.Load<Sprite>("UI_Truck/Cargo");
			connectedCargoIm.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 57.6f);
			connectedCargoIm.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
			connectedCargoIm.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
			connectedCargoIm.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0);
			connectedCargoIm.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			connectedCargoIm.enabled = false;
			//Panel Truck Q1
			var inPanelQ1 = new GameObject();
			inPanelQ1.name = "Panel_Q1";
			inPanelQ1.AddComponent<Image>();
			panelTruckQ1 = inPanelQ1.GetComponent<Image>();
			inPanelQ1.transform.SetParent(truckCanvas.transform, false);
			panelTruckQ1.sprite = Resources.Load<Sprite>("UI_Truck/Panel_Q1");
			panelTruckQ1.GetComponent<RectTransform>().anchoredPosition = new Vector2(-99.26f, 99.4f);
			panelTruckQ1.GetComponent<RectTransform>().sizeDelta = new Vector2(171.05f, 171.05f);
			panelTruckQ1.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
			panelTruckQ1.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
			panelTruckQ1.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panelTruckQ1.GetComponent<Image>().color = new Color32(255, 255, 255, 141);
			//Panel Truck Q2
			var inPanelQ2 = new GameObject();
			inPanelQ2.name = "Panel_Q2";
			inPanelQ2.AddComponent<Image>();
			panelTruckQ2 = inPanelQ2.GetComponent<Image>();
			inPanelQ2.transform.SetParent(truckCanvas.transform, false);
			panelTruckQ2.sprite = Resources.Load<Sprite>("UI_Truck/Panel_Q2");
			panelTruckQ2.GetComponent<RectTransform>().anchoredPosition = new Vector2(-99.26f, 99.4f);
			panelTruckQ2.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 180);
			panelTruckQ2.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
			panelTruckQ2.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
			panelTruckQ2.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panelTruckQ2.GetComponent<Image>().color = new Color32(255, 255, 255, 219);
			//PanelTruck_Q_Tachometer
			var inPanelT = new GameObject();
			inPanelT.name = "Panel_Q_Tachometer";
			inPanelT.AddComponent<Image>();
			panelTruck_Q_Tachometer = inPanelT.GetComponent<Image>();
			inPanelT.transform.SetParent(truckCanvas.transform, false);
			panelTruck_Q_Tachometer.sprite = Resources.Load<Sprite>("UI_Truck/Panel_Q_Tachometer");
			panelTruck_Q_Tachometer.GetComponent<RectTransform>().anchoredPosition = new Vector2(-99.26f, 99.4f);
			panelTruck_Q_Tachometer.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 180);
			panelTruck_Q_Tachometer.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
			panelTruck_Q_Tachometer.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
			panelTruck_Q_Tachometer.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panelTruck_Q_Tachometer.GetComponent<Image>().color = new Color32(30, 255, 0, 165);
			panelTruck_Q_Tachometer.GetComponent<Image>().type = Image.Type.Filled;
			panelTruck_Q_Tachometer.GetComponent<Image>().fillAmount = tachometer_Float;
			//Panel_Q_Fuel
			var inPanelF = new GameObject();
			inPanelF.name = "Panel_Q_Fuel";
			inPanelF.AddComponent<Image>();
			panel_Q_Fuel = inPanelF.GetComponent<Image>();
			inPanelF.transform.SetParent(truckCanvas.transform, false);
			panel_Q_Fuel.sprite = Resources.Load<Sprite>("UI_Truck/Panel_Q_Fuel");
			panel_Q_Fuel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-99.26f, 99.4f);
			panel_Q_Fuel.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 180);
			panel_Q_Fuel.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
			panel_Q_Fuel.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
			panel_Q_Fuel.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panel_Q_Fuel.GetComponent<Image>().color = new Color32(255, 193, 30, 165);
			panel_Q_Fuel.GetComponent<Image>().type = Image.Type.Filled;
			panel_Q_Fuel.GetComponent<Image>().fillMethod = Image.FillMethod.Vertical;
			panel_Q_Fuel.GetComponent<Image>().fillAmount = 1;
			//Panel_Q_Temperature
			var inPanelTm = new GameObject();
			inPanelTm.name = "Panel_Q_Temperature";
			inPanelTm.AddComponent<Image>();
			panel_Q_Temperature = inPanelTm.GetComponent<Image>();
			inPanelTm.transform.SetParent(truckCanvas.transform, false);
			panel_Q_Temperature.sprite = Resources.Load<Sprite>("UI_Truck/Panel_Q_Temperature");
			panel_Q_Temperature.GetComponent<RectTransform>().anchoredPosition = new Vector2(-99.26f, 99.4f);
			panel_Q_Temperature.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 180);
			panel_Q_Temperature.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
			panel_Q_Temperature.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
			panel_Q_Temperature.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panel_Q_Temperature.GetComponent<Image>().color = new Color32(255, 193, 30, 165);
			panel_Q_Temperature.GetComponent<Image>().type = Image.Type.Filled;
			panel_Q_Temperature.GetComponent<Image>().fillMethod = Image.FillMethod.Vertical;
			panel_Q_Temperature.GetComponent<Image>().fillAmount = 0.332f;
			//Panel Truck Q3
			var inPanelQ3 = new GameObject();
			inPanelQ3.name = "Panel_Q3";
			inPanelQ3.AddComponent<Image>();
			panelTruckQ3 = inPanelQ3.GetComponent<Image>();
			inPanelQ3.transform.SetParent(truckCanvas.transform, false);
			panelTruckQ3.sprite = Resources.Load<Sprite>("UI_Truck/Panel_Q3");
			panelTruckQ3.GetComponent<RectTransform>().anchoredPosition = new Vector2(-99.26f, 99.4f);
			panelTruckQ3.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 180);
			panelTruckQ3.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
			panelTruckQ3.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
			panelTruckQ3.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panelTruckQ3.GetComponent<Image>().color = new Color32(255, 255, 255, 184);
			//PanelTruck_TurnS_L
			var inPanelTL = new GameObject();
			inPanelTL.name = "Panel_G_TurnSignal_L";
			inPanelTL.AddComponent<Image>();
			panelTruck_TurnS_L = inPanelTL.GetComponent<Image>();
			inPanelTL.transform.SetParent(truckCanvas.transform, false);
			panelTruck_TurnS_L.sprite = Resources.Load<Sprite>("UI_Truck/Panel_G_TurnSignal");
			panelTruck_TurnS_L.GetComponent<RectTransform>().anchoredPosition = new Vector2(-134.6f, 68);
			panelTruck_TurnS_L.GetComponent<RectTransform>().sizeDelta = new Vector2(27, 27);
			panelTruck_TurnS_L.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
			panelTruck_TurnS_L.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
			panelTruck_TurnS_L.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panelTruck_TurnS_L.GetComponent<Image>().color = new Color32(68, 68, 68, 255);
			//PanelTruck_TurnS_R
			var inPanelTR = new GameObject();
			inPanelTR.name = "Panel_G_TurnSignal_R";
			inPanelTR.AddComponent<Image>();
			panelTruck_TurnS_R = inPanelTR.GetComponent<Image>();
			inPanelTR.transform.SetParent(truckCanvas.transform, false);
			panelTruck_TurnS_R.sprite = Resources.Load<Sprite>("UI_Truck/Panel_G_TurnSignal");
			panelTruck_TurnS_R.GetComponent<RectTransform>().anchoredPosition = new Vector2(-63.25f, 68);
			panelTruck_TurnS_R.GetComponent<RectTransform>().sizeDelta = new Vector2(27, 27);
			panelTruck_TurnS_R.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
			panelTruck_TurnS_R.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
			panelTruck_TurnS_R.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panelTruck_TurnS_R.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);
			panelTruck_TurnS_R.GetComponent<Image>().color = new Color32(68, 68, 68, 255);
			//PanelTruck_G_ABS
			var inPanelABS = new GameObject();
			inPanelABS.name = "Panel_G_ABS";
			inPanelABS.AddComponent<Image>();
			panelTruck_G_ABS = inPanelABS.GetComponent<Image>();
			inPanelABS.transform.SetParent(truckCanvas.transform, false);
			panelTruck_G_ABS.sprite = Resources.Load<Sprite>("UI_Truck/Panel_G_ABS");
			panelTruck_G_ABS.GetComponent<RectTransform>().anchoredPosition = new Vector2(-42.6f, 98.3f);
			panelTruck_G_ABS.GetComponent<RectTransform>().sizeDelta = new Vector2(34, 34);
			panelTruck_G_ABS.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
			panelTruck_G_ABS.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
			panelTruck_G_ABS.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panelTruck_G_ABS.GetComponent<Image>().color = new Color32(86, 86, 86, 163);
			//PanelTruck_G_Handbrake
			var inPanelH = new GameObject();
			inPanelH.name = "Panel_G_Handbrake";
			inPanelH.AddComponent<Image>();
			panelTruck_G_Handbrake = inPanelH.GetComponent<Image>();
			inPanelH.transform.SetParent(truckCanvas.transform, false);
			panelTruck_G_Handbrake.sprite = Resources.Load<Sprite>("UI_Truck/Panel_G_Handbrake");
			panelTruck_G_Handbrake.GetComponent<RectTransform>().anchoredPosition = new Vector2(-114.9f, 98.3f);
			panelTruck_G_Handbrake.GetComponent<RectTransform>().sizeDelta = new Vector2(34, 34);
			panelTruck_G_Handbrake.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
			panelTruck_G_Handbrake.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
			panelTruck_G_Handbrake.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panelTruck_G_Handbrake.GetComponent<Image>().color = new Color32(86, 86, 86, 163);
			//PanelTruck_G_OD
			var inPanelOD = new GameObject();
			inPanelOD.name = "Panel_G_OD";
			inPanelOD.AddComponent<Image>();
			panelTruck_G_OD = inPanelOD.GetComponent<Image>();
			inPanelOD.transform.SetParent(truckCanvas.transform, false);
			panelTruck_G_OD.sprite = Resources.Load<Sprite>("UI_Truck/Panel_G_OD");
			panelTruck_G_OD.GetComponent<RectTransform>().anchoredPosition = new Vector2(-149.3f, 98.3f);
			panelTruck_G_OD.GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
			panelTruck_G_OD.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
			panelTruck_G_OD.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
			panelTruck_G_OD.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panelTruck_G_OD.GetComponent<Image>().color = new Color32(86, 86, 86, 163);
			//PanelTruck_G_OnManip
			var inPanelM = new GameObject();
			inPanelM.name = "Panel_G_OnManip";
			inPanelM.AddComponent<Image>();
			panelTruck_G_OnManip = inPanelM.GetComponent<Image>();
			inPanelM.transform.SetParent(truckCanvas.transform, false);
			panelTruck_G_OnManip.sprite = Resources.Load<Sprite>("UI_Truck/Panel_G_OnManip");
			panelTruck_G_OnManip.GetComponent<RectTransform>().anchoredPosition = new Vector2(-77.4f, 98.3f);
			panelTruck_G_OnManip.GetComponent<RectTransform>().sizeDelta = new Vector2(34, 34);
			panelTruck_G_OnManip.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
			panelTruck_G_OnManip.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
			panelTruck_G_OnManip.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panelTruck_G_OnManip.GetComponent<Image>().color = new Color32(86, 86, 86, 163);
			//Text
			var inPanelTe = new GameObject();
			inPanelTe.name = "Panel_Q_Text";
			inPanelTe.AddComponent<Text>();
			speedCar_Text = inPanelTe.GetComponent<Text>();
			speedCar_Text.transform.SetParent(truckCanvas.transform, false);
			speedCar_Text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			speedCar_Text.GetComponent<RectTransform>().anchoredPosition = new Vector2(-25.07f, -4.3f);
			speedCar_Text.GetComponent<RectTransform>().sizeDelta = new Vector2(171.05f, 171.05f);
			speedCar_Text.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
			speedCar_Text.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
			speedCar_Text.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			speedCar_Text.GetComponent<Text>().color = new Color32(0, 0, 0, 255);
			speedCar_Text.GetComponent<Text>().fontSize = 23;
		}
	}

	public void TriggerCrane()
	{
		if (craneTriggered) return;
		// hack
		pitchSoundTruck = 0.73f;
		if (truckCanvas != null)
		{
			panelTruck_G_OnManip.GetComponent<Image>().color = new Color32(255, 193, 30, 165);
		}

		if (m_ScriptCrable != null)
		{
			m_ScriptCrable.AddCrable();
		}

		m_ScriptHook.HookController();
		onManip = false;
		craneTriggered = true;
	}

	void Update()
	{
		if (Input.GetKeyDown(exitEnterCar_Key) && exitEnterCar_Bool == true && startTruck == true && OnFps == true)
		{
			EnterExitCar_Im.enabled = false;
			StartCoroutine("StartCar");
			exitEnterCar_Bool = false;
		}
		else if (Input.GetKeyDown(exitEnterCar_Key) && exitEnterCar_Bool == false)
		{
			StartCoroutine("StopCar");
			exitEnterCar_Bool = true;
		}

		if (fps_Bool == false)
		{
			if (startTruck == true)
			{
				if (Input.GetKeyDown(powrManipulator) && onManip == true && blockManip_Bool == true && speedI == 0)
				{
					pitchSoundTruck = 0.73f;
					if (truckCanvas != null)
					{
						panelTruck_G_OnManip.GetComponent<Image>().color = new Color32(255, 193, 30, 165);
					}

					if (m_ScriptCrable != null)
					{
						m_ScriptCrable.AddCrable();
					}

					m_ScriptHook.HookController();
					onManip = false;
				}
				else if (Input.GetKeyDown(powrManipulator) && onManip == false && blockOnManip == true &&
				         blockOnManip_Cargo == true)
				{
					pitchSoundTruck = 0.6f;
					if (truckCanvas != null)
					{
						panelTruck_G_OnManip.GetComponent<Image>().color = new Color32(86, 86, 86, 163);
					}

					if (m_ScriptCrable != null)
					{
						m_ScriptCrable.AddCrable();
					}

					m_ScriptHook.HookController();
					onManip = true;
				}

				if (onManip == true)
				{
					// hack
					return;
					
					m_Vertical = Input.GetAxis("Vertical");
					m_Horizontal = Input.GetAxis("Horizontal");
					if (Input.GetKeyDown(lowGear))
					{
						lowGear_Bool = false;
						if (truckCanvas != null)
						{
							panelTruck_G_OD.GetComponent<Image>().color = new Color32(255, 193, 30, 165);
						}
					}
					else if (Input.GetKeyUp(lowGear))
					{
						lowGear_Bool = true;
						if (truckCanvas != null)
						{
							panelTruck_G_OD.GetComponent<Image>().color = new Color32(86, 86, 86, 163);
							;
						}
					}

					Motor();
					SteerTruck();
					Brake();
					Revers();
					Tachometer();
					//Turn Signal
					if (Input.GetKeyDown(turnSignal) && turnSignal_Bool == true)
					{
						if (leftTurn_Bool == false)
						{
							TurnGignalLeftOn();
						}
						else if (RightTurn_Bool == false)
						{
							TurnGignalRightOn();
						}

						if (truckCanvas != null)
						{
							panelTruck_TurnS_L.GetComponent<Image>().color = new Color32(68, 68, 68, 255);
							panelTruck_TurnS_R.GetComponent<Image>().color = new Color32(68, 68, 68, 255);
						}

						TurnGignalOn();
					}
					else if (Input.GetKeyDown(turnSignal) && turnSignal_Bool == false)
					{
						TurnGignalOn();
						if (truckCanvas != null)
						{
							panelTruck_TurnS_L.GetComponent<Image>().color = new Color32(68, 68, 68, 255);
							panelTruck_TurnS_R.GetComponent<Image>().color = new Color32(68, 68, 68, 255);
						}
					}

					//Turn Signal Left
					if (Input.GetKeyDown(leftTurn) && leftTurn_Bool == true)
					{
						if (turnSignal_Bool == false)
						{
							TurnGignalOn();
						}
						else if (RightTurn_Bool == false)
						{
							TurnGignalRightOn();
						}

						if (truckCanvas != null)
						{
							panelTruck_TurnS_R.GetComponent<Image>().color = new Color32(68, 68, 68, 255);
						}

						TurnGignalLeftOn();
					}
					else if (Input.GetKeyDown(leftTurn) && leftTurn_Bool == false)
					{
						TurnGignalLeftOn();
						if (truckCanvas != null)
						{
							panelTruck_TurnS_L.GetComponent<Image>().color = new Color32(68, 68, 68, 255);
						}
					}

					//Turn Signal Right
					if (Input.GetKeyDown(RightTurn) && RightTurn_Bool == true)
					{
						if (turnSignal_Bool == false)
						{
							TurnGignalOn();
						}
						else if (leftTurn_Bool == false)
						{
							TurnGignalLeftOn();
						}

						if (truckCanvas != null)
						{
							panelTruck_TurnS_L.GetComponent<Image>().color = new Color32(68, 68, 68, 255);
						}

						TurnGignalRightOn();
					}
					else if (Input.GetKeyDown(RightTurn) && RightTurn_Bool == false)
					{
						TurnGignalRightOn();
						if (truckCanvas != null)
						{
							panelTruck_TurnS_R.GetComponent<Image>().color = new Color32(68, 68, 68, 255);
						}
					}

					//Check Speed Car
					float speedF = rig.velocity.magnitude;
					speedI = (int)(speedF * 3.6f);
					speedCar_Text.text = "" + speedI.ToString();
				}

				SoundEngineTruck();
			}
		}
	}

	void FixedUpdate()
	{
		if (fps_Bool == false)
		{
			if (startTruck == true)
			{
				// hack
				// UpdateWheelPoses();
			}
		}
	}

	public void UpdateWheelPoses()
	{
		UpdateWheel(wheelCol_FL, wheelTransformFL);
		UpdateWheel(wheelCol_FR, wheelTransformFR);
		UpdateWheel(wheelCol_BL_For, wheelTransformBL_For);
		UpdateWheel(wheelCol_BR_For, wheelTransformBR_For);
		if (wheelTransformBL_Back != null)
		{
			UpdateWheel(wheelCol_BL_Back, wheelTransformBL_Back);
			UpdateWheel(wheelCol_BR_Back, wheelTransformBR_Back);
		}
	}

	private void UpdateWheel(WheelCollider wCol, Transform wTran)
	{
		Vector3 _pos = wTran.position;
		Quaternion _quat = wTran.rotation;
		wCol.GetWorldPose(out _pos, out _quat);
		wTran.transform.position = _pos;
		wTran.transform.rotation = _quat;
	}

	public void Motor()
	{
		if (lowGear_Bool == true)
		{
			wheelCol_BL_For.motorTorque = m_Vertical * speedEngine;
			wheelCol_BR_For.motorTorque = m_Vertical * speedEngine;
			if (wheelTransformBL_Back != null)
			{
				wheelCol_BL_Back.motorTorque = m_Vertical * speedEngine;
				wheelCol_BR_Back.motorTorque = m_Vertical * speedEngine;
			}

			gameObject.GetComponent<Rigidbody>().velocity =
				Vector3.ClampMagnitude(gameObject.GetComponent<Rigidbody>().velocity, maxSpeedEngine);
		}
		else if (lowGear_Bool == false)
		{
			wheelCol_BL_For.motorTorque = m_Vertical * lowGearPower;
			wheelCol_BR_For.motorTorque = m_Vertical * lowGearPower;
			if (wheelTransformBL_Back != null)
			{
				wheelCol_BL_Back.motorTorque = m_Vertical * lowGearPower;
				wheelCol_BR_Back.motorTorque = m_Vertical * lowGearPower;
			}
		}

		gameObject.GetComponent<Rigidbody>().velocity =
			Vector3.ClampMagnitude(gameObject.GetComponent<Rigidbody>().velocity, maxSpeedEngine);
	}

	public void SteerTruck()
	{
		wheelCol_FL.steerAngle = m_Horizontal * steer;
		wheelCol_FR.steerAngle = m_Horizontal * steer;
	}

	public void Brake()
	{
		if (m_Vertical < 0 && wheelCol_FL.rpm > 0)
		{
			wheelCol_FL.brakeTorque = (brace) * (Mathf.Abs(m_Vertical));
			wheelCol_FR.brakeTorque = (brace) * (Mathf.Abs(m_Vertical));
			if (parkingBrake_Bool == true)
			{
				if (stop_L != null)
				{
					stop_L.enabled = true;
					stop_R.enabled = true;
				}
			}
		}
		else if (m_Vertical > 0 && wheelCol_FL.rpm < 0)
		{
			wheelCol_FL.brakeTorque = (brace) * (Mathf.Abs(m_Vertical));
			wheelCol_FR.brakeTorque = (brace) * (Mathf.Abs(m_Vertical));
			if (parkingBrake_Bool == true)
			{
				if (stop_L != null)
				{
					stop_L.enabled = true;
					stop_R.enabled = true;
				}
			}
		}
		else
		{
			wheelCol_FL.brakeTorque = 0;
			wheelCol_FR.brakeTorque = 0;
			if (parkingBrake_Bool == true)
			{
				if (stop_L != null)
				{
					stop_L.enabled = false;
					stop_R.enabled = false;
				}
			}
		}

		if (m_Vertical == 0 && wheelCol_FL.rpm > 1)
		{
			rig.AddForce(-inertiaBrake * gameObject.GetComponent<Rigidbody>().velocity);
		}
		else if (m_Vertical == 0 && wheelCol_FL.rpm < 1)
		{
			rig.AddForce(-inertiaBrake * gameObject.GetComponent<Rigidbody>().velocity);
		}

		if (Input.GetKeyDown(parkingBrake))
		{
			parkingBrake_Bool = false;
			wheelCol_BL_For.brakeTorque = parkingBrakePower;
			wheelCol_BR_For.brakeTorque = parkingBrakePower;
			if (wheelTransformBL_Back != null)
			{
				wheelCol_BL_Back.brakeTorque = parkingBrakePower;
				wheelCol_BR_Back.brakeTorque = parkingBrakePower;
			}

			if (stop_L != null)
			{
				stop_L.enabled = true;
				stop_R.enabled = true;
			}

			if (truckCanvas != null)
			{
				panelTruck_G_Handbrake.GetComponent<Image>().color = new Color32(244, 0, 0, 240);
			}
		}
		else if (Input.GetKeyUp(parkingBrake))
		{
			wheelCol_BL_For.brakeTorque = 0;
			wheelCol_BR_For.brakeTorque = 0;
			if (wheelTransformBL_Back != null)
			{
				wheelCol_BL_Back.brakeTorque = 0;
				wheelCol_BR_Back.brakeTorque = 0;
			}

			parkingBrake_Bool = true;
			if (stop_L != null)
			{
				stop_L.enabled = false;
				stop_R.enabled = false;
			}

			if (truckCanvas != null)
			{
				panelTruck_G_Handbrake.GetComponent<Image>().color = new Color32(86, 86, 86, 163);
			}
		}
	}

	public void SoundEngineTruck()
	{
		//hack
		return;
		engineRPM = Mathf.Clamp(
			(((Mathf.Abs((wheelCol_FL.rpm + wheelCol_FR.rpm)) * gearShiftRate) + minEngineRPM)) /
			(float)(currentGear + 1), minEngineRPM, maxEngineRPM);
		s_Engine.pitch = Mathf.Lerp(s_Engine.pitch,
			Mathf.Lerp(pitchSoundTruck, 2f, (engineRPM - minEngineRPM / 1.82f) / (maxEngineRPM + minEngineRPM)),
			Time.deltaTime * 5f);
	}

	public void Revers()
	{
		checkRevers = wheelCol_FL.rpm;
		if (m_Vertical < -0 && checkRevers < -8)
		{
			s_Revers.mute = false;
			if (revers_L != null)
			{
				revers_L.enabled = true;
				revers_R.enabled = true;
			}
		}
		else
		{
			s_Revers.mute = true;
			if (revers_L != null)
			{
				revers_L.enabled = false;
				revers_R.enabled = false;
			}
		}
	}

	IEnumerator TurnSignal()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.3f);
			if (turnSignal_L != null)
			{
				turnSignal_L.enabled = true;
				turnSignal_R.enabled = true;
			}

			if (turnSignalPanel_L != null)
			{
				turnSignalPanel_L.enabled = true;
				turnSignalPanel_R.enabled = true;
			}

			if (truckCanvas != null)
			{
				panelTruck_TurnS_L.GetComponent<Image>().color = new Color32(30, 255, 0, 165);
				panelTruck_TurnS_R.GetComponent<Image>().color = new Color32(30, 255, 0, 165);
			}

			yield return new WaitForSeconds(0.3f);
			if (turnSignal_L != null)
			{
				turnSignal_L.enabled = false;
				turnSignal_R.enabled = false;
			}

			if (turnSignalPanel_L != null)
			{
				turnSignalPanel_L.enabled = false;
				turnSignalPanel_R.enabled = false;
			}

			if (truckCanvas != null)
			{
				panelTruck_TurnS_L.GetComponent<Image>().color = new Color32(68, 68, 68, 255);
				panelTruck_TurnS_R.GetComponent<Image>().color = new Color32(68, 68, 68, 255);
			}
		}
	}

	IEnumerator TurnSignal_Left()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.3f);
			if (turnSignalPanel_L != null)
			{
				turnSignalPanel_L.enabled = true;
			}

			if (turnSignal_L != null)
			{
				turnSignal_L.enabled = true;
			}

			if (truckCanvas != null)
			{
				panelTruck_TurnS_L.GetComponent<Image>().color = new Color32(30, 255, 0, 165);
			}

			yield return new WaitForSeconds(0.3f);
			if (turnSignalPanel_L != null)
			{
				turnSignalPanel_L.enabled = false;
			}

			if (turnSignal_L != null)
			{
				turnSignal_L.enabled = false;
			}

			if (truckCanvas != null)
			{
				panelTruck_TurnS_L.GetComponent<Image>().color = new Color32(68, 68, 68, 255);
			}
		}
	}

	IEnumerator TurnSignal_Right()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.3f);
			if (turnSignalPanel_R != null)
			{
				turnSignalPanel_R.enabled = true;
			}

			if (turnSignal_R != null)
			{
				turnSignal_R.enabled = true;
			}

			if (truckCanvas != null)
			{
				panelTruck_TurnS_R.GetComponent<Image>().color = new Color32(30, 255, 0, 165);
			}

			yield return new WaitForSeconds(0.3f);
			if (turnSignalPanel_R != null)
			{
				turnSignalPanel_R.enabled = false;
			}

			if (turnSignal_R != null)
			{
				turnSignal_R.enabled = false;
			}

			if (truckCanvas != null)
			{
				panelTruck_TurnS_R.GetComponent<Image>().color = new Color32(68, 68, 68, 255);
			}
		}
	}

	IEnumerator StartCar()
	{
		transform.GetChild(62).gameObject.SetActive(true);
		m_fPSController.gameObject.SetActive(false);
		m_fPSController.parent = transform;
		truckCanvas.enabled = true;
		yield return new WaitForSeconds(0.65f);
		panelTruck_TurnS_L.GetComponent<Image>().color = new Color32(30, 255, 0, 165);
		panelTruck_TurnS_R.GetComponent<Image>().color = new Color32(30, 255, 0, 165);
		panelTruck_G_OnManip.GetComponent<Image>().color = new Color32(255, 193, 30, 165);
		panelTruck_G_Handbrake.GetComponent<Image>().color = new Color32(244, 0, 0, 240);
		panelTruck_G_OD.GetComponent<Image>().color = new Color32(255, 193, 30, 165);
		panelTruck_G_ABS.GetComponent<Image>().color = new Color32(255, 193, 30, 165);
		yield return new WaitForSeconds(0.85f);
		panelTruck_TurnS_L.GetComponent<Image>().color = new Color32(68, 68, 68, 255);
		panelTruck_TurnS_R.GetComponent<Image>().color = new Color32(68, 68, 68, 255);
		panelTruck_G_OnManip.GetComponent<Image>().color = new Color32(86, 86, 86, 163);
		panelTruck_G_Handbrake.GetComponent<Image>().color = new Color32(86, 86, 86, 163);
		panelTruck_G_OD.GetComponent<Image>().color = new Color32(86, 86, 86, 163);
		panelTruck_G_ABS.GetComponent<Image>().color = new Color32(86, 86, 86, 163);
		yield return new WaitForSeconds(0.06f);
		s_StartStop.PlayOneShot(S_start);
		yield return new WaitForSeconds(0.6f);
		s_Engine.Play();
		s_Revers.Play();
		fps_Bool = false;
	}

	IEnumerator StopCar()
	{
		s_Engine.Stop();
		s_Revers.Stop();
		yield return new WaitForSeconds(0.01f);
		s_StartStop.PlayOneShot(S_stop);
		fps_Bool = true;
		truckCanvas.enabled = false;
		transform.GetChild(62).gameObject.SetActive(false);
		m_fPSController.localPosition = new Vector3(-3.05f, 0.68f, -0.24f);
		m_fPSController.parent = null;
		m_fPSController.gameObject.SetActive(true);
	}

	public void TurnGignalOn()
	{
		if (turnSignal_Bool == true)
		{
			StartCoroutine("TurnSignal");
			s_Turn.Play();
			turnSignal_Bool = false;
		}
		else if (turnSignal_Bool == false)
		{
			StopCoroutine("TurnSignal");
			s_Turn.Stop();
			if (turnSignalPanel_L != null)
			{
				turnSignalPanel_L.enabled = false;
				turnSignalPanel_R.enabled = false;
			}

			if (turnSignal_L != null)
			{
				turnSignal_L.enabled = false;
				turnSignal_R.enabled = false;
			}

			turnSignal_Bool = true;
		}
	}

	public void TurnGignalLeftOn()
	{
		if (leftTurn_Bool == true)
		{
			StartCoroutine("TurnSignal_Left");
			s_Turn.Play();
			leftTurn_Bool = false;
		}
		else if (leftTurn_Bool == false)
		{
			StopCoroutine("TurnSignal_Left");
			s_Turn.Stop();
			if (turnSignalPanel_L != null)
			{
				turnSignalPanel_L.enabled = false;
			}

			if (turnSignal_L != null)
			{
				turnSignal_L.enabled = false;
			}

			leftTurn_Bool = true;
		}
	}

	public void TurnGignalRightOn()
	{
		if (RightTurn_Bool == true)
		{
			StartCoroutine("TurnSignal_Right");
			s_Turn.Play();
			RightTurn_Bool = false;
		}
		else if (RightTurn_Bool == false)
		{
			StopCoroutine("TurnSignal_Right");
			s_Turn.Stop();
			if (turnSignalPanel_R != null)
			{
				turnSignalPanel_R.enabled = false;
			}

			if (turnSignal_R != null)
			{
				turnSignal_R.enabled = false;
			}

			RightTurn_Bool = true;
		}
	}

	public void Tachometer()
	{
		panelTruck_Q_Tachometer.fillAmount = tachometer_Float;
		if (lowGear_Bool == true)
		{
			tachometer_Float = Mathf.Clamp(tachometer_Float, 0.17f, 0.415f);
		}
		else if (lowGear_Bool == false)
		{
			tachometer_Float = Mathf.Clamp(tachometer_Float, 0.17f, 0.588f);
		}

		if (m_Vertical != 0)
		{

			tachometer_Float = Mathf.Lerp(tachometer_Float, m_Vertical, Time.deltaTime * 0.78f);
		}
		else if (m_Vertical == 0)
		{
			tachometer_Float = Mathf.Lerp(tachometer_Float, m_Vertical, Time.deltaTime * 0.45f);
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (m_fPSController != null)
		{
			if (col.tag == "Player")
			{
				OnFps = true;
				EnterExitCar_Im.enabled = true;
			}
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (m_fPSController != null)
		{
			OnFps = false;
			EnterExitCar_Im.enabled = false;
		}
	}
}