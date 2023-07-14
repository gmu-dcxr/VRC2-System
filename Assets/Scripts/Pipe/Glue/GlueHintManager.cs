using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC2.Pipe;

public class GlueHintManager : MonoBehaviour
{
    public GameObject hint;

    private GameObject _pipe;

    [HideInInspector]
    public bool glued
    {
        get => hint.activeSelf;
    }

    // Start is called before the first frame update
    void Start()
    {
        // disable at the beginning
        HideHint();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowHintFor(GameObject p)
    {
        _pipe = p;

        MoveHintTo(p);
        hint.SetActive(true);
    }

    public void HideHint()
    {
        hint.SetActive(false);
    }

    void MoveHintTo(GameObject pipe)
    {
        // get hint width (x)
        var width = PipeHelper.GetExtendsX(hint);
        // get pipe width (x)
        var x = PipeHelper.GetExtendsX(pipe);

        var pos = Vector3.zero;
        pos.x = x - width;

        hint.transform.localPosition = pos;
        hint.transform.localRotation = Quaternion.identity;
    }
}
