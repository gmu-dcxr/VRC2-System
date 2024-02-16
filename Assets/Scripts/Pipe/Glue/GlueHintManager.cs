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
        // set once
        if(_pipe == p) return;
        
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
        var (cc, cr) = PipeHelper.GetRightMostCenter(pipe);

        var rot = pipe.transform.rotation;
        var up = pipe.transform.up;
        var forward = Vector3.Cross(cr, up);

        rot = Quaternion.LookRotation(forward, up);

        hint.transform.position = cc;
        hint.transform.rotation = rot;
    }
}
