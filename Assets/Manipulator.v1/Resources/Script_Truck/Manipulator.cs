using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VRC2.Animations.CraneTruck;

public class Manipulator : MonoBehaviour
{

	// [HideInInspector]
	public ControllerTruck m_ScriptM;
	[HideInInspector] public Transform obj_Manipulator;
	[HideInInspector] public int checkManip = 0;
	[HideInInspector] public Transform support_FL_Vertical;
	private bool support_FL_Vertical_Bool = true;
	[HideInInspector] public Transform support_FR_Vertical;
	private bool support_FR_Vertical_Bool = true;
	[HideInInspector] public Transform support_FL_Horizontal;
	private bool support_FL_Horizontal_Bool = true;
	[HideInInspector] public Transform support_FR_Horizontal;

	private bool support_FR_Horizontal_Bool = true;

	//Arrow
	// [HideInInspector]
	public Transform arrowRotation;

	// [HideInInspector]
	public Transform arrowForward1;

	// [HideInInspector]
	public Transform arrowForward2;

	private Vector3 endPos_Arrow2;

	// [HideInInspector]
	public Transform arrowForward3;

	private Vector3 endPos_Arrow3;

	// [HideInInspector]
	public Transform arrowForward4;

	private Vector3 endPos_Arrow4;

	// [HideInInspector]
	public Transform arrowForward5;

	private Vector3 endPos_Arrow5;

	//Piston
	[HideInInspector] public Transform pistonUp;
	[HideInInspector] public Transform pistonDown;
	[HideInInspector] public Transform checkDistanceHook_A;
	[HideInInspector] public Transform checkDistanceHook_B;

	private float angleRotArrow_float = 0;

	//Wing Manip
	[HideInInspector] public Transform wingManip;
	[HideInInspector] public float arrowUp_Float = 0f;
	private Transform obj_Piston_1;

	private Transform obj_Piston_2;

	//UI
	private Image panelManip;
	private Image panelManipB1;
	private Image panelManipFL;
	private Image panelManipFR;
	private Image panelManipRot;
	private Image warningOn;
	private Button panelManipFL_Button;
	private Button panelManipFR_Button;
	private bool panelManipFL_Button_Bool = true;
	private bool panelManipFR_Button_Bool = true;
	[HideInInspector] public bool uiManip_Bool = true;

	[HideInInspector] public Text hook_Text;

	//Bloc Crable
	[HideInInspector] public bool block_Left = true;
	[HideInInspector] public bool block_Right = true;
	[HideInInspector] public bool block_For = true;
	[HideInInspector] public bool block_Back = true;
	[HideInInspector] public bool block_Down = true;
	[HideInInspector] public Transform obl_Crable;
	[HideInInspector] public Transform pisCrableUp;
	[HideInInspector] public Transform pisCrableDown;
	[HideInInspector] public Transform pisCrableUp_Parent;
	[HideInInspector] public Transform pisCrableDown_Parent;
	private Transform pisCrable_1;
	private Transform pisCrable_2;

	#region Hack for recording

	private CraneTruckInputActions craneTIA;

	private InputAction craneMoveInput;
	private InputAction craneArrowInput;
	private InputAction craneSeizeInput;


	public CraneTruckInputRecording recording;

	#endregion

	void Start()
	{

		// initialize input actions
		craneTIA = new CraneTruckInputActions();
		craneTIA.Enable();

		craneMoveInput = craneTIA.Crane.Move;
		craneArrowInput = craneTIA.Crane.Arrow;
		craneSeizeInput = craneTIA.Crane.Seize;

		if (m_ScriptM != null)
		{
			var obj1 = new GameObject();
			var obj2 = new GameObject();
			obj_Piston_1 = obj1.transform;
			obj_Piston_2 = obj2.transform;
			obj_Piston_1.position = pistonUp.position;
			obj_Piston_2.position = pistonDown.position;
			obj_Piston_1.parent = arrowForward1;
			obj_Piston_2.parent = arrowRotation;
			obj_Piston_1.LookAt(obj_Piston_2.position, obj_Piston_1.up);
			obj_Piston_2.LookAt(obj_Piston_1.position, obj_Piston_2.up);
			pistonUp.parent = obj_Piston_1;
			pistonDown.parent = obj_Piston_2;
			if (obl_Crable != null)
			{
				var ob1 = new GameObject();
				var ob2 = new GameObject();
				pisCrable_1 = ob1.transform;
				pisCrable_2 = ob2.transform;
				pisCrable_1.position = pisCrableUp.position;
				pisCrable_2.position = pisCrableDown.position;
				pisCrable_1.parent = pisCrableUp_Parent;
				pisCrable_2.parent = pisCrableDown_Parent;
				pisCrable_1.LookAt(pisCrable_2.position, pisCrable_1.up);
				pisCrable_2.LookAt(pisCrable_1.position, pisCrable_2.up);
				pisCrableUp.parent = pisCrable_1;
				pisCrableDown.parent = pisCrable_2;
			}
		}
	}

