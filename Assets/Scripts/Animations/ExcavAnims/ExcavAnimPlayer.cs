using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CONTROLS BASELINE S3 - TRUCK BACKING UP CLOSE TO USER ACCIDENT

public class ExcavAnimPlayer : MonoBehaviour
{
    private Animator anim;
    public Transform digPoint1;
    public Transform digPoint2;
    public Transform digPoint3;
    private Transform destination;
    private bool ready;

    //dirt spawning/spawning
    public Transform spawn;
    public Transform endPiece;
    private bool awake;
    public GameObject dirt;
    public GameObject hole;

    public GameObject lid;
    private GameObject scoop;

    public GameObject spill;
    public GameObject truck;

    //treads
    private float offsetL;
    private float offsetR;
    public Material matL;
    public Material matR;
    public GameObject TreadsL;
    public GameObject TreadsR;
    public GameObject leftTread;
    public GameObject rightTread;

    public GameObject WheelFrontLeft;
    public GameObject WheelFrontRight;
    private float rotSpeed = 30f;
    public GameObject WheelBackLeft;
    public GameObject WheelBackRight;

    public float NumberOfDigs;
    private bool done;

    public GameObject FirstBackup;
    public GameObject SecondBackup;
    public GameObject ThirdBackup;

    public GameObject truckCheck;

    private Vector3 scaleChange = new Vector3(0.03f, 0.0f, 0.03f);
    private Vector3 initScale = new Vector3(0.99f, 0.99f, 0.99f);
    //private Vector3 initScale = new Vector3(0.2f, 0.2f, 0.2f);

    // private GameObject[] clonesArray;
    int curIndex = 0;

    internal enum part
    {
        nextTo = 0,
        into1 = 1,
        into2 = 2,

    }

    private part pt;


