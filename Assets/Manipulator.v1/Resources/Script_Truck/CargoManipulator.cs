using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VRC2.Animations.CraneTruck;

public class CargoManipulator : MonoBehaviour {

	public KeyCode connectedCargo = KeyCode.C;
	[HideInInspector]
	public bool connectedCargoToHook_Bool = true;
	[HideInInspector]
	public Transform hook;
	[HideInInspector]
	public Transform pointHook;
	[HideInInspector]
	public float distanceHook = 0;
	public float massCagro;
	public float speedRotation = 0;
	public KeyCode onRotationCargo = KeyCode.LeftControl;
	public KeyCode leftRotation = KeyCode.Mouse0;
	public KeyCode RightRotation = KeyCode.Mouse1;
	private bool rotationCargo_Bool = false;
	private float motorRot = 0;
	private float targetVelosityPlatform = 30;
	public Transform lineRen1;
	public Transform lineRen2;
	public Transform lineRen3;
	public Transform lineRen4;
	public Material lineMaterial;
	public float lineStartWidth = 0;
	public float lineEndWidth = 0;
	private LineRenderer m_lineCargo;
	private Transform bodyTruck;
	private bool stopCarutine_Bool = true;
	[Header("The time after which the Cargo will be attached to the Truck is indicated")]
	public float connectedSecond = 0f;
	
	[Space(30)]
	[Header("Recording/Replay")]
	#region Hack for recording

	private CraneTruckInputActions craneTIA;
	private InputAction craneSeizeInput;


	public CraneTruckInputRecording recording;

	#endregion

