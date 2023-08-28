using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VRC2.Animations.CraneTruck;

public class HookManip : MonoBehaviour
{
	private Material matLine;
	[HideInInspector] public CargoManipulator m_Cargo;
	[HideInInspector] public Manipulator m_ScriptHook_1;
	[HideInInspector] public ControllerTruck m_ScriptHook_2;
	private bool hook_Bool = true;
	private float limitHook = 0f;
	private bool onCol = true;
	[HideInInspector] public bool onCol_Cargo = true;
	private Transform lineRen1A;
	private Transform lineRen2A;
	private Transform lineRen1B;
	private Transform lineRen2B;
	[HideInInspector] public Transform lineArrow1For;
	[HideInInspector] public Transform lineArrow2For;
	[HideInInspector] public Transform decalHook;
	private float distanceHook_float = 0;
	[HideInInspector] public float distanceHook_Int = 0;
	[HideInInspector] public string nameCargo;

	#region Hack for recording/replay


	private CraneTruckInputActions craneTIA;

	// private InputAction craneArrowInput;
	private InputAction craneArrowInput;
	// private InputAction craneSeizeInput;


	public CraneTruckInputRecording recording;

	#endregion

	void Start()
	{

		// initialize input actions
		craneTIA = new CraneTruckInputActions();
		craneTIA.Enable();

		// craneArrowInput = craneTIA.Crane.Move;
		craneArrowInput = craneTIA.Crane.Arrow;
		// craneSeizeInput = craneTIA.Crane.Seize;

		if (m_ScriptHook_2 != null)
		{
			if (m_ScriptHook_1.m_ScriptM.startTruck == true)
			{
				if (m_ScriptHook_1.checkManip == 1 || m_ScriptHook_1.checkManip == 2)
				{
					lineRen1A = m_ScriptHook_1.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0)
						.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0);
					lineRen2A = m_ScriptHook_1.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0)
						.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1);
					lineRen1B = transform.GetChild(0);
					lineRen2B = transform.GetChild(1);
					transform.parent = m_ScriptHook_1.arrowForward4;
					decalHook = transform.GetChild(3);
				}
				else if (m_ScriptHook_1.checkManip == 3)
				{
					lineRen1A = m_ScriptHook_1.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0)
						.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0);
					lineRen2A = m_ScriptHook_1.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0)
						.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1);
					lineRen1B = transform.GetChild(0);
					lineRen2B = transform.GetChild(1);
					transform.parent = m_ScriptHook_1.arrowForward5;
					decalHook = transform.GetChild(3);
				}

				if (m_ScriptHook_2 != null)
				{
					LineRen();
				}

				matLine = Resources.Load("Material/LineMat", typeof(Material)) as Material;
				decalHook.eulerAngles = new Vector3(90, 0, 0);
				decalHook.localScale = new Vector3(0.48f, 0.48f, 0.48f);
				m_ScriptHook_2.m_ScriptHook = gameObject.GetComponent<HookManip>();
			}

			m_ScriptHook_1.checkDistanceHook_A = lineRen1A.transform;
			m_ScriptHook_1.checkDistanceHook_B = lineRen1B.transform;
		}
	}

	void Update()
	{
		// return in truck mode
		if (recording.truckMode) return;

		if (m_ScriptHook_2 != null)
		{
			if (m_ScriptHook_2.fps_Bool == false)
			{
				if (m_ScriptHook_1.m_ScriptM.startTruck == true)
				{
					if (m_ScriptHook_2.onManip == false)
					{
						MoveHook();
						MoveLine();
						if (Input.GetKeyDown(m_ScriptHook_2.powrManipulator) && distanceHook_Int != 0)
						{
							m_ScriptHook_1.StartCoroutine("BlockOnPower");
						}
					}

					LineHook();
					// Decal
					if (hook_Bool == false)
					{
						Debug.DrawRay(transform.position, -Vector3.up * 1000, Color.red);
						Ray rayDecal = new Ray(transform.position, -Vector3.up);
						int layerIgnore = ~(1 << 8);
						int layerCargo = (1 << 9);
						RaycastHit hit;
						if (Physics.Raycast(rayDecal, out hit, 1000, layerIgnore, QueryTriggerInteraction.Ignore))
						{
							decalHook.position = hit.point + hit.normal * 0.01f;
							decalHook.rotation = Quaternion.LookRotation(-hit.normal);
						}

						//Cargo
						RaycastHit hitCargo;
						if (Physics.Raycast(rayDecal, out hitCargo, 3.6f, layerCargo, QueryTriggerInteraction.Ignore))
						{
							if (hitCargo.distance > 2.7f)
							{
								// hack
								// m_ScriptHook_2.connectedCargoIm.enabled = false;
							}
							else if (hitCargo.distance < 2.5f)
							{
								// hack
								// m_ScriptHook_2.connectedCargoIm.enabled = false;
							}
							else
							{
								// hack
								// m_ScriptHook_2.connectedCargoIm.enabled = true;
							}

							if (m_ScriptHook_2.connectedCargoIm.enabled == true)
							{
								hit.collider.GetComponent<CargoManipulator>().pointHook = transform.GetChild(4);
								hit.collider.GetComponent<CargoManipulator>().hook = transform;
								hit.collider.GetComponent<CargoManipulator>().distanceHook = hitCargo.distance;
							}

							nameCargo = hitCargo.collider.name;
						}
						else
						{
							// hack
							// m_ScriptHook_2.connectedCargoIm.enabled = false;
						}
					}
				}
			}

			if (hook_Bool == false)
			{
				if (m_ScriptHook_1.uiManip_Bool == false)
				{
					//Check Distance Hook
					distanceHook_float = Vector3.Distance(m_ScriptHook_1.checkDistanceHook_A.position,
						m_ScriptHook_1.checkDistanceHook_B.position);
					distanceHook_Int = (int)(distanceHook_float * 3.6f);
					m_ScriptHook_1.hook_Text.text = "" + distanceHook_Int.ToString();
				}

				if (distanceHook_Int != 0)
				{
					m_ScriptHook_2.blockOnManip = false;
				}
				else
				{
					m_ScriptHook_2.blockOnManip = true;
				}
			}
		}
	}

	void FixedUpdate()
	{
		if (m_ScriptHook_2 != null)
		{
			if (m_ScriptHook_2.fps_Bool == false)
			{
				if (m_ScriptHook_1.m_ScriptM.startTruck == true)
				{
					if (hook_Bool == false)
					{
						ConfigurableJoint hJ = gameObject.GetComponent<ConfigurableJoint>();
						SoftJointLimit lj = new SoftJointLimit();
						lj.limit = limitHook;
						hJ.linearLimit = lj;
					}
				}
			}
		}
	}

	void OnCollisionEnter(Collision coll)
	{
		onCol = false;
	}

	void OnCollisionExit(Collision col)
	{
		onCol = true;
	}

	public void HookController()
	{
		if (hook_Bool == true)
		{
			transform.parent = m_ScriptHook_2.transform;
			Rigidbody rig = gameObject.AddComponent<Rigidbody>();
			rig.mass = 20;
			rig.drag = 1.1f;
			ConstantForce m_force = gameObject.AddComponent<ConstantForce>();
			m_force.force = new Vector3(0, -0.1f, 0);
			ConfigurableJoint joinHook = gameObject.AddComponent<ConfigurableJoint>();
			joinHook.xMotion = ConfigurableJointMotion.Locked;
			joinHook.yMotion = ConfigurableJointMotion.Limited;
			joinHook.zMotion = ConfigurableJointMotion.Locked;
			joinHook.angularYMotion = ConfigurableJointMotion.Locked;
			if (m_ScriptHook_1.checkManip == 1 || m_ScriptHook_1.checkManip == 2)
			{
				joinHook.anchor = new Vector3(0, 0.18f, 0);
				joinHook.connectedBody = m_ScriptHook_1.arrowForward4.gameObject.GetComponent<Rigidbody>();
				joinHook.connectedAnchor = new Vector3(-0.01639175f, 1.575f, -3.996238f);
			}
			else if (m_ScriptHook_1.checkManip == 3)
			{
				joinHook.anchor = new Vector3(0, 0.36f, 0);
				joinHook.connectedBody = m_ScriptHook_1.arrowForward5.gameObject.GetComponent<Rigidbody>();
				joinHook.connectedAnchor = new Vector3(0.006637573f, 1.536f, -3.799213f);
			}

			joinHook.autoConfigureConnectedAnchor = false;
			decalHook.gameObject.SetActive(true);
			hook_Bool = false;
		}
		else if (hook_Bool == false && m_ScriptHook_2.blockOnManip_Cargo == true)
		{
			Destroy(gameObject.GetComponent<ConfigurableJoint>());
			Destroy(gameObject.GetComponent<ConstantForce>());
			Destroy(gameObject.GetComponent<Rigidbody>());
			decalHook.gameObject.SetActive(false);
			if (m_ScriptHook_1.checkManip == 1 || m_ScriptHook_1.checkManip == 2)
			{
				transform.parent = m_ScriptHook_1.arrowForward4;
			}
			else if (m_ScriptHook_1.checkManip == 3)
			{
				transform.parent = m_ScriptHook_1.arrowForward5;
			}

			hook_Bool = true;
		}
	}

	public void MoveHook()
	{
		// if (Input.GetKey (m_ScriptHook_2.UpHook) && Input.GetKey (m_ScriptHook_2.hook) && hook_Bool == false && limitHook > 0) {
		if (craneArrowInput.ReadValue<Vector2>().y > 0 && hook_Bool == false && limitHook > 0)
		{
			limitHook -= Time.deltaTime * m_ScriptHook_2.speedHook;
			m_ScriptHook_1.SoundPitchManip();
			// } else if (Input.GetKey (m_ScriptHook_2.DownHook) && Input.GetKey (m_ScriptHook_2.hook) && hook_Bool == false && onCol == true && onCol_Cargo == true) {
		}
		else if (craneArrowInput.ReadValue<Vector2>().y < 0 && hook_Bool == false && onCol == true &&
		         onCol_Cargo == true)
		{
			limitHook += Time.deltaTime * m_ScriptHook_2.speedHook;
			m_ScriptHook_1.SoundPitchManip();
		}
	}

	public void LineRen()
	{
		lineRen1A.gameObject.AddComponent<LineRenderer>();
		lineRen2A.gameObject.AddComponent<LineRenderer>();
		if (m_ScriptHook_1.checkManip == 1 || m_ScriptHook_1.checkManip == 2)
		{
			lineArrow1For.gameObject.AddComponent<LineRenderer>();
		}
	}

	public void LineHook()
	{
		lineRen1A.GetComponent<LineRenderer>().startWidth = 0.015f;
		lineRen1A.GetComponent<LineRenderer>().endWidth = 0.015f;
		Vector3[] line1 = new Vector3[2];
		line1[0] = new Vector3(lineRen1A.position.x, lineRen1A.position.y, lineRen1A.position.z);
		line1[1] = new Vector3(lineRen1B.position.x, lineRen1B.position.y, lineRen1B.position.z);
		lineRen1A.GetComponent<LineRenderer>().positionCount = line1.Length;
		lineRen1A.GetComponent<LineRenderer>().SetPositions(line1);
		lineRen1A.GetComponent<LineRenderer>().material = matLine;

		lineRen2A.GetComponent<LineRenderer>().startWidth = 0.015f;
		lineRen2A.GetComponent<LineRenderer>().endWidth = 0.015f;
		Vector3[] line2 = new Vector3[2];
		line2[0] = new Vector3(lineRen2A.position.x, lineRen2A.position.y, lineRen2A.position.z);
		line2[1] = new Vector3(lineRen2B.position.x, lineRen2B.position.y, lineRen2B.position.z);
		lineRen2A.GetComponent<LineRenderer>().positionCount = line2.Length;
		lineRen2A.GetComponent<LineRenderer>().SetPositions(line2);
		lineRen2A.GetComponent<LineRenderer>().material = matLine;

		if (m_ScriptHook_1.checkManip == 1 || m_ScriptHook_1.checkManip == 2)
		{
			lineArrow1For.GetComponent<LineRenderer>().startWidth = 0.015f;
			lineArrow1For.GetComponent<LineRenderer>().endWidth = 0.015f;
			Vector3[] lineFor = new Vector3[2];
			lineFor[0] = new Vector3(lineArrow1For.position.x, lineArrow1For.position.y, lineArrow1For.position.z);
			lineFor[1] = new Vector3(lineArrow2For.position.x, lineArrow2For.position.y, lineArrow2For.position.z);
			lineArrow1For.GetComponent<LineRenderer>().positionCount = lineFor.Length;
			lineArrow1For.GetComponent<LineRenderer>().SetPositions(lineFor);
			lineArrow1For.GetComponent<LineRenderer>().material = matLine;
		}
	}

	public void MoveLine()
	{
		if (m_ScriptHook_1.checkManip == 1 || m_ScriptHook_1.checkManip == 2)
		{
			// if (Input.GetKey (m_ScriptHook_2.DownArrow)) {
			if (craneArrowInput.ReadValue<Vector2>().y < 0)
			{
				lineRen2A.transform.localPosition = Vector3.MoveTowards(lineRen2A.transform.localPosition,
					new Vector3(-0.006336521f, -0.1862f, -3.9628f), 0.01f * Time.deltaTime);
				// }else	if (Input.GetKey (m_ScriptHook_2.UpArrow)) {
			}
			else if (craneArrowInput.ReadValue<Vector2>().y > 0)
			{
				lineRen2A.transform.localPosition = Vector3.MoveTowards(lineRen2A.transform.localPosition,
					new Vector3(-0.006336521f, -0.237f, -3.9628f), 0.01f * Time.deltaTime);
			}
		}
		else if (m_ScriptHook_1.checkManip == 3)
		{
			// if (Input.GetKey (m_ScriptHook_2.DownArrow)) {
			if (craneArrowInput.ReadValue<Vector2>().y < 0)
			{
				lineRen2A.transform.localPosition = Vector3.MoveTowards(lineRen2A.transform.localPosition,
					new Vector3(0.01227948f, -0.1604001f, -0.1042f), 0.01f * Time.deltaTime);
				// }else	if (Input.GetKey (m_ScriptHook_2.UpArrow)) {
			}
			else if (craneArrowInput.ReadValue<Vector2>().y > 0)
			{
				lineRen2A.transform.localPosition = Vector3.MoveTowards(lineRen2A.transform.localPosition,
					new Vector3(0.01227948f, -0.207f, -0.1042f), 0.01f * Time.deltaTime);
			}
		}
	}
}