using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToManipulator : MonoBehaviour {
	[HideInInspector]
	public Transform target;
	public float distance = 12f;
	private float x = 0f;
	private float y = 0f;
	float xSpeed= 250f;
	float  ySpeed= 120f;
	private float yMinLi= -30f;
	private float yMaxLi= 85f;
	[HideInInspector]
	public bool ifDownKey_Bool = true;
	public Texture2D cursorT;
	[HideInInspector]
	public CursorMode cursorM = CursorMode.Auto;
	[HideInInspector]
	public Vector2 hotSpot = Vector2.zero;
	public KeyCode mouseKey = KeyCode.H;

	void Start(){

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	void  LateUpdate (){
		x += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;
		y -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;
		y = ClampAngle (y, yMinLi, yMaxLi);
		Quaternion rotation = Quaternion.Euler (y, x, 0);
		Vector3 position = rotation * new Vector3 (0f, 0f, -distance) + target.position;

		transform.rotation = rotation;
		transform.position = position;

		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			distance--;
		} else if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			distance++;
		}
		if (Input.GetKeyDown (mouseKey)) {
			xSpeed = 0.0f;
			ySpeed = 0.0f;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			Cursor.SetCursor (cursorT, hotSpot, cursorM);	
			ifDownKey_Bool = false;
		} else if (Input.GetKeyUp (mouseKey)) {
			xSpeed = 250f;
			ySpeed = 120f;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			Cursor.SetCursor (null, Vector2.zero, cursorM);
			ifDownKey_Bool = true;
		}
	}
	static float ClampAngle ( float angle ,   float min ,   float max  ){

		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}
















}
