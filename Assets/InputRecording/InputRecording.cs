using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using System.IO;

public class InputRecording : MonoBehaviour {

	[SerializeField]
	Transform m_Player;

	[SerializeField]
	Button m_RecordButton;

	[SerializeField]
	Button m_ReplayButton;

	[SerializeField]
	Button m_SaveButton;

	[SerializeField]
	Button m_LoadButton;

	[SerializeField]
	string m_FileName = "recording";

	InputEventTrace m_Trace;
	PlayerInputActions m_PlayerInputActions;
	InputAction m_MoveInput;

	private Vector3 startPosition;

	void Awake ()
	{

		startPosition = m_Player.transform.position;
		
		m_PlayerInputActions = new PlayerInputActions();
		m_PlayerInputActions.Enable();
		m_MoveInput = m_PlayerInputActions.Player.Move;

		m_Trace = new InputEventTrace(Keyboard.current);
		m_Trace.onEvent += OnEvent;

		m_RecordButton.onClick.AddListener(ToggleRecording);
		m_ReplayButton.onClick.AddListener(Replay);
		m_SaveButton.onClick.AddListener(Save);
		m_LoadButton.onClick.AddListener(Load);
	}

	void OnDestroy () {
		m_PlayerInputActions.Dispose();
		m_Trace.Dispose();
	}

	void Update () {
		var input = m_MoveInput.ReadValue<Vector2>();
		var movement = new Vector3(input.x,0f,input.y) * 4f * Time.deltaTime;
		m_Player.Translate(movement,Space.Self);
	}

	void ToggleRecording () {
		if (m_Trace.enabled) {
			m_Trace.Disable();
		} else {
			m_Trace.Clear();
			m_Trace.Enable();
		}

		m_Player.position = startPosition;
		m_ReplayButton.interactable = !m_Trace.enabled;

		Debug.Log(m_Trace.enabled ? "Start Recording" : "Stop Recording");
	}

	void Replay () {
		m_Player.position = startPosition;
		m_RecordButton.interactable = false;

		m_Trace.Replay()
			.OnFinished(() => m_RecordButton.interactable = true)
			.PlayAllEventsAccordingToTimestamps();

		Debug.Log("Replay");
	}

	void OnEvent (InputEventPtr ev) {
		Debug.Log(ev.ToString());
	}

	void Save () {
		string filePath = GetFilePath();
		string directoryPath = Path.GetDirectoryName(filePath);
		if (!Directory.Exists(directoryPath)) {
			Directory.CreateDirectory(directoryPath);
		}
		
		m_Trace.WriteTo(filePath);
		Debug.Log("Save to " + filePath);
	}

	void Load () {
		string filePath = GetFilePath();
		if (!File.Exists(filePath)) {
			throw new FileNotFoundException();
		}

		m_Trace.ReadFrom(filePath);
		Debug.Log("Load from " + filePath);
	}

	string GetFilePath () => Path.Combine(Path.GetDirectoryName(Application.dataPath),"InputRecordings",m_FileName + ".txt");

}