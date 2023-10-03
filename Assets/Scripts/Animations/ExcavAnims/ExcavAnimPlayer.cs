using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcavAnimPlayer : MonoBehaviour
{
    private Animator anim;
    public Transform digPoint1;
    public Transform digPoint2;
    public Transform digPoint3;
    private Transform destination;

    public Transform spawn;
    public Transform endPiece;
    private bool awake;
    public GameObject dirt;
    public GameObject hole;

    public float NumberOfDigs;

    private Vector3 scaleChange = new Vector3(0.03f, 0.0f, 0.03f);
    private Vector3 initScale = new Vector3(0.05f, 0.05f, 0.05f);

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
        dirt.SetActive(false);
        awake = false;
        destination = digPoint1;
        anim = GetComponent<Animator>();
        //anim.SetBool("Dig", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!awake)
        {
            //play wakeup animation
            anim.SetBool("Wakeup", true);
            if ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f) && anim.GetCurrentAnimatorStateInfo(0).IsName("Wakeup"))
            {
                print("STOP");
                //wakeup animation has played
                anim.SetBool("Wakeup", false);
                awake = true;
            }
        }
        //if(pt == part.nextTo)
        //{
        if (ReachDestination(destination.position))
            {
                anim.SetBool("Forward", false);
                //anim.enabled = false;
                anim.SetBool("Dig", true);
            //anim.enabled = true;
            if ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime > NumberOfDigs) && anim.GetCurrentAnimatorStateInfo(0).IsName("DIG"))
            {
                print("STOP33");
                anim.SetBool("Dig", false);

                anim.enabled = false;
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
            pt = part.nextTo;
            destination = digPoint1;
            anim.SetBool("Forward", true);
    }

    public void start_3()
    {
        //go forward til its at location 3
        //then dig
        //then dump
        anim.enabled = true;
        anim.SetBool("Dig",false);
        pt = part.into1;
        destination = digPoint2;
        anim.SetBool("Forward", true);
    }

    public void start_4()
    {
        //go forward til its at location 4
        //then dig
        //then dump
        anim.enabled = true;
        anim.SetBool("Dig", false);
        pt = part.into2;
        destination = digPoint3;
        anim.SetBool("Forward", true);
    }

    public void makeDirt()
    {
        //dirtSpawned = false;
        //dirt.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y, spawn.transform.position.z);
        //dirt.transform.rotation = Quaternion.Euler(spawn.transform.rotation.x, spawn.transform.rotation.y, spawn.transform.rotation.z);

        
        dirt.transform.SetParent(endPiece);
        dirt.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y,
            spawn.transform.position.z);
        dirt.transform.rotation = Quaternion.Euler(spawn.transform.rotation.x, spawn.transform.rotation.y,
            spawn.transform.rotation.z);
        dirt.GetComponent<Rigidbody>().useGravity = false;
        dirt.GetComponent<Rigidbody>().isKinematic = false;
        dirt.GetComponent<MeshCollider>().convex = false;
        dirt.GetComponent<MeshCollider>().convex = false;
        //make hole bigger
        //if (!moved)
        //{
        UpdateHole(hole);
            //moved = true;
        //}
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
        GameObject dupe = Instantiate(dirt);
        //dirtSpawned = true;

        dupe.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y, spawn.transform.position.z);
        dupe.transform.SetParent(endPiece);
        dupe.transform.rotation = Quaternion.Euler(spawn.transform.rotation.x + (Random.Range(0.0f, 40.0f)), spawn.transform.rotation.y + (Random.Range(0.0f, 40.0f)), spawn.transform.rotation.z + (Random.Range(0.0f, 40.0f)));
        dupe.GetComponent<Rigidbody>().useGravity = false;
        dupe.GetComponent<Rigidbody>().isKinematic = false;
        dupe.GetComponent<MeshCollider>().convex = false;
        dupe.transform.localScale = initScale;


        dupe.transform.SetParent(null);
        dupe.GetComponent<Rigidbody>().useGravity = true;
        dupe.GetComponent<MeshCollider>().convex = true;
        dirt.SetActive(false);

        dirt.transform.SetParent(null);
        dirt.GetComponent<Rigidbody>().useGravity = true;
        dirt.GetComponent<MeshCollider>().convex = true;
    }

}
