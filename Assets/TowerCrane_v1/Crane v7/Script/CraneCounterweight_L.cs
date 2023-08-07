using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CraneCounterweight_L : MonoBehaviour
{
    private CraneCarController_L mScriptCar;
    private CraneController_L mScriptCrane;
    private UIPanelCrane_L mScriptUI;
    public KeyCode animationCounterweight_Keu;
    private bool animationCounterweight_Bool = true;
    public Transform counterweight;
    public Transform counterweightPiston;
    public Animator anim;
    [HideInInspector]
    public bool blocedKey = true;

    void Start()
    {
        mScriptCar = transform.GetComponent<CraneCarController_L>();
        mScriptUI = transform.GetComponent<UIPanelCrane_L>();
        mScriptCrane = transform.GetComponent<CraneController_L>();
    }
    void Update()
    {
            if(mScriptUI._toggleMenuCrane == 1 && mScriptCar.startCrane_Bool == false && mScriptCrane.blockingAnimation == true)
            {
        if (Input.GetKeyDown(animationCounterweight_Keu) && animationCounterweight_Bool == true && blocedKey == true)
        {
            anim.Play("Animation Crane(A)_L");
            animationCounterweight_Bool = false;
        }
        else if (Input.GetKeyDown(animationCounterweight_Keu) && animationCounterweight_Bool == false && blocedKey == true)
        {
            anim.Play("Animation Crane(B)_L");
            animationCounterweight_Bool = true;
        }
       }
            if(blocedKey == false)
        {
            mScriptCrane.SoundPitchCraneCabin();
        }
    }
    IEnumerator SpeedAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.7f);
            anim.speed = 1f;
        }
    }
   public void EventAnim_Connect()
    {
        anim.speed = 0f;
        StartCoroutine("SpeedAnimation");
        counterweight.SetParent(counterweightPiston);
    }
    public void EventAnim_Disconnect()
    {
        anim.speed = 0f;
        StartCoroutine("SpeedAnimation");
        counterweight.SetParent(transform);
    }
    public void BlocedOn()
    {
        blocedKey = false;
    }
    public void BlocedOff()
    {
        blocedKey = true;
        if(counterweight.parent == counterweightPiston)
        {
            mScriptUI.textConectedCargo.text = "ON";
            mScriptUI.icon_N2.color = new Color32(88, 255, 95, 255);
        }
        if (counterweight.parent == transform)
        {
            mScriptUI.textConectedCargo.text = "OFF";
            mScriptUI.icon_N2.color = new Color32(246, 246, 246, 255);
        }
    }
}
