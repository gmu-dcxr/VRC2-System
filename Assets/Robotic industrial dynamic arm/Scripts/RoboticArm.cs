using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC2.Extention;

public class RoboticArm : MonoBehaviour
{

	// Use this for initialization

	//this are the parts of the robotic arm
	public Transform part0;
	public Transform part1;
	public Transform part2;
	public Transform part3;
	public Transform gripLeft;
	public Transform gripRight;
	
	public Transform attachPoint;

	// this is the audio source to play the arm sound
	public AudioSource audioS;

	#region Rotation backups

	private Quaternion rotation0;
	private Quaternion rotation1;
	private Quaternion rotation2;
	private Quaternion rotation3;
	private Quaternion rotationLeft;
	private Quaternion rotationRight;



	#endregion

	#region Grip / release status

	[HideInInspector]
	public float rightYRot
	{
		get
		{
			var rot = gripRight.localRotation.eulerAngles;
			return rot.y;
		}
	}

	[HideInInspector]
	public bool gripped
	{
		get => attachPoint.childCount != 0 && rightYRot <= 150;
	}

	[HideInInspector]
	public bool holdingObject
	{
		get => attachPoint.childCount != 0;
	}
	

	[HideInInspector]
	public bool needReleasing
	{
		get => attachPoint.childCount != 0 && rightYRot >= 180;
	}
    

	#endregion

	void Start()
	{
		rotation0 = Quaternion.Euler(270,0,0);
		rotation1 = Quaternion.Euler(0,270,0);
		rotation2 = Quaternion.Euler(0,0,0);
		rotation3 = Quaternion.Euler(90,90,0);
		rotationLeft = Quaternion.Euler(270,0,0);
		rotationRight = Quaternion.Euler(90,180,0);
	}

	// Update is called once per frame

	void FixedUpdate()
	{
		// var r1 = part0.localRotation.eulerAngles;
		// var r2 = part1.localRotation.eulerAngles;
		// var r3 = part2.localRotation.eulerAngles;
		// var r4 = part3.localRotation.eulerAngles;
		// var r5 = gripLeft.localRotation.eulerAngles;
		// var r6 = gripRight.localRotation.eulerAngles;
		//
		// print($"{r1} {r2} {r3} {r4} {r5} {r6}");
	}

	private void Update()
	{
		if (gripped)
		{
			var go = attachPoint.transform.GetChild(0);
			// force local position
			go.localPosition = Vector3.zero;
		}
	}

	public void rotatePart0(float val)
	{
		// between 0 and 360 degrees
		part0.localRotation = Quaternion.Euler(-90, val * 360, 0);
		if (audioS.isPlaying == false)
		{
			audioS.Play();
		}

	}

	public void rotatePart1(float val)
	{
		// between 0 and 360 degrees
		part1.localRotation = Quaternion.Euler(0, -90, val * 360);
		if (audioS.isPlaying == false)
		{
			audioS.Play();
		}

	}

	public void rotatePart2(float val)
	{
		// between 0 and 360 degrees
		part2.localRotation = Quaternion.Euler(0, 0, val * 360);
		if (audioS.isPlaying == false)
		{
			audioS.Play();
		}

	}

	public void rotatePart3(float val)
	{
		// between 0 and 360 degrees
		part3.localRotation = Quaternion.Euler(val * 360, 90, 0);
		if (audioS.isPlaying == false)
		{
			audioS.Play();
		}

	}


	public void grip(float val)
	{
		// between 0 and 360 degrees
		gripLeft.localRotation = Quaternion.Euler(-90, 0, 180 + val * 360);

		gripRight.localRotation = Quaternion.Euler(90, 0, val * 360);
		if (audioS.isPlaying == false)
		{
			audioS.Play();
		}
	}

	public void CloseGrip(float step)
	{
		var left = gripLeft.localRotation.eulerAngles;
		var right = gripRight.localRotation.eulerAngles;

		left.z += step;
		right.z += step;

		// make it not too small
		if (right.y < 150) return;

		gripLeft.localRotation = Quaternion.Euler(left);
		gripRight.localRotation = Quaternion.Euler(right);
		if (audioS.isPlaying == false)
		{
			audioS.Play();
		}
	}

	public void OpenGrip(float step)
	{
		var left = gripLeft.localRotation.eulerAngles;
		var right = gripRight.localRotation.eulerAngles;

		left.z -= step;
		right.z -= step;
		
		// make it not too big
		if (right.y > 230) return;

		gripLeft.localRotation = Quaternion.Euler(left);
		gripRight.localRotation = Quaternion.Euler(right);
		if (audioS.isPlaying == false)
		{
			audioS.Play();
		}
	}

	public void ResetRotations()
	{
		part0.localRotation = rotation0;
		part1.localRotation = rotation1;
		part2.localRotation = rotation2;
		part3.localRotation = rotation3;
		gripLeft.localRotation = rotationLeft;
		gripRight.localRotation = rotationRight;
	}

}
