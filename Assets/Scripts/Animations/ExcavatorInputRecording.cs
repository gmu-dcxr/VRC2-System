using UnityEngine;
using UnityEngine.InputSystem;

namespace VRC2.Animations
{
	public class ExcavatorInputRecording : BaseInputRecording
	{
		[Space(30)]
		public Transform rotation;
		#region Self fields

		public ExcavatorScript excavatorScript;

		private ExcavatorInputActions excavatorIA;

		private InputAction switchInput;
		private InputAction bigArmInput;
		private InputAction doorInput;
		private InputAction rotateInput;
		private InputAction shovelInput;
		private InputAction smallArmInput;
		private InputAction moveInput;

		private Transform excavatorBody
		{
			get => excavatorScript.gameObject.transform;
		}

		Animator anim
		{
			get => excavatorScript.anim;
		}

		private bool InDriveMode
		{
			get => excavatorScript.InDriveMode;
			set => excavatorScript.InDriveMode = value;
		}

		//Treads
		private GameObject TreadsL
		{
			get => excavatorScript.TreadsL;
		}

		private GameObject TreadsR
		{
			get => excavatorScript.TreadsR;
		}

		//Weight Points - determines the rotation and movement axis of the Excavator
		private GameObject leftTread
		{
			get => excavatorScript.leftTread;
		}

		private GameObject rightTread
		{
			get => excavatorScript.rightTread;
		}

		//Big Wheels
		private GameObject WheelFrontLeft
		{
			get => excavatorScript.WheelFrontLeft;
		}

		private GameObject WheelFrontRight
		{
			get => excavatorScript.WheelBackRight;
		}

		private GameObject WheelBackLeft
		{
			get => excavatorScript.WheelBackLeft;
		}

		private GameObject WheelBackRight
		{
			get => excavatorScript.WheelBackRight;
		}

		private float rotSpeed
		{
			get => excavatorScript.rotSpeed;
		}


		public float driveSpeed
		{
			get => excavatorScript.driveSpeed;
		}

		private bool opened
		{
			get => excavatorScript.opened;
			set => excavatorScript.opened = value;
		}

		public float scrollSpeed
		{
			get => excavatorScript.scrollSpeed;
		}

		public float offsetL
		{
			get => excavatorScript.offsetL;
			set => excavatorScript.offsetL = value;
		}

		public float offsetR
		{
			get => excavatorScript.offsetR;
			set => excavatorScript.offsetR = value;
		}

		public bool U
		{
			get => excavatorScript.U;
		}

		public bool V
		{
			get => excavatorScript.V;
		}

		public Material matL
		{
			get => excavatorScript.matL;
		}

		public Material matR
		{
			get => excavatorScript.matR;
		}

		#endregion

		public override void InitInputActions()
		{
			excavatorIA = new ExcavatorInputActions();
			excavatorIA.Enable();

			switchInput = excavatorIA.Excavator.Switch;
			bigArmInput = excavatorIA.Excavator.BigArm;
			doorInput = excavatorIA.Excavator.Door;
			rotateInput = excavatorIA.Excavator.Rotate;
			shovelInput = excavatorIA.Excavator.Shovel;
			smallArmInput = excavatorIA.Excavator.SmallArm;
			moveInput = excavatorIA.Excavator.Move;
		}

		public override void DisposeInputActions()
		{
			excavatorIA.Dispose();
		}

		public float getRotation()
        {
			return rotation.localRotation.eulerAngles.x;
		}

