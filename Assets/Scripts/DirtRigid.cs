using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtRigid : MonoBehaviour
{
    public GameObject truck;
    public GameObject back;
    private bool done;
    // Start is called before the first frame update
    void Start()
    {
        done = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (done)
        {
            //if(transform.position.y > )
        }
        
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "bottom")
        {
            print("XXXX");
            Destroy(GetComponent<Rigidbody>());
            transform.parent = truck.transform;
            GetComponent<Collider>().enabled = false;
            done = true;

        }
    }

    bool close(Vector3 des)
    {
        var t = transform.position;
        // use the same y
        des.x = 0;
        t.x = 0;
        des.z = 0;
        t.z = 0;
        var distance = Vector3.Distance(t, des);

        print(distance);
        if (distance < 0.3f)
        {
            done = true;
            print("raaa");
            //Destroy(GetComponent<Rigidbody>());
            //transform.parent = truck.transform;
            Invoke("stop", 1);
            return true;
        }
        return false;
    }

    void stop()
    {
        Destroy(GetComponent<Rigidbody>());
        transform.parent = truck.transform;
    }

}