	void Update()
	{
		if (recording.truckMode) return;

		if (m_ScriptM != null)
		{
			if (m_ScriptM.fps_Bool == false)
			{
				if (m_ScriptM.onManip == false)
				{
					RotationArrow();
					ArrowUp();
					ArrowForward();
					//Lift Horizontal
					if (Input.GetKey(m_ScriptM.horizontalLiftTruck) && Input.GetKey(m_ScriptM.forwardLift))
					{
						Lift_Horizontal_FL();
						Lift_Horizontal_FR();
						support_FL_Horizontal_Bool = true;
						support_FR_Horizontal_Bool = true;
					}

					if (Input.GetKey(m_ScriptM.horizontalLiftTruck) && Input.GetKey(m_ScriptM.backLift))
					{
						Lift_Horizontal_FL();
						Lift_Horizontal_FR();
						support_FL_Horizontal_Bool = false;
						support_FR_Horizontal_Bool = false;
					}

					//Lift Vertical
					if (Input.GetKey(m_ScriptM.verticalLiftTruck) && Input.GetKey(m_ScriptM.forwardLift))
					{
						Lift_Vertical_FL();
						Lift_Vertical_FR();
						support_FL_Vertical_Bool = true;
						support_FR_Vertical_Bool = true;
					}

					if (Input.GetKey(m_ScriptM.verticalLiftTruck) && Input.GetKey(m_ScriptM.backLift))
					{
						Lift_Vertical_FL();
						Lift_Vertical_FR();
						support_FL_Vertical_Bool = false;
						support_FR_Vertical_Bool = false;
					}
				}

				if (Input.GetKeyDown(m_ScriptM.powrManipulator))
				{
					// AddUIManip ();
					// if (panelManipFL_Button_Bool == false) {
					// 	panelManipFL.GetComponent<Image> ().color = new Color32 (255, 193, 30, 165);
					// }
					// if (panelManipFR_Button_Bool == false) {
					// 	panelManipFR.GetComponent<Image> ().color = new Color32 (255, 193, 30, 165);
					// }
				}

				if (Input.GetKeyDown(m_ScriptM.powrManipulator) && m_ScriptM.blockOnManip_Cargo == false)
				{
					StartCoroutine("BlockOnPower");
				}
			}
		}
	}

	void LateUpdate()
	{
		if (obj_Piston_1 != null && obj_Piston_2 != null)
		{
			obj_Piston_1.LookAt(obj_Piston_2.position, obj_Piston_1.up);
			obj_Piston_2.LookAt(obj_Piston_1.position, obj_Piston_2.up);
		}

		if (obl_Crable != null)
		{
			if (pisCrable_1 != null && pisCrable_2 != null)
			{
				pisCrable_1.LookAt(pisCrable_2.position, pisCrable_1.up);
				pisCrable_2.LookAt(pisCrable_1.position, pisCrable_2.up);
			}
		}
	}

