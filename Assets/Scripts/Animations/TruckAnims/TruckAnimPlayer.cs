using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckAnimPlayer : MonoBehaviour
{
    public GameObject NORMAL;
    public Animator anim;
    public GameObject truckCheck;

    public AudioSource source;


    public GameObject FirstBackup;
    public GameObject SecondBackup;
    public GameObject ThirdBackup;

    public GameObject backUpWarning;
    public GameObject waterWarning;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!(FirstBackup.active) && anim.GetBool("FirstBackup"))
        {
            anim.SetBool("FirstBackup", false);
            anim.SetBool("FirstForward", true);
            truckCheck.SetActive(false);
        }
        if (!(SecondBackup.active) && anim.GetBool("SecondBackup"))
        {
            anim.SetBool("SecondBackup", false);
            anim.SetBool("SecondForward", true);
            truckCheck.SetActive(false);
        }
        if (!(ThirdBackup.active) && anim.GetBool("ThirdBackup"))
        {
            anim.SetBool("ThirdBackup", false);
            anim.SetBool("ThirdForward", true);
            truckCheck.SetActive(false);
        }
    }

    public void Backup1()
    {
        anim.SetBool("FirstBackup", true);
    }
    public void Backup2()
    {
        anim.SetBool("FirstForward", false);
        anim.SetBool("SecondBackup", true);
    }
    public void Backup3()
    {
        anim.SetBool("FirstForward", false);
        anim.SetBool("SecondForward", false);
        anim.SetBool("ThirdBackup", true);
    }

    public void Forward()
    {
        anim.SetBool("FirstBackup", false);
        anim.SetBool("SecondBackup", false);
        anim.SetBool("ThirdBackup", false);

        anim.SetBool("Forward", true);
    }

    public void checkT()
    {
        if(!(anim.GetBool("FirstForward") || anim.GetBool("SecondForward") || anim.GetBool("ThirdForward")))
        {
            truckCheck.SetActive(true);
        }
    }

    public void playSound()
    {
        if (!(anim.GetBool("FirstForward") || anim.GetBool("SecondForward") || anim.GetBool("ThirdForward")))
        {
            print("start");
            source.Play();
        }
    }

    public void stopSound()
    {
        
        if (!(anim.GetBool("FirstForward") || anim.GetBool("SecondForward") || anim.GetBool("ThirdForward")))
        {
            //print("stop");
            source.Stop();

        }
    }

    public void playBackUpWarning()
    {
        if (!NORMAL.active)
        {
            if (!(anim.GetBool("FirstForward") || anim.GetBool("SecondForward") || anim.GetBool("ThirdForward")))
            {
                print("Warning: A truck is backing up nearby.");
                backUpWarning.GetComponent<AudioSource>().Play();
            }
        }
    }

    public void playWaterWarning()
    {
        if (!NORMAL.active)
        {
            if ((anim.GetBool("FirstForward") || anim.GetBool("SecondForward") || anim.GetBool("ThirdForward")))
            {
                print("Electrical struck warning: please be aware of the water stain when crossing the road.");
                waterWarning.GetComponent<AudioSource>().Play();
            }
        }
    }
}
