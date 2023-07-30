using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using VRC2.Conditions;
using VRC2.Scenarios;
using UnityTimer;
using VRC2;
using VRC2.Pipe;

public class WarningController : MonoBehaviour
{

    public GameObject dialog;

    public TextMeshProUGUI title;

    public TextMeshProUGUI content;

    public float distance = 0.5f;

    private ScenariosManager scenariosManager;

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

    private Transform _cameraTransform;

    private bool showing = false;

    private Timer _timer;

    private AudioSource _audioSource;



    // Start is called before the first frame update
    void Start()
    {
        scenariosManager = FindFirstObjectByType<ScenariosManager>();
        _cameraTransform = Camera.main.transform;

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

    public void Show(string title, string scenename, int incidentid, string content)
    {
        this.title.text = title;
        this.content.text = content;

        if (format == Format.Audio)
        {
            // load audio clip
            var ac = LoadAudioClip(scenename, incidentid);
            _audioSource.clip = ac;
            _audioSource.Play();
        }
        else
        {
            dialog.SetActive(true);
        }

        StartTimer();
    }

    public void Hide(bool force = false)
    {
        if (force || frequency == Frequency.OneTime)
        {
            if (format == Format.Audio)
            {
                _audioSource.Stop();
            }
            else
            {
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

        dialog.transform.rotation = _cameraTransform.rotation;
        dialog.transform.position = _cameraTransform.position + forward * distance;
    }

    public AudioClip LoadAudioClip(string name, int incident)
    {
        var filename = $"{name}_{incident}.wav";
        var path = $"{GlobalConstants.warningAudioPath}{filename}";
        
        print(path);

        AudioClip ac = AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip)) as AudioClip;

        return ac;
    }
}