    // Start is called before the first frame update
    void Start()
    {
        //scoop = GameObject.FindGameObjectWithTag("Scoop");
        //Physics.IgnoreCollision(GetComponent<Collider>(), dirt.GetComponent<Collider>());
        dirt.SetActive(false);
        awake = false;
        destination = digPoint1;
        anim = GetComponent<Animator>();
        matL = TreadsL.GetComponent<Renderer>().material;
        matR = TreadsR.GetComponent<Renderer>().material;
        done = false;
        // clonesArray = new GameObject[50];
        anim.Rebind();

        ready = true;
        //anim.SetBool("Dig", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!awake)
        {
            //play wakeup animation
            anim.SetBool("Wakeup", true);

            if ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f) &&
                anim.GetCurrentAnimatorStateInfo(0).IsName("Wakeup"))
            {
                //print("STOP");
                //wakeup animation has played
                anim.SetBool("Wakeup", false);
                awake = true;
            }

        }

        if (ready && truckCheck.active)
        {
            anim.SetBool("Dig", false);
            anim.SetBool("Forward", true);
            ready = false;
        }


        if (anim.GetCurrentAnimatorStateInfo(0).IsName("FORWARD"))
        {
            //animate the treads
            offsetR = Time.time * (0.75f) % 1;
            offsetL = Time.time * (0.75f) % 1;
            matL.mainTextureOffset = new Vector2(0, offsetL);
            matR.mainTextureOffset = new Vector2(0, offsetR);
            WheelFrontRight.transform.Rotate(Vector3.forward * Time.deltaTime * rotSpeed * 10);
            WheelBackRight.transform.Rotate(Vector3.forward * Time.deltaTime * rotSpeed * 10);
            WheelFrontLeft.transform.Rotate(-Vector3.forward * Time.deltaTime * rotSpeed * 10);
            WheelBackLeft.transform.Rotate(-Vector3.forward * Time.deltaTime * rotSpeed * 10);
        }

        if (ReachDestination(destination.position))
        {
            if (!done && truckCheck.active)
            {
                anim.SetBool("Forward", false);
                anim.SetBool("Dig", true);
                done = true;
            }

            if ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime > NumberOfDigs) &&
                anim.GetCurrentAnimatorStateInfo(0).IsName("DIG"))
            {
                anim.SetBool("Dig", false);
                anim.enabled = false;
                done = true;
                anim.enabled = true;

                if (pt == part.nextTo)
                {
                    FirstBackup.SetActive(false);
                }

                if (pt == part.into1)
                {

                    SecondBackup.SetActive(false);

                }

                if (pt == part.into2)
                {

                    ThirdBackup.SetActive(false);

                }
            }

        }
        //}
    }

    bool ReachDestination(Vector3 des)
    {
        var t = transform.position;
        // use the same y
        des.y = 0;
        t.y = 0;
        var distance = Vector3.Distance(t, des);

        if (distance < 1.0f)
        {
            return true;
        }

        return false;
    }

    public void start_2()
    {
        //go forward til its at location 2
        //then dig
        //then dump
        // while (curIndex > 0)
        // {
        //     Destroy(clonesArray[curIndex]);
        //     curIndex--;
        // }
        //
        // clonesArray = new GameObject[50];

        dirt.SetActive(false);
        anim.Rebind();
        //ready = true;
        done = false;
        pt = part.nextTo;
        destination = digPoint1;
        //clonesArray = new GameObject[50];
    }

    public void start_3()
    {
        //go forward til its at location 3
        //then dig
        //then dump
        ready = true;
        done = false;
        anim.enabled = true;
        pt = part.into1;
        destination = digPoint2;

        // while (curIndex > 0)
        // {
        //     Destroy(clonesArray[curIndex]);
        //     curIndex--;
        // }
        //
        // clonesArray = new GameObject[50];
    }

    public void start_4()
    {
        //go forward til its at location 4
        //then dig
        //then dump
        ready = true;
        done = false;
        anim.enabled = true;
        pt = part.into2;
        destination = digPoint3;
        // while (curIndex > 0)
        // {
        //     Destroy(clonesArray[curIndex]);
        //     curIndex--;
        // }
        //
        // clonesArray = new GameObject[50];
    }

    public void makeDirt()
    {
        UpdateHole(hole);
        dirt.SetActive(true);
    }

    void UpdateHole(GameObject h)
    {
        h.transform.localScale -= scaleChange;
        h.transform.position = new Vector3(h.transform.position.x - 0.1f, h.transform.position.y - 0.1f,
            h.transform.position.z - 0.1f);
    }

    public void dirtDupe()
    {
        var t = dirt.transform;
        GameObject dupe = Instantiate(dirt, t.transform);
        dupe.transform.localPosition = new Vector3(0, curIndex * 0.2f, -0.3f);

        // dupe.transform.SetParent(endPiece);
        // if (pt == part.into2)
        // {
        //     dupe.transform.position = new Vector3(endPiece.transform.position.x-1.7f, endPiece.transform.position.y-0.3f, endPiece.transform.position.z-0.75f);
        // }
        // if (pt == part.into1)
        // {
        //     dupe.transform.position = new Vector3(endPiece.transform.position.x - 1.71f, endPiece.transform.position.y, endPiece.transform.position.z - 0.75f );
        //     dupe.transform.rotation = Quaternion.Euler(spawn.transform.rotation.x + (Random.Range(0.0f, 40.0f)), spawn.transform.rotation.y + (Random.Range(0.0f, 40.0f)), spawn.transform.rotation.z + (Random.Range(0.0f, 40.0f)));
        // }
        // if(pt == part.nextTo)
        // {
        //     dupe.transform.position = new Vector3(endPiece.transform.position.x-1.0f, endPiece.transform.position.y, endPiece.transform.position.z-0.75f);
        //     dupe.transform.rotation = Quaternion.Euler(spawn.transform.rotation.x + (Random.Range(0.0f, 40.0f)), spawn.transform.rotation.y + (Random.Range(0.0f, 40.0f)), spawn.transform.rotation.z + (Random.Range(0.0f, 40.0f)));
        // }        
        dupe.GetComponent<Rigidbody>().isKinematic = false;
        dupe.transform.localScale = initScale;
        dupe.transform.SetParent(null);
        dupe.GetComponent<Rigidbody>().useGravity = true;
        curIndex++;
        // clonesArray[curIndex] = dupe;
        dirt.SetActive(false);
    }

    public void spillUpdate()
    {
        spill.transform.position = new Vector3(spill.transform.position.x, spill.transform.position.y + 0.030f,
            spill.transform.position.z);
    }

    public void deleteDirt()
    {
        // while (curIndex > 0)
        // {
        //     Destroy(clonesArray[curIndex]);
        //     curIndex--;
        // }
    }

}