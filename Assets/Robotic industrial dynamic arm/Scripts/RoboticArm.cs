using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	// this is the audio source to play the arm sound
	public AudioSource audioS;

	void Start()
	{


	}

	// Update is called once per frame

	void FixedUpdate()
	{

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
}
