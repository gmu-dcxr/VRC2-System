using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PipeManipulation))]
public class PipeLabelController : MonoBehaviour
{
    [SerializeField] private GameObject label;
    public bool showWhenHover { set; get; }
    private TextMeshPro _textMeshPro;

    private PipeManipulation _pipeManipulation;


    // Start is called before the first frame update
    void Start()
    {
        _textMeshPro = label.GetComponent<TextMeshPro>();
        _pipeManipulation = gameObject.GetComponent<PipeManipulation>();
        UpdateLabel();
        // hide at the beginning
        Show(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Show(bool flag)
    {
        if (showWhenHover)
        {
            label.SetActive(flag);
        }
        else
        {
            label.SetActive(false);
        }
    }

    void UpdateLabel()
    {
        var scale = _pipeManipulation.pipeSize;
        // update text
        _textMeshPro.text = $"Size: {scale}";
    }
}
