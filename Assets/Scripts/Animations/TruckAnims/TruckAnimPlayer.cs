using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckAnimPlayer : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Backup()
    {
        anim.SetBool("Back", true);
    }

    public void Forward()
    {
        anim.SetBool("Back",false);
        anim.SetBool("Forward", true);
    }
}
