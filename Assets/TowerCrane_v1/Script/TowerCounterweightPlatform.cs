using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCounterweightPlatform : MonoBehaviour
{
    [HideInInspector]
    public int checkPoint = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Left Counterweight") // 1
        {
            checkPoint = 1;
        }
        if (other.tag == "Right Counterweight") // 2
        {
            checkPoint = 2;
        }
        if (other.tag == "Forward Counterweight") // 3
        {
            checkPoint = 3;
        }
        if (other.tag == "Back Counterweight") // 4
        {
            checkPoint = 4;
        }
    }
}
