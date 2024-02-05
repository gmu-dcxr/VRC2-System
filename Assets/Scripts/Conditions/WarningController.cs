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

    private AudioSource _audioSource;

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

        _audioSource = gameObject.GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;


        dialog.SetActive(false);
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

    private void ProcessVisualWarning()
    {
        dialog.SetActive(true);
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
            ProcessVisualWarning();
        }
        else if (format == Format.Both)
        {
            ProcessVisualWarning();
            ProcessAudioWarning(scenename, incidentid, delay);
        }

        StartTimer();
    }

    public void ShowVisualOnly()
    {
        ProcessVisualWarning();
        StartTimer();
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
            MoveDialogFaceHeadset();
        }
    }

    void MoveDialogFaceHeadset()
    {
        var forward = _cameraTransform.forward;

        var rot = _cameraTransform.rotation;
        var pos = _cameraTransform.position + forward * distance;

        pos += offset;

        dialog.transform.rotation = rot;
        dialog.transform.position = pos;
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
}