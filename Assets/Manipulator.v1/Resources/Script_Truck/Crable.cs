using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crable : MonoBehaviour {

	[HideInInspector]
	public Manipulator m_ScriptManipulator;
	private bool crable_Bool = true;
	private Transform obj_Crable;

	void Start(){
		if(m_ScriptManipulator != null){
				m_ScriptManipulator.obl_Crable = transform.GetChild (0);
				m_ScriptManipulator.pisCrableUp = transform.GetChild (0).transform.GetChild (1);
				m_ScriptManipulator.pisCrableDown = transform.GetChild (1);
			m_ScriptManipulator.pisCrableDown_Parent = transform;
			m_ScriptManipulator.pisCrableUp_Parent = transform.GetChild (0);
		}
}
	public void AddCrable(){
		if (crable_Bool == true) {
			FixedJoint fix = gameObject.AddComponent<FixedJoint> ();
			fix.connectedBody = m_ScriptManipulator.arrowForward5.GetComponent<Rigidbody>();
			fix.enablePreprocessing = false;
			crable_Bool = false;
		} else if (crable_Bool == false) {
			Destroy (gameObject.GetComponent<FixedJoint> ());
			Destroy (gameObject.GetComponent<Rigidbody> ());
			transform.localPosition = new Vector3 (0,0,0);
			transform.localRotation = Quaternion.Euler (0,0,0);
			crable_Bool = true;
		}
	}
	void OnCollisionEnter(Collision col){
		if (Input.GetKey (m_ScriptManipulator.m_ScriptM.rotationArrowLeft)) {
			m_ScriptManipulator.block_Left = false;
			m_ScriptManipulator.StartCoroutine ("BlockArrow");
		}
		if (Input.GetKey (m_ScriptManipulator.m_ScriptM.rotationArrowRight)) {
			m_ScriptManipulator.block_Right = false;
			m_ScriptManipulator.StartCoroutine ("BlockArrow");
		}
		if (Input.GetKey (m_ScriptManipulator.m_ScriptM.forwardArrow)) {
			m_ScriptManipulator.block_For = false;
			m_ScriptManipulator.StartCoroutine ("BlockArrow");
		}
		if (Input.GetKey (m_ScriptManipulator.m_ScriptM.backArrow)) {
			m_ScriptManipulator.block_Back = false;
			m_ScriptManipulator.StartCoroutine ("BlockArrow");
		}
		if (Input.GetKey (m_ScriptManipulator.m_ScriptM.DownArrow)) {
			m_ScriptManipulator.block_Down = false;
			m_ScriptManipulator.StartCoroutine ("BlockArrow");
		}
	}
	void OnCollisionExit(Collision col){
		if (m_ScriptManipulator.block_Left == false) {
			m_ScriptManipulator.block_Left = true;
		}
		if (m_ScriptManipulator.block_Right == false) {
			m_ScriptManipulator.block_Right = true;
		}
		if (m_ScriptManipulator.block_For == false) {
			m_ScriptManipulator.block_For = true;
		}
		if (m_ScriptManipulator.block_Back == false) {
			m_ScriptManipulator.block_Back = true;
		}
		if (m_ScriptManipulator.block_Down == false) {
			m_ScriptManipulator.block_Down = true;
		}
	}









}

