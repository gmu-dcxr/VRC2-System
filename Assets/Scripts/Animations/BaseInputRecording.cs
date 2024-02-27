using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using System.IO;
using Fusion;
using UnityEngine.Serialization;

public class BaseInputRecording : NetworkBehaviour
{

	#region Buttons

	[Header("Recording Control")] public Button recordButton;
	public Button replayButton;
	public Button saveButton;
	public Button loadButton;

	#endregion

	private string folder = "InputRecordings";
	private InputEventTrace m_Trace;

	string GetFilePath() =>
		Path.Combine(Path.GetDirectoryName(Application.dataPath), folder, GetFilename() + ".inputtrace");

	string ClsName
	{
		get => GetType().Name;
	}

	public virtual string GetFilename()
	{
		return ClsName;
	}

	#region Need implementation in children classes

	public virtual void InitInputActions()
	{

	}

	public virtual void DisposeInputActions()
	{
		//m_PlayerInputActions.Dispose();
	}

	public virtual void UpdateLogic()
	{
		// var input = m_MoveInput.ReadValue<Vector2>();
		// var movement = new Vector3(input.x,0f,input.y) * 4f * Time.deltaTime;
		// m_Player.Translate(movement,Space.Self);
	}

	#endregion

	void Awake()
	{
		InitInputActions();

		m_Trace = new InputEventTrace(Keyboard.current);
		m_Trace.onEvent += OnEvent;

		recordButton?.onClick.AddListener(ToggleRecording);
		replayButton?.onClick.AddListener(Replay);
		saveButton?.onClick.AddListener(Save);
		loadButton?.onClick.AddListener(Load);
	}

	void OnDestroy()
	{
		DisposeInputActions();
		m_Trace.Dispose();
	}

	void Update()
	{
		UpdateLogic();
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

		replayButton.interactable = !m_Trace.enabled;

		Debug.Log(m_Trace.enabled ? "Start Recording" : "Stop Recording");
	}

	void Replay()
	{

		recordButton.interactable = false;

		m_Trace.Replay()
			.OnFinished(() => recordButton.interactable = true)
			.PlayAllEventsAccordingToTimestamps();

		Debug.Log("Replay");
	}

	void OnEvent(InputEventPtr ev)
	{
		Debug.Log(ev.ToString());
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
}