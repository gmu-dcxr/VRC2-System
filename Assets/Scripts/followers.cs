using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class followers : MonoBehaviour
{
    // Start is called before the first frame update
   
        public PathCreator pathCreator;
        public float speed;
        float distanceTravelled;
        public float rotation;
    

    // Update is called once per frame
    void Update()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
    }
}
