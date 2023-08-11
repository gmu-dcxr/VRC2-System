using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOnCollider : MonoBehaviour {
	[HideInInspector]
	public Manipulator m_ScriptManip;

	void OnCollisionEnter(Collision col){
		if (Input.GetKey (m_ScriptManip.m_ScriptM.rotationArrowLeft)) {
			m_ScriptManip.block_Left = false;
			m_ScriptManip.StartCoroutine ("BlockArrow");
		}
		if (Input.GetKey (m_ScriptManip.m_ScriptM.rotationArrowRight)) {
			m_ScriptManip.block_Right = false;
			m_ScriptManip.StartCoroutine ("BlockArrow");
		}
		if (Input.GetKey (m_ScriptManip.m_ScriptM.forwardArrow)) {
			m_ScriptManip.block_For = false;
			m_ScriptManip.StartCoroutine ("BlockArrow");
		}
		if (Input.GetKey (m_ScriptManip.m_ScriptM.backArrow)) {
			m_ScriptManip.block_Back = false;
			m_ScriptManip.StartCoroutine ("BlockArrow");
		}
		if (Input.GetKey (m_ScriptManip.m_ScriptM.DownArrow)) {
			m_ScriptManip.block_Down = false;
			m_ScriptManip.StartCoroutine ("BlockArrow");
		}
	}
	void OnCollisionExit(Collision col){
		if (m_ScriptManip.block_Left == false) {
			m_ScriptManip.block_Left = true;
		}
		if (m_ScriptManip.block_Right == false) {
			m_ScriptManip.block_Right = true;
		}
		if (m_ScriptManip.block_For == false) {
			m_ScriptManip.block_For = true;
		}
		if (m_ScriptManip.block_Back == false) {
			m_ScriptManip.block_Back = true;
		}
		if (m_ScriptManip.block_Down == false) {
			m_ScriptManip.block_Down = true;
		}
	}
}
