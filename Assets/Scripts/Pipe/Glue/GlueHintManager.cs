using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC2.Pipe;

public class GlueHintManager : MonoBehaviour
{
    public GameObject hint;

    public GameObject pipe;

    // Start is called before the first frame update
    void Start()
    {
        MoveHint();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void MoveHint()
    {
        // get hint width (x)
        var width = PipeHelper.GetExtendsX(hint);
        // to the pipe's right end
        var x = GetPipeRightMostX(pipe);
        var pos = pipe.transform.position;
        pos.x = x - width;
        var rot = pipe.transform.rotation;
        hint.transform.position = pos;
        hint.transform.rotation = rot;
    }

    float GetPipeRightMostX(GameObject pipe)
    {
        var mesh = pipe.GetComponent<MeshFilter>().mesh;

        var vertices = mesh.vertices;

        var maxx = vertices[0].x;

        foreach (var v in vertices)
        {
            if (v.x > maxx) maxx = v.x;
        }

        var p = Vector3.zero;
        p.x = maxx;

        var t = pipe.transform;

        return t.TransformPoint(p).x;
    }
}
