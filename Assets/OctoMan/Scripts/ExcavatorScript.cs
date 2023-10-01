using UnityEngine;
using System.Collections;

public class ExcavatorScript : MonoBehaviour {

	//Animator
	public Animator anim;
	public float rotSpeed = 30f;
	public float driveSpeed = 2f;
	//Door
	public bool opened = false;

	public bool InDriveMode = true;
	//Animate UV'S
	public float scrollSpeed = 0.5f;

	public float offsetL;
	public float offsetR;

	public bool U = false;
	public bool V = true;

	public Material matL;
	public Material matR;

	//Treads
	public GameObject TreadsL;
	public GameObject TreadsR;

	//Weight Points - determines the rotation and movement axis of the Excavator
	public GameObject leftTread;
	public GameObject rightTread;

	//Big Wheels
	public GameObject WheelFrontLeft;
	public GameObject WheelFrontRight;

	public GameObject WheelBackLeft;
	public GameObject WheelBackRight;


	void Start()
	{
		// Materials for the Treads
		matL = TreadsL.GetComponent<Renderer> ().material;
		matR = TreadsR.GetComponent<Renderer> ().material;

		//set the bigarm to a non colliding position
		anim.SetFloat("BigArmSpeed",10f);
		anim.Play("BigOpen", 0 , (1/30)*5);
	}

	void Update() 
	{
		return;
		//if(!InDriveMode)
		//{
			//-------------------------------------------------BIG ARM-----------------------------------------------------------------
			if (Input.GetKey(KeyCode.Alpha0) && !Input.GetKey(KeyCode.Alpha9) && anim.GetInteger("BigArmPosition")!=2)
			{
				anim.SetInteger("BigArmPosition",1);
				anim.SetFloat("BigArmSpeed",1f);
			}
			else if (!Input.GetKey(KeyCode.Alpha9) && Input.GetKey(KeyCode.Alpha0) && anim.GetInteger("BigArmPosition")!=0)
			{
				anim.SetInteger("BigArmPosition",1);
				anim.SetFloat("BigArmSpeed", -1f);
			}
			else
			{
				anim.SetFloat("BigArmSpeed", 0);
			}

			//-------------------------------------------------------SMALL ARM-------------------------------------------------------------
			if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && anim.GetInteger("SmallArmPosition")!=2)
			{
				anim.SetInteger("SmallArmPosition",1);
				anim.SetFloat("SmallArmSpeed",1f);
			}
			else if (!Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow) && anim.GetInteger("SmallArmPosition")!=0)
			{
				anim.SetInteger("SmallArmPosition",1);
				anim.SetFloat("SmallArmSpeed", -1f);
			}
			else
			{
				anim.SetFloat("SmallArmSpeed", 0);
			}

			//----------------------------------------------------------SHOVEL-----------------------------------------------------------------
			if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)  && anim.GetInteger("ShovelPosition")!=2)
			{
				anim.SetInteger("ShovelPosition",1);
				anim.SetFloat("ShovelSpeed", 1f);
			}
			else if (!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)  && anim.GetInteger("ShovelPosition")!=0)
			{
				anim.SetInteger("ShovelPosition",1);
				anim.SetFloat("ShovelSpeed", -1f);
			}
			else
			{
				anim.SetFloat("ShovelSpeed", 0);
			}

			//---------------------------------------------------------ROTATE BODY----------------------------------------------------------
			if (Input.GetKey(KeyCode.Alpha1) && !Input.GetKey(KeyCode.Alpha2))
			{
				anim.SetFloat("RotateSpeed", 0.5f);
			}
			else if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
			{
				anim.SetFloat("RotateSpeed", -0.5f);
			}
			else
			{
				anim.SetFloat("RotateSpeed", 0f);
			}
		
		//}

		//---------------------------------------------------------DRIVE MODE--------------------------------------------------------------
		//if (InDriveMode)
		//{
			//ANIMATE RIGHT TREAD
			if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.A))
			{
				transform.RotateAround(leftTread.transform.position, -Vector3.up, Time.deltaTime * rotSpeed);
				offsetR = Time.time * scrollSpeed % 1;
				WheelFrontRight.transform.Rotate(Vector3.forward * Time.deltaTime *rotSpeed *4);
				WheelBackRight.transform.Rotate(Vector3.forward * Time.deltaTime *rotSpeed *4);

			}

			if (!Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.A))
			{
				transform.RotateAround(leftTread.transform.position, Vector3.up, Time.deltaTime * rotSpeed);
				offsetR = Time.time * -scrollSpeed % 1;
				WheelFrontRight.transform.Rotate(-Vector3.forward * Time.deltaTime *rotSpeed *4);
				WheelBackRight.transform.Rotate(-Vector3.forward * Time.deltaTime *rotSpeed *4);
			}

			//ANIMATE LEFT TREAD
			if (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.D))
			{
				transform.RotateAround(rightTread.transform.position, Vector3.up, Time.deltaTime * rotSpeed);
				offsetL = Time.time * scrollSpeed % 1;
				WheelFrontLeft.transform.Rotate(-Vector3.forward * Time.deltaTime *rotSpeed *4);
				WheelBackLeft.transform.Rotate(-Vector3.forward * Time.deltaTime *rotSpeed *4);
			}

			if (!Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.D))
			{
				transform.RotateAround(rightTread.transform.position, -Vector3.up, Time.deltaTime * rotSpeed);
				offsetL = Time.time * -scrollSpeed % 1;
				WheelFrontLeft.transform.Rotate(Vector3.forward * Time.deltaTime *rotSpeed *4);
				WheelBackLeft.transform.Rotate(Vector3.forward * Time.deltaTime *rotSpeed *4);
			}
		//}

		//------------------------------------------------------DOOR OPEN / CLOSE-----------------------------------------------------
		if (Input.GetKeyDown(KeyCode.F))
		{
			opened = !opened;
			anim.SetBool("DoorOpen", opened);
		}

		//-----------------------------------------------Switch Drive Mode/ Work Mode-------------------------------------------------
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			InDriveMode = !InDriveMode;
		}

		//--------------------------------------------------------------Animate UV's---------------------------------------------------
		if(U && V)
		{
			matL.mainTextureOffset = new Vector2(offsetL,offsetL);
			matR.mainTextureOffset = new Vector2(offsetR,offsetR);
		}
		else if(U)
		{
			matL.mainTextureOffset = new Vector2(offsetL,0);
			matR.mainTextureOffset = new Vector2(offsetR,0);
		}
		else if(V)
		{
			matL.mainTextureOffset = new Vector2(0,offsetL);
			matR.mainTextureOffset = new Vector2(0,offsetR);
		}
	}
}