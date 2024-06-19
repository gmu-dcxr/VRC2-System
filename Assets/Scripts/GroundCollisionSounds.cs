using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollisionSounds : MonoBehaviour
{
    public AudioSource collisionNoise;

    public string collisionTag;

    void Start()
    {
        collisionNoise = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == collisionTag) 
        {
            collisionNoise.Play();
        }
    }
}
