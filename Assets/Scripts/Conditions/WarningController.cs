using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using VRC2.Conditions;
using VRC2.Scenarios;
using UnityTimer;
using VRC2.Utility;
using VRC2;
using VRC2.Pipe;

public class WarningController : MonoBehaviour
{

    [Header("Visual")] public GameObject dialog;

    public TextMeshProUGUI title;

    public TextMeshProUGUI content;

    public float distance = 0.5f;

    [Header("Hazard")] public Transform centerEyeAnchor;
    public Vector3 offset = Vector3.zero;

    [Header("Noise Simulation")] public AudioSource noise;

    [Space(30)] [Header("Counter Down")] public GameObject counterDownDialog;
    public TextMeshProUGUI counterDown;

    private Timer _CounterDowntimer;

    private int totalSecond;

    private ScenariosManager scenariosManager;

    private Transform _cameraTransform
    {
        get => centerEyeAnchor;
    }

    public bool showing
    {
        get { return (dialog.activeSelf || _audioSource.isPlaying); }
    }

    private Timer _timer;

    private AudioSource _defaultSource;

    private AudioSource _updatedSource;

    private AudioSource _audioSource
    {
        get
        {
            if (_updatedSource == null) return _defaultSource;

            return _updatedSource;
        }
    }

    #region Use specific audio source for each vehicle

    public void SetAudioSource(AudioSource source)
    {
        if (source != null)
        {
            _updatedSource = source;
            Debug.LogWarning("[WarningController] AudioSource is updated.");
        }
    }

    #endregion

    #region Conditions

    private Existence existence
    {
        get => scenariosManager.condition.Existence;
    }

    private Frequency frequency
    {
        get => scenariosManager.condition.Frequency;
    }


    private TimeLimits timeLimits
    {
        get => scenariosManager.condition.TimeLimits;
    }

    private Format format
    {
        get => scenariosManager.condition.Format;
    }

    private Quality quality
    {
        get => scenariosManager.condition.Quality;
    }

    private Context context
    {
        get => scenariosManager.condition.Context;
    }

    private Amount amount
    {
        get => scenariosManager.condition.Amount;
    }


    #endregion



    // Start is called before the first frame update
    void Start()
    {
        scenariosManager = FindFirstObjectByType<ScenariosManager>();

        // var cam = Camera.main;
        // if (cam == null)
        // {
        //     cam = FindObjectOfType<Camera>();
        // }
        //
        // _cameraTransform = cam.transform;

        _defaultSource = gameObject.GetComponent<AudioSource>();
        _defaultSource.playOnAwake = false;


        dialog.SetActive(false);

        // clear it to make it invisible
        counterDownDialog.SetActive(false);
        counterDown.text = "";
    }

    void StartTimer()
    {
        if (_timer != null)
        {
            Timer.Cancel(_timer);
        }

        var interval = 5f;
        if (timeLimits == TimeLimits.Duration10Sec)
        {
            interval = 10f;
        }

        bool looped = frequency == Frequency.Repeat;

        _audioSource.loop = looped;

        _timer = Timer.Register(interval, () => { Hide(); }, isLooped: looped, useRealTime: true);
    }

    // warning delay
    private IEnumerator PlayAudio(float delay)
    {
        yield return new WaitForSeconds(delay);

        _audioSource.Play();
        if (quality == Quality.Bad)
        {
            // add noise
            noise.Play();
        }

        yield return null;
    }

    private void ProcessAudioWarning(string scenename, int incidentid, float? delay)
    {
        // load audio clip
        var ac = LoadAudioClip(scenename, incidentid);
        _audioSource.clip = ac;

        if (delay == null)
        {
            _audioSource.Play();

            if (quality == Quality.Bad)
            {
                // add noise
                noise.Play();
            }
        }
        else
        {
            // play audio with delay
            StartCoroutine(PlayAudio(delay.Value));
        }
    }

    private void ProcessVisualWarning(float? delay)
    {
        if (delay == null)
        {
            dialog.SetActive(true);
        }
        else
        {
            StartCoroutine(ShowVisual(delay.Value));
        }
    }

