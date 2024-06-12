using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollisionSounds : MonoBehaviour
{
    public AudioSource collisionNoise;

    void Start()
    {
        collisionNoise = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground") 
        {
            collisionNoise.Play();
        }
    }
}
