#if UNITY_WSA
using UnityEngine;
public class TransformAttachment : MonoBehaviour
{
    
}
#else
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class TransformAttachment : NetworkBehaviour
{
    [Header("Transform")] public Transform transform;
    [Header("Source Object")] public GameObject source;

    [Header("Setting")] public bool enablePosition = true;

    public bool enableRotation = true;

    public bool enableScale = true;
    
    public override void FixedUpdateNetwork()
    {
        if (source == null || transform == null) return;
        var t = source.transform;
        if (enablePosition) transform.position = t.position;
        if (enableRotation) transform.rotation = t.rotation;
        if (enableScale) transform.localScale = t.localScale;
    }
}
#endif
