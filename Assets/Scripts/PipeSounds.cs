using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSounds : MonoBehaviour
{
    AudioSource pipeSound;
    // Start is called before the first frame update
    void Start()
    {
        pipeSound = GetComponent<AudioSource> ();
    }
    void OnCollisionEnter() 
    {
        pipeSound.PlayOneShot(pipeSound.clip, 0.7f);
    }
}
