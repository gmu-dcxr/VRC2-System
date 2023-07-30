using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VRC2.Conditions;
using VRC2.Scenarios;
using UnityTimer;

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

    private Transform _cameraTransform;

    private bool showing = false;

    private Timer _timer;



    // Start is called before the first frame update
    void Start()
    {
        scenariosManager = FindFirstObjectByType<ScenariosManager>();
        _cameraTransform = Camera.main.transform;
        Hide(true);
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

        _timer = Timer.Register(interval, () => { Hide(); }, isLooped: looped, useRealTime: true);

    }

    public void Show(string title, string content)
    {
        this.title.text = title;
        this.content.text = content;

        dialog.SetActive(true);

        StartTimer();
    }

    public void Hide(bool force = false)
    {
        if (force || frequency == Frequency.OneTime)
        {
            dialog.SetActive(false);
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
}