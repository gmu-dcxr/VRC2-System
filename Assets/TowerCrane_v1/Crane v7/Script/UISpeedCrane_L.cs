using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpeedCrane_L : MonoBehaviour {

	static float minSpeed = 28f;
	static float maxSpeed = -150.3f;
	static UISpeedCrane_L thisSpeed;

	void Start(){
		thisSpeed = this;
	}
	public static void ShowSpeed(float speed,float min,float max){
    float ang = Mathf.Lerp (minSpeed,maxSpeed, Mathf.InverseLerp (min, max, speed));
//    thisSpeed.transform.eulerAngles = new Vector3 (0,0,ang);
	}
}