		public override void UpdateLogic()
		{
			//if (!InDriveMode)
			//{
				//-------------------------------------------------BIG ARM-----------------------------------------------------------------
				// if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && anim.GetInteger("BigArmPosition") != 2)
				if (bigArmInput.ReadValue<Vector2>().y > 0 && anim.GetInteger("BigArmPosition") != 2)
				{
					anim.SetInteger("BigArmPosition", 1);
					anim.SetFloat("BigArmSpeed", 1f);
				}
				// else if (!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) && anim.GetInteger("BigArmPosition") != 0)
				else if (bigArmInput.ReadValue<Vector2>().y < 0 && anim.GetInteger("BigArmPosition") != 0)
				{
					anim.SetInteger("BigArmPosition", 1);
					anim.SetFloat("BigArmSpeed", -1f);
				}
				else
				{
					anim.SetFloat("BigArmSpeed", 0);
				}

				//-------------------------------------------------------SMALL ARM-------------------------------------------------------------
				// if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && anim.GetInteger("SmallArmPosition") != 2)
				if (smallArmInput.ReadValue<Vector2>().y > 0 && anim.GetInteger("SmallArmPosition") != 2)
				{
					anim.SetInteger("SmallArmPosition", 1);
					anim.SetFloat("SmallArmSpeed", 1f);
				}
				else if (smallArmInput.ReadValue<Vector2>().y < 0 && anim.GetInteger("SmallArmPosition") != 0)
				{
					anim.SetInteger("SmallArmPosition", 1);
					anim.SetFloat("SmallArmSpeed", -1f);
				}
				else
				{
					anim.SetFloat("SmallArmSpeed", 0);
				}

				//----------------------------------------------------------SHOVEL-----------------------------------------------------------------
				if (shovelInput.ReadValue<Vector2>().y > 0 && anim.GetInteger("ShovelPosition") != 2)
				{
					anim.SetInteger("ShovelPosition", 1);
					anim.SetFloat("ShovelSpeed", 1f);
				}
				else if (shovelInput.ReadValue<Vector2>().y < 0 &&
				         anim.GetInteger("ShovelPosition") != 0)
				{
					anim.SetInteger("ShovelPosition", 1);
					anim.SetFloat("ShovelSpeed", -1f);
				}
				else
				{
					anim.SetFloat("ShovelSpeed", 0);
				}

				//---------------------------------------------------------ROTATE BODY----------------------------------------------------------
				if (rotateInput.ReadValue<Vector2>().x < 0)
				{
					anim.SetFloat("RotateSpeed", 0.5f);
				}
				else if (rotateInput.ReadValue<Vector2>().x > 0)
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
				if (moveInput.ReadValue<Vector2>().y > 0)
				{
					excavatorBody.RotateAround(leftTread.transform.position, -Vector3.up, Time.deltaTime * rotSpeed);
					offsetR = Time.time * scrollSpeed % 1;
					WheelFrontRight.transform.Rotate(Vector3.forward * Time.deltaTime * rotSpeed * 4);
					WheelBackRight.transform.Rotate(Vector3.forward * Time.deltaTime * rotSpeed * 4);

				}

				if (moveInput.ReadValue<Vector2>().y < 0)
				{
					excavatorBody.RotateAround(leftTread.transform.position, Vector3.up, Time.deltaTime * rotSpeed);
					offsetR = Time.time * -scrollSpeed % 1;
					WheelFrontRight.transform.Rotate(-Vector3.forward * Time.deltaTime * rotSpeed * 4);
					WheelBackRight.transform.Rotate(-Vector3.forward * Time.deltaTime * rotSpeed * 4);
				}

				//ANIMATE LEFT TREAD
				if (moveInput.ReadValue<Vector2>().x < 0)
				{
					excavatorBody.RotateAround(rightTread.transform.position, Vector3.up, Time.deltaTime * rotSpeed);
					offsetL = Time.time * scrollSpeed % 1;
					WheelFrontLeft.transform.Rotate(-Vector3.forward * Time.deltaTime * rotSpeed * 4);
					WheelBackLeft.transform.Rotate(-Vector3.forward * Time.deltaTime * rotSpeed * 4);
				}

				if (moveInput.ReadValue<Vector2>().x > 0)
				{
					excavatorBody.RotateAround(rightTread.transform.position, -Vector3.up, Time.deltaTime * rotSpeed);
					offsetL = Time.time * -scrollSpeed % 1;
					WheelFrontLeft.transform.Rotate(Vector3.forward * Time.deltaTime * rotSpeed * 4);
					WheelBackLeft.transform.Rotate(Vector3.forward * Time.deltaTime * rotSpeed * 4);
				}
			//}

			//------------------------------------------------------DOOR OPEN / CLOSE-----------------------------------------------------
			if (doorInput.triggered)
			{
				opened = !opened;
				anim.SetBool("DoorOpen", opened);
			}

			//-----------------------------------------------Switch Drive Mode/ Work Mode-------------------------------------------------
			if (switchInput.triggered)
			{
				InDriveMode = !InDriveMode;
			}

			//--------------------------------------------------------------Animate UV's---------------------------------------------------
			if (U && V)
			{
				matL.mainTextureOffset = new Vector2(offsetL, offsetL);
				matR.mainTextureOffset = new Vector2(offsetR, offsetR);
			}
			else if (U)
			{
				matL.mainTextureOffset = new Vector2(offsetL, 0);
				matR.mainTextureOffset = new Vector2(offsetR, 0);
			}
			else if (V)
			{
				matL.mainTextureOffset = new Vector2(0, offsetL);
				matR.mainTextureOffset = new Vector2(0, offsetR);
			}
		}
	}
}