	void Start(){
		
		// initialize input actions
		craneTIA = new CraneTruckInputActions();
		craneTIA.Enable();
		
		craneSeizeInput = craneTIA.Crane.Seize;
		
		if (gameObject.GetComponent<Rigidbody> () == null) {
			gameObject.AddComponent<Rigidbody> ();
		}
		gameObject.GetComponent<Rigidbody> ().mass = massCagro;
		lineMaterial = Resources.Load ("Material/LineMat", typeof(Material))as Material;
	}
	void Update(){
		if (hook != null)
		{
			if (recording.truckMode) return;
			
			// if (Input.GetKeyDown (connectedCargo) && hook.GetComponent<HookManip> ().m_ScriptHook_2.connectedCargoIm.enabled == true && gameObject.GetComponent<HingeJoint>() == null) {
			if (craneSeizeInput.triggered && hook.GetComponent<HookManip> ().m_ScriptHook_2.connectedCargoIm.enabled == true && gameObject.GetComponent<HingeJoint>() == null) {
				ConnectedCargoToHook ();
			}else
			// if (Input.GetKeyDown (connectedCargo) && gameObject.GetComponent<HingeJoint>() != null) {
			if (craneSeizeInput.triggered && gameObject.GetComponent<HingeJoint>() != null) {
				ConnectedCargoToHook ();
			}
		}
		RotationCargo ();
		if (lineRen1 != null || lineRen2 != null || lineRen3 != null || lineRen4 != null) {
			LineRendererCargo ();
		}
	}
	void FixedUpdate(){
		if (connectedCargoToHook_Bool == false) {
			if (gameObject.GetComponent<HingeJoint> () != null) {
				gameObject.GetComponent<HingeJoint> ().connectedAnchor = pointHook.localPosition;
			}
				//Rotation Cargo
			if (rotationCargo_Bool == true) {
				HingeJoint hin = gameObject.GetComponent<HingeJoint> ();
				JointMotor mot = new JointMotor ();
				mot.targetVelocity = motorRot;
				mot.force = speedRotation;
				hin.motor = mot;
				hin.useMotor = true;
			} else if (rotationCargo_Bool == false) {
				if (gameObject.GetComponent<HingeJoint> () != null) {
					gameObject.GetComponent<HingeJoint> ().useMotor = false;
				}
			}
		}
	}
	public void ConnectedCargoToHook(){
		if (hook.GetComponent<HookManip>().nameCargo == gameObject.name && connectedCargoToHook_Bool == true) {
				transform.parent = null;
				if (gameObject.GetComponent<Rigidbody> () == null) {
					Rigidbody rig = gameObject.AddComponent<Rigidbody> ();
					rig.mass = massCagro;
				}
				gameObject.GetComponent<Rigidbody> ().drag = 2;	
				HingeJoint hin = gameObject.AddComponent<HingeJoint> ();
				hin.connectedBody = hook.GetComponent<Rigidbody> ();
				hin.axis = new Vector3 (0, 1, 0);
				hin.anchor = new Vector3 (0, distanceHook, 0);
				hin.autoConfigureConnectedAnchor = false;
				gameObject.layer = 8;
				foreach (Transform layerCargo in GetComponentInChildren<Transform>(true)) {
					layerCargo.gameObject.layer = 8;
				}
				if (hook.GetComponent<HookManip> ().m_ScriptHook_2.connectedCargoIm.enabled == true) {
					hook.GetComponent<HookManip> ().m_ScriptHook_2.connectedCargoIm.enabled = false;
				}
				if (gameObject.GetComponent<ConstantForce> () != null) {
					Destroy (gameObject.GetComponent<ConstantForce> ());
				}
				if (lineRen1 != null || lineRen2 != null || lineRen3 != null || lineRen4 != null) {
					pointHook.gameObject.AddComponent<LineRenderer> ();
					m_lineCargo = pointHook.GetComponent<LineRenderer> ();
				}
				if (hook.GetComponent<HookManip> () != null) {
					hook.GetComponent<HookManip> ().m_Cargo = gameObject.GetComponent<CargoManipulator> ();
				}
				hook.GetComponent<HookManip> ().m_ScriptHook_2.blockOnManip_Cargo = false;
				if (bodyTruck != null) {
					hook.GetComponent<HookManip> ().m_ScriptHook_2.rig.mass -= massCagro;
				}
				stopCarutine_Bool = true;
				connectedCargoToHook_Bool = false;
			} else if (connectedCargoToHook_Bool == false) {
			m_lineCargo = null;
			hook.GetComponent<HookManip>().m_ScriptHook_2.blockOnManip_Cargo = true;
			Destroy (pointHook.GetComponent<LineRenderer> ());
			Destroy (gameObject.GetComponent<HingeJoint> ());
			ConstantForce cons = gameObject.AddComponent<ConstantForce> ();
			cons.force = new Vector3 (0, -0.02f, 0);
			gameObject.layer = 9;
			foreach (Transform m_layer in GetComponentInChildren<Transform>(true)) {
				m_layer.gameObject.layer = 0;
			}
			if (hook.GetComponent<HookManip> () != null) {
				hook.GetComponent<HookManip> ().m_Cargo = null;
			}
				connectedCargoToHook_Bool = true;
			hook.GetComponent<HookManip> ().onCol_Cargo = true;
		}
	}
	IEnumerator OnBody(){
		yield return new WaitForSeconds (connectedSecond);
		m_lineCargo = null;
			Destroy (pointHook.GetComponent<LineRenderer> ());
			gameObject.layer = 9;
			foreach (Transform m_layer in GetComponentInChildren<Transform>(true)) {
				m_layer.gameObject.layer = 0;
			}
			Destroy (gameObject.GetComponent<HingeJoint> ());
			Destroy (gameObject.GetComponent<ConstantForce> ());
			Destroy (gameObject.GetComponent<Rigidbody> ());
		bodyTruck.GetComponentInParent<ControllerTruck> ().rig.mass += massCagro;	
		transform.parent = bodyTruck;
		if (hook != null) {
			hook = null;
		}
		if (pointHook != null) {
			pointHook = null;
		}
	}
	public void RotationCargo(){
		if (connectedCargoToHook_Bool == false) {
			if (Input.GetKey (onRotationCargo) && Input.GetKey (leftRotation)) {
				rotationCargo_Bool = true;
				motorRot = +targetVelosityPlatform;
			} else if (Input.GetKey (onRotationCargo) && Input.GetKey (RightRotation)) {
				rotationCargo_Bool = true;
				motorRot = -targetVelosityPlatform;
			} else if (Input.GetKeyUp (onRotationCargo)) {
				rotationCargo_Bool = false;
			}
		}
	}
	public void LineRendererCargo(){
		if (connectedCargoToHook_Bool == false) {
			m_lineCargo.startWidth = lineStartWidth;
			m_lineCargo.endWidth = lineEndWidth;
			Vector3[] line = new Vector3[8];
				line [0] = new Vector3 (pointHook.position.x, pointHook.position.y, pointHook.position.z);
				line [1] = new Vector3 (lineRen1.position.x, lineRen1.position.y, lineRen1.position.z);
				line [2] = new Vector3 (pointHook.position.x, pointHook.position.y, pointHook.position.z);
				line [3] = new Vector3 (lineRen2.position.x, lineRen2.position.y, lineRen2.position.z);
				line [4] = new Vector3 (pointHook.position.x, pointHook.position.y, pointHook.position.z);
				line [5] = new Vector3 (lineRen3.position.x, lineRen3.position.y, lineRen3.position.z);
				line [6] = new Vector3 (pointHook.position.x, pointHook.position.y, pointHook.position.z);
				line [7] = new Vector3 (lineRen4.position.x, lineRen4.position.y, lineRen4.position.z);
			m_lineCargo.positionCount = line.Length;
			m_lineCargo.SetPositions (line);
			m_lineCargo.material = lineMaterial;
		}
	}
	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.GetComponent<MeshRenderer> ().sharedMaterial.name == "Body_diffuse") {
			bodyTruck = coll.transform;
		}
	}
	void OnTriggerStay(Collider coll){
		if(stopCarutine_Bool == true){
		if (coll.gameObject.GetComponent<MeshRenderer> ().sharedMaterial.name == "Body_diffuse") {
			if (gameObject.GetComponent<HingeJoint> () == null && bodyTruck != null) {
				StartCoroutine ("OnBody");
					stopCarutine_Bool = false;
				}
		}
	}
}
	void OnTriggerExit(Collider coll){
		bodyTruck = null;
	}	
	void OnCollisionEnter(Collision coll_H){
		if (connectedCargoToHook_Bool == false) {
			hook.GetComponent<HookManip> ().onCol_Cargo = false;
		}
	}
	void OnCollisionExit(Collision col_H){
		if (connectedCargoToHook_Bool == false) {
			hook.GetComponent<HookManip> ().onCol_Cargo = true;
		}
	}
}