	public void RotationArrow()
	{
		print($"uiManip_Bool: {uiManip_Bool}");
		// if (Input.GetKey (m_ScriptM.rotationArrowLeft) && block_Left == true) {
		if (craneMoveInput.ReadValue<Vector2>().x > 0 && block_Left == true)
		{
			// arrowRotation.Rotate(Vector3.up * m_ScriptM.speedRotation * Time.deltaTime);
			// SoundPitchManip();
			// angleRotArrow_float += Time.deltaTime * m_ScriptM.speedRotation;
			LeftArm();
			// } else if (Input.GetKey (m_ScriptM.rotationArrowRight) && block_Right == true) {
			
			
		}
		else if (craneMoveInput.ReadValue<Vector2>().x < 0 && block_Right == true)
		{
			// arrowRotation.Rotate(Vector3.up * -m_ScriptM.speedRotation * Time.deltaTime);
			// SoundPitchManip();
			// angleRotArrow_float -= Time.deltaTime * m_ScriptM.speedRotation;
			RightArm();
		}

		if (uiManip_Bool == false)
		{
			panelManip.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, angleRotArrow_float);
		}
	}

	public void ArrowUp()
	{
		// if (Input.GetKey (m_ScriptM.UpArrow)) {
		if (craneMoveInput.ReadValue<Vector2>().y > 0)
		{
			if (checkManip == 1 || checkManip == 2)
			{
				arrowUp_Float = Mathf.Clamp(arrowUp_Float, -62f, 0);
			}
			else if (checkManip == 3 && obl_Crable == null)
			{
				// arrowUp_Float = Mathf.Clamp(arrowUp_Float, -62f, 13f);

				UpArm();
				return;

			}
			else if (checkManip == 3 && obl_Crable != null)
			{
				arrowUp_Float = Mathf.Clamp(arrowUp_Float, -51.1f, 13f);
			}

			arrowUp_Float -= Time.deltaTime * m_ScriptM.speedUpArrow;
			arrowForward1.transform.localRotation = Quaternion.AngleAxis(arrowUp_Float, Vector3.left);
			if (obl_Crable != null)
			{
				obl_Crable.localRotation = Quaternion.AngleAxis(arrowUp_Float, Vector3.right);
			}

			SoundPitchManip();
			// } else if (Input.GetKey (m_ScriptM.DownArrow) && block_Down == true) {
		}
		else if (craneMoveInput.ReadValue<Vector2>().y < 0 && block_Down == true)
		{
			if (checkManip == 1 || checkManip == 2)
			{
				arrowUp_Float = Mathf.Clamp(arrowUp_Float, -62f, 0);
			}
			else if (checkManip == 3 && obl_Crable == null)
			{
				// arrowUp_Float = Mathf.Clamp(arrowUp_Float, -62f, 13f);

				DownArm();
				return;
			}
			else if (checkManip == 3 && obl_Crable != null)
			{
				arrowUp_Float = Mathf.Clamp(arrowUp_Float, -51.1f, 13f);
			}

			arrowUp_Float += Time.deltaTime * m_ScriptM.speedUpArrow;
			arrowForward1.transform.localRotation = Quaternion.AngleAxis(arrowUp_Float, Vector3.left);
			if (obl_Crable != null)
			{
				obl_Crable.transform.localRotation = Quaternion.AngleAxis(arrowUp_Float, Vector3.right);
			}

			SoundPitchManip();
		}
	}

	public void ArrowForward()
	{
		// if (Input.GetKey (m_ScriptM.backArrow) && block_Back == true) {
		if (craneArrowInput.ReadValue<Vector2>().x < 0 && block_Back == true)
		{
			if (checkManip == 1 || checkManip == 2)
			{
				if (endPos_Arrow3 == new Vector3(0.0006604248f, -0.06289651f, -0.09905349f))
				{
					arrowForward2.transform.localPosition = Vector3.MoveTowards(arrowForward2.transform.localPosition,
						new Vector3(0.0006269385f, 0.1260378f, -0.1995553f), m_ScriptM.speedForward * Time.deltaTime);
				}

				if (endPos_Arrow4 == new Vector3(0.005361533f, -0.05230227f, 0.1309257f))
				{
					arrowForward3.transform.localPosition = Vector3.MoveTowards(arrowForward3.transform.localPosition,
						new Vector3(0.0006604248f, -0.06289651f, -0.09905349f),
						m_ScriptM.speedForward * Time.deltaTime);
				}

				arrowForward4.transform.localPosition = Vector3.MoveTowards(arrowForward4.transform.localPosition,
					new Vector3(0.005361533f, -0.05230227f, 0.1309257f), m_ScriptM.speedForward * Time.deltaTime);
			}
			else if (checkManip == 3)
			{
				// if (endPos_Arrow3 == new Vector3(0.0005618262f, 0.0002853314f, -0.04771918f))
				// {
				// 	arrowForward2.transform.localPosition = Vector3.MoveTowards(arrowForward2.transform.localPosition,
				// 		new Vector3(-0.01917278f, 0.07633314f, -3.588468f), m_ScriptM.speedForward * Time.deltaTime);
				// }
				//
				// if (endPos_Arrow4 == new Vector3(-0.0003943945f, -0.04917584f, -0.05477962f))
				// {
				// 	arrowForward3.transform.localPosition = Vector3.MoveTowards(arrowForward3.transform.localPosition,
				// 		new Vector3(0.0005618262f, 0.0002853314f, -0.04771918f),
				// 		m_ScriptM.speedForward * Time.deltaTime);
				// }
				//
				// if (endPos_Arrow5 == new Vector3(0.004825752f, -0.08844844f, -0.141324f))
				// {
				// 	arrowForward4.transform.localPosition = Vector3.MoveTowards(arrowForward4.transform.localPosition,
				// 		new Vector3(-0.0003943945f, -0.04917584f, -0.05477962f),
				// 		m_ScriptM.speedForward * Time.deltaTime);
				// }
				//
				// arrowForward5.transform.localPosition = Vector3.MoveTowards(arrowForward5.transform.localPosition,
				// 	new Vector3(0.004825752f, -0.08844844f, -0.141324f), m_ScriptM.speedForward * Time.deltaTime);
				ShrinkArm();
			}

			SoundPitchManip();
			// } else if (Input.GetKey (m_ScriptM.forwardArrow) && block_For == true) {
		}
		else if (craneArrowInput.ReadValue<Vector2>().x > 0 && block_For == true)
		{
			if (checkManip == 1 || checkManip == 2)
			{
				arrowForward2.transform.localPosition = Vector3.MoveTowards(arrowForward2.transform.localPosition,
					new Vector3(0.0006269385f, 0.1260378f, -3.562f), m_ScriptM.speedForward * Time.deltaTime);
				if (endPos_Arrow2 == new Vector3(0.0006269385f, 0.1260378f, -3.562f))
				{
					arrowForward3.transform.localPosition = Vector3.MoveTowards(arrowForward3.transform.localPosition,
						new Vector3(0.0006604248f, -0.06289651f, -3.471f), m_ScriptM.speedForward * Time.deltaTime);
				}

				if (endPos_Arrow3 == new Vector3(0.0006604248f, -0.06289651f, -3.471f))
				{
					arrowForward4.transform.localPosition = Vector3.MoveTowards(arrowForward4.transform.localPosition,
						new Vector3(0.005361533f, -0.05230227f, -3.469f), m_ScriptM.speedForward * Time.deltaTime);
				}
			}
			else if (checkManip == 3)
			{
				// arrowForward2.transform.localPosition = Vector3.MoveTowards(arrowForward2.transform.localPosition,
				// 	new Vector3(-0.01917278f, 0.07633314f, -6.513f), m_ScriptM.speedForward * Time.deltaTime);
				// if (endPos_Arrow2 == new Vector3(-0.01917278f, 0.07633314f, -6.513f))
				// {
				// 	arrowForward3.transform.localPosition = Vector3.MoveTowards(arrowForward3.transform.localPosition,
				// 		new Vector3(0.0005618262f, 0.0002853314f, -3.065f), m_ScriptM.speedForward * Time.deltaTime);
				// }
				//
				// if (endPos_Arrow3 == new Vector3(0.0005618262f, 0.0002853314f, -3.065f))
				// {
				// 	arrowForward4.transform.localPosition = Vector3.MoveTowards(arrowForward4.transform.localPosition,
				// 		new Vector3(-0.0003943945f, -0.04917584f, -3.109f), m_ScriptM.speedForward * Time.deltaTime);
				// }
				//
				// if (endPos_Arrow4 == new Vector3(-0.0003943945f, -0.04917584f, -3.109f))
				// {
				// 	arrowForward5.transform.localPosition = Vector3.MoveTowards(arrowForward5.transform.localPosition,
				// 		new Vector3(0.004825752f, -0.08844844f, -3.22f), m_ScriptM.speedForward * Time.deltaTime);
				// }
				ExtendArm();
			}

			SoundPitchManip();
		}

		//Check Distance
		endPos_Arrow2 = arrowForward2.localPosition;
		endPos_Arrow3 = arrowForward3.localPosition;
		endPos_Arrow4 = arrowForward4.localPosition;
		if (checkManip == 3)
		{
			endPos_Arrow5 = arrowForward5.localPosition;
		}
	}

	#region API

	// the following actions are based on checkManip == 3 and obl_Crable == null and uiManip_Bool == true

	public void UpArm()
	{
		arrowUp_Float = Mathf.Clamp(arrowUp_Float, -62f, 13f);

		arrowUp_Float -= Time.deltaTime * m_ScriptM.speedUpArrow;
		arrowForward1.transform.localRotation = Quaternion.AngleAxis(arrowUp_Float, Vector3.left);

		SoundPitchManip();
	}

	public void DownArm()
	{
		arrowUp_Float = Mathf.Clamp(arrowUp_Float, -62f, 13f);

		arrowUp_Float += Time.deltaTime * m_ScriptM.speedUpArrow;
		arrowForward1.transform.localRotation = Quaternion.AngleAxis(arrowUp_Float, Vector3.left);

		SoundPitchManip();
	}

	public void ExtendArm()
	{
		arrowForward2.transform.localPosition = Vector3.MoveTowards(arrowForward2.transform.localPosition,
			new Vector3(-0.01917278f, 0.07633314f, -6.513f), m_ScriptM.speedForward * Time.deltaTime);
		if (endPos_Arrow2 == new Vector3(-0.01917278f, 0.07633314f, -6.513f))
		{
			arrowForward3.transform.localPosition = Vector3.MoveTowards(arrowForward3.transform.localPosition,
				new Vector3(0.0005618262f, 0.0002853314f, -3.065f), m_ScriptM.speedForward * Time.deltaTime);
		}

		if (endPos_Arrow3 == new Vector3(0.0005618262f, 0.0002853314f, -3.065f))
		{
			arrowForward4.transform.localPosition = Vector3.MoveTowards(arrowForward4.transform.localPosition,
				new Vector3(-0.0003943945f, -0.04917584f, -3.109f), m_ScriptM.speedForward * Time.deltaTime);
		}

		if (endPos_Arrow4 == new Vector3(-0.0003943945f, -0.04917584f, -3.109f))
		{
			arrowForward5.transform.localPosition = Vector3.MoveTowards(arrowForward5.transform.localPosition,
				new Vector3(0.004825752f, -0.08844844f, -3.22f), m_ScriptM.speedForward * Time.deltaTime);
		}

		// add sound effect
		SoundPitchManip();
	}

	public void ShrinkArm()
	{
		if (endPos_Arrow3 == new Vector3(0.0005618262f, 0.0002853314f, -0.04771918f))
		{
			arrowForward2.transform.localPosition = Vector3.MoveTowards(arrowForward2.transform.localPosition,
				new Vector3(-0.01917278f, 0.07633314f, -3.588468f), m_ScriptM.speedForward * Time.deltaTime);
		}

		if (endPos_Arrow4 == new Vector3(-0.0003943945f, -0.04917584f, -0.05477962f))
		{
			arrowForward3.transform.localPosition = Vector3.MoveTowards(arrowForward3.transform.localPosition,
				new Vector3(0.0005618262f, 0.0002853314f, -0.04771918f),
				m_ScriptM.speedForward * Time.deltaTime);
		}

		if (endPos_Arrow5 == new Vector3(0.004825752f, -0.08844844f, -0.141324f))
		{
			arrowForward4.transform.localPosition = Vector3.MoveTowards(arrowForward4.transform.localPosition,
				new Vector3(-0.0003943945f, -0.04917584f, -0.05477962f),
				m_ScriptM.speedForward * Time.deltaTime);
		}

		arrowForward5.transform.localPosition = Vector3.MoveTowards(arrowForward5.transform.localPosition,
			new Vector3(0.004825752f, -0.08844844f, -0.141324f), m_ScriptM.speedForward * Time.deltaTime);

		SoundPitchManip();
	}

	public void RightArm()
	{
		arrowRotation.Rotate(Vector3.up * -m_ScriptM.speedRotation * Time.deltaTime);
		SoundPitchManip();
		angleRotArrow_float -= Time.deltaTime * m_ScriptM.speedRotation;
	}

	public void LeftArm()
	{
		arrowRotation.Rotate(Vector3.up * m_ScriptM.speedRotation * Time.deltaTime);
		SoundPitchManip();
		angleRotArrow_float += Time.deltaTime * m_ScriptM.speedRotation;
	}


	#endregion

	IEnumerator BlockOnPower()
	{
		yield return new WaitForSeconds(0.1f);
		panelManipB1.GetComponent<Image>().color = new Color32(255, 185, 0, 218);
		yield return new WaitForSeconds(0.1f);
		panelManipB1.GetComponent<Image>().color = new Color32(101, 101, 101, 218);
		yield return new WaitForSeconds(0.1f);
		panelManipB1.GetComponent<Image>().color = new Color32(255, 185, 0, 218);
		yield return new WaitForSeconds(0.1f);
		panelManipB1.GetComponent<Image>().color = new Color32(101, 101, 101, 218);
		yield return new WaitForSeconds(0.1f);
		panelManipB1.GetComponent<Image>().color = new Color32(255, 185, 0, 218);
		yield return new WaitForSeconds(0.1f);
		panelManipB1.GetComponent<Image>().color = new Color32(101, 101, 101, 218);
	}

	IEnumerator BlockArrow()
	{
		yield return new WaitForSeconds(0.1f);
		warningOn.GetComponent<Image>().color = new Color32(255, 185, 0, 218);
		yield return new WaitForSeconds(0.1f);
		warningOn.GetComponent<Image>().color = new Color32(107, 107, 107, 187);
		yield return new WaitForSeconds(0.1f);
		warningOn.GetComponent<Image>().color = new Color32(255, 185, 0, 218);
		yield return new WaitForSeconds(0.1f);
		warningOn.GetComponent<Image>().color = new Color32(107, 107, 107, 187);
		yield return new WaitForSeconds(0.1f);
		warningOn.GetComponent<Image>().color = new Color32(255, 185, 0, 218);
		yield return new WaitForSeconds(0.1f);
		warningOn.GetComponent<Image>().color = new Color32(107, 107, 107, 187);
		yield return new WaitForSeconds(0.1f);
		warningOn.GetComponent<Image>().color = new Color32(255, 185, 0, 218);
		yield return new WaitForSeconds(0.1f);
		warningOn.GetComponent<Image>().color = new Color32(107, 107, 107, 187);
	}

	public void SoundPitchManip()
	{
		m_ScriptM.s_Engine.pitch = Mathf.Lerp(m_ScriptM.s_Engine.pitch, 1.95f, Time.deltaTime * 0.8f);
	}

	public void AddUIManip()
	{
		if (uiManip_Bool == true)
		{
			//Panel_Q_Manipulator_B1
			var inPanelPanelB1 = new GameObject();
			inPanelPanelB1.name = "Panel_Q_Manipulator_B1";
			inPanelPanelB1.AddComponent<Image>();
			panelManipB1 = inPanelPanelB1.GetComponent<Image>();
			inPanelPanelB1.transform.SetParent(m_ScriptM.truckCanvas.transform, false);
			panelManipB1.sprite = Resources.Load<Sprite>("UI_Truck/Panel_Q_Manipulator_B1");
			panelManipB1.GetComponent<RectTransform>().anchoredPosition = new Vector2(38.1f, 88.7f);
			panelManipB1.GetComponent<RectTransform>().sizeDelta = new Vector2(58, 58);
			panelManipB1.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
			panelManipB1.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
			panelManipB1.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panelManipB1.GetComponent<Image>().color = new Color32(101, 101, 101, 218);
			//Panel Rotation
			var inPanelPanelR = new GameObject();
			inPanelPanelR.name = "PanelManipulatorRot";
			inPanelPanelR.AddComponent<Image>();
			panelManip = inPanelPanelR.GetComponent<Image>();
			inPanelPanelR.transform.SetParent(m_ScriptM.truckCanvas.transform, false);
			panelManip.sprite = Resources.Load<Sprite>("UI_Truck/Panel_Q_Manipulator_R");
			panelManip.GetComponent<RectTransform>().anchoredPosition = new Vector2(281.87f, 88.8f);
			panelManip.GetComponent<RectTransform>().sizeDelta = new Vector2(67, 67);
			panelManip.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
			panelManip.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
			panelManip.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panelManip.GetComponent<Image>().color = new Color32(101, 101, 101, 218);
			//Warning
			var wOn = new GameObject();
			wOn.name = "WarningON";
			wOn.AddComponent<Image>();
			warningOn = wOn.GetComponent<Image>();
			wOn.transform.SetParent(m_ScriptM.truckCanvas.transform, false);
			warningOn.sprite = Resources.Load<Sprite>("UI_Truck/WarningON");
			warningOn.GetComponent<RectTransform>().anchoredPosition = new Vector2(210.4f, 91.1f);
			warningOn.GetComponent<RectTransform>().sizeDelta = new Vector2(58.2f, 53);
			warningOn.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
			warningOn.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
			warningOn.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			warningOn.GetComponent<Image>().color = new Color32(107, 107, 107, 187);
			//Panel
			var inPanelPanel = new GameObject();
			inPanelPanel.name = "PanelManipulator";
			inPanelPanel.AddComponent<Image>();
			panelManipRot = inPanelPanel.GetComponent<Image>();
			inPanelPanel.transform.SetParent(m_ScriptM.truckCanvas.transform, false);
			panelManipRot.sprite = Resources.Load<Sprite>("UI_Truck/Panel_Q_Manipulator");
			panelManipRot.GetComponent<RectTransform>().anchoredPosition = new Vector2(166.4f, 75);
			panelManipRot.GetComponent<RectTransform>().sizeDelta = new Vector2(320, 320);
			panelManipRot.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
			panelManipRot.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
			panelManipRot.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panelManipRot.GetComponent<Image>().color = new Color32(255, 255, 255, 230);
			// Text Hoop Dis
			var textH = new GameObject();
			textH.name = "Text Hook";
			textH.AddComponent<Text>();
			hook_Text = textH.GetComponent<Text>();
			hook_Text.transform.SetParent(m_ScriptM.truckCanvas.transform, false);
			hook_Text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			hook_Text.GetComponent<RectTransform>().anchoredPosition = new Vector2(271.6f, 87.7f);
			hook_Text.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);
			hook_Text.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
			hook_Text.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
			hook_Text.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			hook_Text.GetComponent<Text>().color = new Color32(9, 9, 9, 255);
			hook_Text.GetComponent<Text>().fontSize = 24;
			//Panel_Q_Manipulator_FL
			var inPanelPanelFL = new GameObject();
			inPanelPanelFL.name = "Panel_Q_Manipulator_FL";
			inPanelPanelFL.AddComponent<Image>();
			panelManipFL = inPanelPanelFL.GetComponent<Image>();
			inPanelPanelFL.AddComponent<Button>();
			panelManipFL_Button = inPanelPanelFL.GetComponent<Button>();
			panelManipFL_Button.onClick.AddListener(gameObject.GetComponent<Manipulator>().ImFL);
			inPanelPanelFL.transform.SetParent(m_ScriptM.truckCanvas.transform, false);
			panelManipFL.sprite = Resources.Load<Sprite>("UI_Truck/Panel_Q_Manipulator_F");
			panelManipFL.GetComponent<Button>().image = Resources.Load<Image>("UI_Truck/Panel_Q_Manipulator_F");
			panelManipFL.GetComponent<RectTransform>().anchoredPosition = new Vector2(281.6f, 151.9f);
			panelManipFL.GetComponent<RectTransform>().sizeDelta = new Vector2(55, 55);
			panelManipFL.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
			panelManipFL.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
			panelManipFL.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panelManipFL.GetComponent<Image>().color = new Color32(45, 45, 45, 218);
			panelManipFL_Button.targetGraphic = panelManipFL;
			//Panel_Q_Manipulator_FR
			var inPanelPanelFR = new GameObject();
			inPanelPanelFR.name = "Panel_Q_Manipulator_FR";
			inPanelPanelFR.AddComponent<Image>();
			panelManipFR = inPanelPanelFR.GetComponent<Image>();
			inPanelPanelFR.AddComponent<Button>();
			panelManipFR_Button = inPanelPanelFR.GetComponent<Button>();
			panelManipFR_Button.onClick.AddListener(gameObject.GetComponent<Manipulator>().ImFR);
			inPanelPanelFR.transform.SetParent(m_ScriptM.truckCanvas.transform, false);
			panelManipFR.sprite = Resources.Load<Sprite>("UI_Truck/Panel_Q_Manipulator_F");
			panelManipFR.GetComponent<RectTransform>().anchoredPosition = new Vector2(281.6f, 25.7f);
			panelManipFR.GetComponent<RectTransform>().sizeDelta = new Vector2(55, 55);
			panelManipFR.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
			panelManipFR.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
			panelManipFR.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			panelManipFR.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);
			panelManipFR.GetComponent<Image>().color = new Color32(45, 45, 45, 218);
			panelManipFR_Button.targetGraphic = panelManipFR;
			uiManip_Bool = false;
		}
		else if (uiManip_Bool == false && m_ScriptM.blockOnManip == true && m_ScriptM.blockOnManip_Cargo == true)
		{
			Destroy(panelManip.gameObject);
			Destroy(panelManipB1.gameObject);
			Destroy(panelManipFL.gameObject);
			Destroy(panelManipFR.gameObject);
			Destroy(panelManipRot.gameObject);
			Destroy(hook_Text.gameObject);
			Destroy(warningOn.gameObject);
			uiManip_Bool = true;
		}
	}

	public void ImFL()
	{
		if (panelManipFL_Button_Bool == true)
		{
			panelManipFL.GetComponent<Image>().color = new Color32(255, 193, 30, 165);
			panelManipFL_Button_Bool = false;
		}
		else if (panelManipFL_Button_Bool == false)
		{
			panelManipFL.GetComponent<Image>().color = new Color32(45, 45, 45, 218);
			panelManipFL_Button_Bool = true;
		}
	}

	public void ImFR()
	{
		if (panelManipFR_Button_Bool == true)
		{
			panelManipFR.GetComponent<Image>().color = new Color32(255, 193, 30, 165);
			panelManipFR_Button_Bool = false;
		}
		else if (panelManipFR_Button_Bool == false)
		{
			panelManipFR.GetComponent<Image>().color = new Color32(45, 45, 45, 218);
			panelManipFR_Button_Bool = true;
		}
	}

	//Lift Horizontal FL
	public void Lift_Horizontal_FL()
	{
		if (checkManip == 1 && panelManipFL_Button_Bool == false)
		{
			if (support_FL_Horizontal_Bool == true)
			{
				support_FL_Horizontal.transform.localPosition = Vector3.MoveTowards(
					support_FL_Horizontal.transform.localPosition, new Vector3(-1.24601f, -0.1445284f, -0.06621829f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}
			else if (support_FL_Horizontal_Bool == false)
			{
				support_FL_Horizontal.transform.localPosition = Vector3.MoveTowards(
					support_FL_Horizontal.transform.localPosition, new Vector3(-2.656f, -0.145f, -0.06621829f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}

			SoundPitchManip();
		}
		else if (checkManip == 2 && panelManipFL_Button_Bool == false)
		{
			if (support_FL_Horizontal_Bool == true)
			{
				support_FL_Horizontal.transform.localPosition = Vector3.MoveTowards(
					support_FL_Horizontal.transform.localPosition, new Vector3(-1.24601f, -0.1305281f, -0.06621457f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}
			else if (support_FL_Horizontal_Bool == false)
			{
				support_FL_Horizontal.transform.localPosition = Vector3.MoveTowards(
					support_FL_Horizontal.transform.localPosition, new Vector3(-2.611f, -0.131f, -0.06621457f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}

			SoundPitchManip();
		}
		else if (checkManip == 3 && panelManipFL_Button_Bool == false)
		{
			if (support_FL_Horizontal_Bool == true)
			{
				support_FL_Horizontal.transform.localPosition = Vector3.MoveTowards(
					support_FL_Horizontal.transform.localPosition, new Vector3(-1.175445f, -0.1384991f, -0.04297363f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);

			}
			else if (support_FL_Horizontal_Bool == false)
			{
				support_FL_Horizontal.transform.localPosition = Vector3.MoveTowards(
					support_FL_Horizontal.transform.localPosition, new Vector3(-2.616f, -0.1384991f, -0.04297363f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}

			SoundPitchManip();
		}
	}

	//Lift Horizontal FR
	public void Lift_Horizontal_FR()
	{
		if (checkManip == 1 && panelManipFR_Button_Bool == false)
		{
			if (support_FR_Horizontal_Bool == true)
			{
				support_FR_Horizontal.transform.localPosition = Vector3.MoveTowards(
					support_FR_Horizontal.transform.localPosition, new Vector3(1.241764f, -0.1445291f, -0.06622003f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}
			else if (support_FR_Horizontal_Bool == false)
			{
				support_FR_Horizontal.transform.localPosition = Vector3.MoveTowards(
					support_FR_Horizontal.transform.localPosition, new Vector3(2.658f, -0.145f, -0.06622003f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}

			SoundPitchManip();
		}
		else if (checkManip == 2 && panelManipFR_Button_Bool == false)
		{
			if (support_FR_Horizontal_Bool == true)
			{
				support_FR_Horizontal.transform.localPosition = Vector3.MoveTowards(
					support_FR_Horizontal.transform.localPosition, new Vector3(1.221479f, -0.1305281f, -0.06621899f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}
			else if (support_FR_Horizontal_Bool == false)
			{
				support_FR_Horizontal.transform.localPosition = Vector3.MoveTowards(
					support_FR_Horizontal.transform.localPosition, new Vector3(2.622f, -0.131f, -0.06621899f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}

			SoundPitchManip();
		}
		else if (checkManip == 3 && panelManipFR_Button_Bool == false)
		{
			if (support_FR_Horizontal_Bool == true)
			{
				support_FR_Horizontal.transform.localPosition = Vector3.MoveTowards(
					support_FR_Horizontal.transform.localPosition, new Vector3(1.228052f, -0.1384991f, -0.04297502f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}
			else if (support_FR_Horizontal_Bool == false)
			{
				support_FR_Horizontal.transform.localPosition = Vector3.MoveTowards(
					support_FR_Horizontal.transform.localPosition, new Vector3(2.623f, -0.1384991f, -0.04297502f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}

			SoundPitchManip();
		}
	}

	//Lift Vertical FL
	public void Lift_Vertical_FL()
	{
		if (checkManip == 1 && panelManipFL_Button_Bool == false)
		{
			if (support_FL_Vertical_Bool == true)
			{
				support_FL_Vertical.transform.localPosition = Vector3.MoveTowards(
					support_FL_Vertical.transform.localPosition, new Vector3(0.04820915f, -1.00017f, 0.03412036f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}
			else if (support_FL_Vertical_Bool == false)
			{
				support_FL_Vertical.transform.localPosition = Vector3.MoveTowards(
					support_FL_Vertical.transform.localPosition, new Vector3(0.048f, -1.959f, 0.03412036f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}

			SoundPitchManip();
		}
		else if (checkManip == 2 && panelManipFL_Button_Bool == false)
		{
			if (support_FL_Vertical_Bool == true)
			{
				support_FL_Vertical.transform.localPosition = Vector3.MoveTowards(
					support_FL_Vertical.transform.localPosition, new Vector3(0.02299581f, -0.7332212f, 0.03411815f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}
			else if (support_FL_Vertical_Bool == false)
			{
				support_FL_Vertical.transform.localPosition = Vector3.MoveTowards(
					support_FL_Vertical.transform.localPosition, new Vector3(0.02299581f, -2.691f, 0.03411815f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}

			SoundPitchManip();
		}
		else if (checkManip == 3 && panelManipFL_Button_Bool == false)
		{
			if (support_FL_Vertical_Bool == true)
			{
				support_FL_Vertical.transform.localPosition = Vector3.MoveTowards(
					support_FL_Vertical.transform.localPosition, new Vector3(0.00499923f, -1.007683f, -0.01323524f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}
			else if (support_FL_Vertical_Bool == false)
			{
				support_FL_Vertical.transform.localPosition = Vector3.MoveTowards(
					support_FL_Vertical.transform.localPosition, new Vector3(0.00499923f, -1.96f, -0.01323524f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}

			SoundPitchManip();
		}
	}

	//Lift Vertical FR
	public void Lift_Vertical_FR()
	{
		if (checkManip == 1 && panelManipFR_Button_Bool == false)
		{
			if (support_FR_Vertical_Bool == true)
			{
				support_FR_Vertical.transform.localPosition = Vector3.MoveTowards(
					support_FR_Vertical.transform.localPosition, new Vector3(-0.04887562f, -1.00017f, 0.03412036f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}
			else if (support_FR_Vertical_Bool == false)
			{
				support_FR_Vertical.transform.localPosition = Vector3.MoveTowards(
					support_FR_Vertical.transform.localPosition, new Vector3(-0.04887562f, -1.906f, 0.03412036f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}

			SoundPitchManip();
		}
		else if (checkManip == 2 && panelManipFR_Button_Bool == false)
		{
			if (support_FR_Vertical_Bool == true)
			{
				support_FR_Vertical.transform.localPosition = Vector3.MoveTowards(
					support_FR_Vertical.transform.localPosition, new Vector3(-0.02433154f, -0.7332211f, 0.03411908f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}
			else if (support_FR_Vertical_Bool == false)
			{
				support_FR_Vertical.transform.localPosition = Vector3.MoveTowards(
					support_FR_Vertical.transform.localPosition, new Vector3(-0.02433154f, -2.691f, 0.03411908f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}

			SoundPitchManip();
		}
		else if (checkManip == 3 && panelManipFR_Button_Bool == false)
		{
			if (support_FR_Vertical_Bool == true)
			{
				support_FR_Vertical.transform.localPosition = Vector3.MoveTowards(
					support_FR_Vertical.transform.localPosition, new Vector3(-0.006394029f, -1.007683f, -0.01323454f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}
			else if (support_FR_Vertical_Bool == false)
			{
				support_FR_Vertical.transform.localPosition = Vector3.MoveTowards(
					support_FR_Vertical.transform.localPosition, new Vector3(-0.006394029f, -1.96f, -0.01323454f),
					m_ScriptM.speedLiftTruck * Time.deltaTime);
			}

			SoundPitchManip();
		}
	}
}