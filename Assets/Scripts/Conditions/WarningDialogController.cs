using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VRC2.Conditions;
using VRC2.Scenarios;

public class WarningDialogController : MonoBehaviour
{

    public GameObject dialog;

    public TextMeshProUGUI title;

    public TextMeshProUGUI content;

    public ScenariosManager scenariosManager;

    public float distance = 1.0f;

    private Frequency frequency;
    private TimeLimits timeLimits;

    private Transform _cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        MoveDialogFaceHeadset();
    }

    void MoveDialogFaceHeadset()
    {
        var forward = _cameraTransform.forward;

        dialog.transform.rotation = _cameraTransform.rotation;
        dialog.transform.position = _cameraTransform.position + forward * distance;
    }
}
