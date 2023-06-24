using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PipeLabelController : MonoBehaviour
{
    [SerializeField] private GameObject labelGameObject;

    public bool showWhenHover
    {
        set;
        get;
    }
    
    private TextMeshPro _textMeshPro;

    private string label;
    
    
    // Start is called before the first frame update
    void Start()

    {

        _textMeshPro = labelGameObject.GetComponent<TextMeshPro>();
        label = GetLabel();
        
        // update text
        _textMeshPro.text = label;
        
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
            labelGameObject.SetActive(flag);   
        }
        else
        {
            labelGameObject.SetActive(false);
        }
    }

    int GetSize()
    {
        // TODO: customize size mapping
        var x = labelGameObject.transform.localScale.x;
        return (int)x;
    }

    string GetLabel()
    {
        var s = GetSize();
        return $"Size: {s}";
    }
}