    // show visual with delay
    private IEnumerator ShowVisual(float delay)
    {
        yield return new WaitForSeconds(delay);

        dialog.SetActive(true);

        yield return null;
    }

    public void Show(string title, string scenename, int incidentid, string content, float? delay)
    {
        // directly return if no warning
        if (existence == Existence.NoWarning) return;

        this.title.text = title;
        this.content.text = content;

        if (format == Format.Audio)
        {
            ProcessAudioWarning(scenename, incidentid, delay);
        }
        else if (format == Format.Visual)
        {
            ProcessVisualWarning(delay);
        }
        else if (format == Format.Both)
        {
            ProcessVisualWarning(delay);
            ProcessAudioWarning(scenename, incidentid, delay);
        }

        StartTimer();
    }

    public void ShowVisualOnly(string title, string content)
    {
        if (format == Format.Audio)
        {
            // hide
            dialog.SetActive(false);
        }
        else
        {
            this.title.text = title;
            this.content.text = content;
            // show without delay
            ProcessVisualWarning(null);
            StartTimer();
        }
    }

    public void Hide(bool force = false)
    {
        if (force || frequency == Frequency.OneTime)
        {
            if (format == Format.Audio)
            {
                _audioSource.Stop();
                if (quality == Quality.Bad)
                {
                    // add noise
                    noise.Stop();
                }
            }
            else if (format == Format.Visual)
            {
                dialog.SetActive(false);
            }
            else if (format == Format.Both)
            {
                _audioSource.Stop();
                if (quality == Quality.Bad)
                {
                    // add noise
                    noise.Stop();
                }

                dialog.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dialog.activeSelf)
        {
            MoveDialogFaceHeadset(dialog);
        }

        if (counterDownDialog.activeSelf)
        {
            MoveDialogFaceHeadset(counterDownDialog);
        }
    }

    void MoveDialogFaceHeadset(GameObject obj)
    {
        var forward = _cameraTransform.forward;

        var rot = _cameraTransform.rotation;
        var pos = _cameraTransform.position + forward * distance;

        pos += offset;

        obj.transform.rotation = rot;
        obj.transform.position = pos;
    }

    public AudioClip LoadAudioClip(string scenario, int incident)
    {
        var name = Helper.GetAudioFileName(scenario, incident, context == Context.Irrelevant,
            amount == Amount.Overload);
        var path = $"{GlobalConstants.warningAudioPath}{name}";

        print(path);

        AudioClip ac = AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip)) as AudioClip;

        return ac;
    }

    #region Adaptation for new design

    public void PlayAudioClip(string filename, float? delay)
    {
        var path = $"{GlobalConstants.warningAudioPath}{filename}";
        // load audio clip
        AudioClip ac = AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip)) as AudioClip;
        _audioSource.clip = ac;

        if (delay == null)
        {
            _audioSource.Play();

            if (quality == Quality.Bad)
            {
                // add noise
                noise.Play();
            }
        }
        else
        {
            // play audio with delay
            StartCoroutine(PlayAudio(delay.Value));
        }
    }

    #endregion

    #region Counter Down

    public void StartCounterDown(int total_second)
    {
        totalSecond = total_second;

        if (_CounterDowntimer != null)
        {
            Timer.Cancel(_CounterDowntimer);
        }

        counterDownDialog.SetActive(true);

        _CounterDowntimer = Timer.Register(1, () => { SetCounterDown(); }, isLooped: true, useRealTime: true);
    }

    private void SetCounterDown()
    {
        if (totalSecond <= 0)
        {
            Timer.Cancel(_CounterDowntimer);
            _CounterDowntimer = null;
        }

        var m = totalSecond / 60;
        var s = totalSecond % 60;
        counterDown.text = $"{m} : {s}";
        totalSecond -= 1;
    }

    public void StopCounterDown()
    {
        counterDownDialog.SetActive(false);
        counterDown.text = "";

        if (_CounterDowntimer != null)
        {
            Timer.Cancel(_CounterDowntimer);
        }
    }

    #endregion
}