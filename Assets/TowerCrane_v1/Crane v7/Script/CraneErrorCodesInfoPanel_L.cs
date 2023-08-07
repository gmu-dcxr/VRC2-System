using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneErrorCodesInfoPanel_L : MonoBehaviour
{
    private UIPanelCrane_L _mScriptUI;
    private CraneController_L _mscriptC;
    [Header("Boom rotation locked Raise arrow up")]
    public string codes1;
    [Header("Boom rotation blocked Hook caught on frame")]
    public string codes2;
    [Header("Boom swing locked While secondary boom deployed")]
    public string codes3;
    [Header("Boom rotation is locked while counterweight animation is running")]
    public string codes4;
    [Header("The hook cannot be connected while the boom is turned")]
    public string codes5;
    [Header("The hook is locked! Boom extended")]
    public string codes6;
    [Header("The hook is locked! Boom raised")]
    public string codes7;
    [Header("Secondary boom locked while main boom is raised")]
    public string codes8;
    [Header("Secondary boom locked while main boom is moved")]
    public string codes9;
    [Header("The hook cannot be connected while the auxiliary boom is deployed")]
    public string codes10;
    [Header("The additional boom cannot be extended while the hook is engaged")]
    public string codes11;
    [Header("You cannot roll an additional arrow while it is bent")]
    public string codes12;
    [Header("The action will be blocked if a load is hooked to the hook")]
    public string codes13;
    [HideInInspector]
    public bool blocksKeyErroe = true;
    [Header("How long will it take for the text to disappear")]
    public float tinePanelError = 1.5f;
    void Start()
    {
        _mScriptUI = transform.GetComponent<UIPanelCrane_L>();
        _mscriptC = transform.GetComponent<CraneController_L>();
    }
    IEnumerator ErroeCodes()
    {
        blocksKeyErroe = false;
        yield return new WaitForSeconds(tinePanelError);
        _mscriptC.errorCodesPanel = 0;
        _mScriptUI.textInfoPanel.text = "";
        blocksKeyErroe = true;
    }
   
    void LateUpdate()
    {
        if(_mscriptC.errorCodesPanel == 1)
        {
            _mScriptUI.textInfoPanel.text = codes1;
        }
        if (_mscriptC.errorCodesPanel == 2)
        {
            _mScriptUI.textInfoPanel.text = codes2;
        }
        if (_mscriptC.errorCodesPanel == 3)
        {
            _mScriptUI.textInfoPanel.text = codes3;
        }
        if (_mscriptC.errorCodesPanel == 4)
        {
            _mScriptUI.textInfoPanel.text = codes4;
        }
        if (_mscriptC.errorCodesPanel == 5)
        {
            _mScriptUI.textInfoPanel.text = codes5;
        }
        if (_mscriptC.errorCodesPanel == 6)
        {
            _mScriptUI.textInfoPanel.text = codes6;
        }
        if (_mscriptC.errorCodesPanel == 7)
        {
            _mScriptUI.textInfoPanel.text = codes7;
        }
        if (_mscriptC.errorCodesPanel == 8)
        {
            _mScriptUI.textInfoPanel.text = codes8;
        }
        if (_mscriptC.errorCodesPanel == 9)
        {
            _mScriptUI.textInfoPanel.text = codes9;
        }
        if (_mscriptC.errorCodesPanel == 10)
        {
            _mScriptUI.textInfoPanel.text = codes10;
        }
        if (_mscriptC.errorCodesPanel == 11)
        {
            _mScriptUI.textInfoPanel.text = codes11;
        }
        if (_mscriptC.errorCodesPanel == 12)
        {
            _mScriptUI.textInfoPanel.text = codes12;
        }
        if (_mscriptC.errorCodesPanel == 13)
        {
            _mScriptUI.textInfoPanel.text = codes13;
        }
    }
}
