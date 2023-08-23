using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using System.IO;

public class InputRecording : MonoBehaviour
{

	[SerializeField] Transform m_Player;

	[SerializeField] private ExcavatorScript excavatorScript;

	[SerializeField] Button m_RecordButton;

	[SerializeField] Button m_ReplayButton;

	[SerializeField] Button m_SaveButton;

	[SerializeField] Button m_LoadButton;

	[SerializeField] string m_FileName = "recording";

	InputEventTrace m_Trace;
	PlayerInputActions m_PlayerInputActions;
	InputAction m_MoveInput;
	// private InputAction m_BigArmInput;

	private ExcavatorInputActions excavatorIA;

	private InputAction switchInput;
	private InputAction bigArmInput;
	private InputAction doorInput;
	private InputAction rotateInput;
	private InputAction shovelInput;
	private InputAction smallArmInput;
	private InputAction moveInput;

	private Vector3 startPosition;

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


	void Awake()
	{

		startPosition = m_Player.transform.position;

		m_PlayerInputActions = new PlayerInputActions();
		m_PlayerInputActions.Enable();
		m_MoveInput = m_PlayerInputActions.Player.Move;
		// m_BigArmInput = m_PlayerInputActions.Player.BigArm;

		excavatorIA = new ExcavatorInputActions();
		excavatorIA.Enable();

		switchInput = excavatorIA.Excavator.Switch;
		bigArmInput = excavatorIA.Excavator.BigArm;
		doorInput = excavatorIA.Excavator.Door;
		rotateInput = excavatorIA.Excavator.Rotate;
		shovelInput = excavatorIA.Excavator.Shovel;
		smallArmInput = excavatorIA.Excavator.SmallArm;
		moveInput = excavatorIA.Excavator.Move;

		m_Trace = new InputEventTrace(Keyboard.current);
		m_Trace.onEvent += OnEvent;

		m_RecordButton.onClick.AddListener(ToggleRecording);
		m_ReplayButton.onClick.AddListener(Replay);
		m_SaveButton.onClick.AddListener(Save);
		m_LoadButton.onClick.AddListener(Load);
	}

	void OnDestroy()
	{
		m_PlayerInputActions.Dispose();
		excavatorIA.Dispose();
		m_Trace.Dispose();
	}

	void Update()
	{
		// var input = m_MoveInput.ReadValue<Vector2>();
		// var movement = new Vector3(input.x,0f,input.y) * 4f * Time.deltaTime;
		// m_Player.Translate(movement,Space.Self);
		// print(m_BigArmInput.ReadValue<Vector2>());
		if (!InDriveMode)
		{
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

		}

		//---------------------------------------------------------DRIVE MODE--------------------------------------------------------------
		if (InDriveMode)
		{
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
		}

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

	void ToggleRecording()
	{
		if (m_Trace.enabled)
		{
			m_Trace.Disable();
		}
		else
		{
			m_Trace.Clear();
			m_Trace.Enable();
		}

		m_Player.position = startPosition;
		m_ReplayButton.interactable = !m_Trace.enabled;

		Debug.Log(m_Trace.enabled ? "Start Recording" : "Stop Recording");
	}

	void Replay()
	{
		m_Player.position = startPosition;
		m_RecordButton.interactable = false;

		m_Trace.Replay()
			.OnFinished(() => m_RecordButton.interactable = true)
			.PlayAllEventsAccordingToTimestamps();

		Debug.Log("Replay");
	}

	void OnEvent(InputEventPtr ev)
	{
		
	}

	void Save()
	{
		string filePath = GetFilePath();
		string directoryPath = Path.GetDirectoryName(filePath);
		if (!Directory.Exists(directoryPath))
		{
			Directory.CreateDirectory(directoryPath);
		}

		m_Trace.WriteTo(filePath);
		Debug.Log("Save to " + filePath);
	}

	void Load()
	{
		string filePath = GetFilePath();
		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException();
		}

		m_Trace.ReadFrom(filePath);
		Debug.Log("Load from " + filePath);
	}

	string GetFilePath() =>
		Path.Combine(Path.GetDirectoryName(Application.dataPath), "InputRecordings", m_FileName + ".txt");

}