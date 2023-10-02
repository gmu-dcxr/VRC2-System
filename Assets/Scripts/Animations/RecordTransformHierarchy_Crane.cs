using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class RecordTransformHierarchy_Crane : MonoBehaviour
{
    public AnimationClip clip;

    private GameObjectRecorder m_Recorder;
    public GameObject craneGroup;
    public GameObject hook;


    void Start()
    {
        while (hook.transform.parent == null)
        {
            hook.transform.parent = craneGroup.transform;
        }
        // Create recorder and record the script GameObject.
       
            m_Recorder = new GameObjectRecorder(gameObject);

        // Bind all the Transforms on the GameObject and all its children.
        m_Recorder.BindComponentsOfType<Transform>(gameObject, true);
    }

    void LateUpdate()
    {
        if (clip == null)
            return;

        // Take a snapshot and record all the bindings values for this frame.
        m_Recorder.TakeSnapshot(Time.deltaTime);
    }

    void OnDisable()
    {
        // if (clip == null)
        //     return;
        //
        // if (m_Recorder.isRecording)
        // {
        //     // Save the recorded session to the clip.
        //     m_Recorder.SaveToClip(clip);
        // }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(200, 10, 100, 50), "Save"))
        {
            if (m_Recorder.isRecording)
            {
                // Save the recorded session to the clip.
                m_Recorder.SaveToClip(clip);
                print("saved");
            }
        }
    }
}






