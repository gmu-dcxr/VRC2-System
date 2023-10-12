using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckAnimPlayer : MonoBehaviour
{
    public Animator anim;


    public GameObject FirstBackup;
    public GameObject SecondBackup;
    public GameObject ThirdBackup;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!(FirstBackup.active) && anim.GetBool("FirstBackup"))
        {
            anim.SetBool("FirstBackup", false);
            anim.SetBool("FirstForward", true);
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
}
