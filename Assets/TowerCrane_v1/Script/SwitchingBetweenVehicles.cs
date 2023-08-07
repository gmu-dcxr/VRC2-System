using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchingBetweenVehicles : MonoBehaviour
{
    public CraneCarController_L scriptCraneCarController;
    private TowerControllerCrane scriptTowerController;
    private TowerPlatformInstallation scriptTPL;
    public Camera carCrane;
    public Camera towerCrane;
    public KeyCode switchingCrane = KeyCode.M;
    public Image window;
    public Text description;
    public Text _key;
    public KeyCode infoPanel = KeyCode.N;
    public KeyCode infoControllerCrane = KeyCode.B;
    private bool infoControllerCrane_Bool = true;
    private bool infoPanel_Bool = true;
    public KeyCode nextList = KeyCode.Alpha2;
    public KeyCode backList = KeyCode.Alpha1;
    public Image listPanel;
    public Text numberList;
    public Text infoDisplayKey1;
    public Text infoDisplayKey2;
    public Text descriptionElement;
    public Text descriptionControll;
    public int numberListElement_Int = 1;
    public int numberListControll_Int = 1;
    [HideInInspector]
    public int blockCrane_Int = 0;
    [HideInInspector]
    public bool blockCamera_Bool = false;
    public GameObject[] listInfoPanel;
    public GameObject[] listInfoPanelControll;

    void Start()
    {
        blockCrane_Int = 1;
        scriptCraneCarController.scriptSwitch = this.gameObject.GetComponent<SwitchingBetweenVehicles>();
        scriptTowerController = gameObject.GetComponent<TowerControllerCrane>();
        _key.text = switchingCrane.ToString();
        scriptTPL = gameObject.GetComponent<TowerPlatformInstallation>();

    }
    void Update()
    {
        if (Input.GetKeyDown(switchingCrane) && window.enabled == true)
        {
            SwitchCrane();
        }
        if (Input.GetKeyDown(infoPanel)) // InfoPanel
        {
            if (infoPanel_Bool == true)
            {
                if (infoControllerCrane_Bool == false)
                {
                    descriptionControll.enabled = false;
                    infoControllerCrane_Bool = true;
                    for (int i = 0; i < listInfoPanelControll.Length; i++)
                    {
                        listInfoPanelControll[i].SetActive(false);
                    }
                }
                listPanel.enabled = true;
                numberList.enabled = true;
                infoDisplayKey1.enabled = true;
                infoDisplayKey2.enabled = true;
                descriptionElement.enabled = true;
                infoPanel_Bool = false;
                ControllerListNext();
            }
            else if (infoPanel_Bool == false)
            {
                listPanel.enabled = false;
                numberList.enabled = false;
                infoDisplayKey1.enabled = false;
                infoDisplayKey2.enabled = false;
                descriptionElement.enabled = false;
                for (int i = 0; i < listInfoPanel.Length; i++)
                {
                    listInfoPanel[i].SetActive(false);
                }
                infoPanel_Bool = true;
            }
        }
        if (Input.GetKeyDown(infoControllerCrane))
        {
            if (infoControllerCrane_Bool == true)
            {
                if (infoPanel_Bool == false)
                {
                    infoPanel_Bool = true;
                    for (int i = 0; i < listInfoPanel.Length; i++)
                    {
                        listInfoPanel[i].SetActive(false);
                    }
                    descriptionElement.enabled = false;
                }
                listPanel.enabled = true;
                numberList.enabled = true;
                infoDisplayKey1.enabled = true;
                infoDisplayKey2.enabled = true;
                descriptionControll.enabled = true;
                infoControllerCrane_Bool = false;
                ControllerListNext();
            }
            else if (infoControllerCrane_Bool == false)
            {
                listPanel.enabled = false;
                numberList.enabled = false;
                infoDisplayKey1.enabled = false;
                infoDisplayKey2.enabled = false;
                descriptionControll.enabled = false;
                infoControllerCrane_Bool = true;
                for (int i = 0; i < listInfoPanelControll.Length; i++)
                {
                    listInfoPanelControll[i].SetActive(false);
                }
            }
        }
        if (infoPanel_Bool == false || infoControllerCrane_Bool == false)
        {
            if (Input.GetKeyDown(nextList))
            {
                if (infoPanel_Bool == false && numberListElement_Int < 6)
                {
                    numberListElement_Int += 1;
                    ControllerListNext();
                }
                if (infoControllerCrane_Bool == false && numberListControll_Int < 3)
                {
                    numberListControll_Int += 1;
                    ControllerListNext();
                }
            }
            else if (Input.GetKeyDown(backList))
            {
                if(infoPanel_Bool==false && numberListElement_Int > 1)
                {
                    numberListElement_Int -= 1;
                    ControllerListBack();
                }
                if (infoControllerCrane_Bool == false && numberListControll_Int > 1)
                {
                    numberListControll_Int -= 1;
                    ControllerListBack();
                }
            }
        }
    }
    private void SwitchCrane()
    {
       if(blockCrane_Int == 1)
        {
            if (scriptCraneCarController.pover_Bool == false)
            {
                scriptCraneCarController.PoverCrane();
            }
            carCrane.enabled = false;
            towerCrane.enabled = true;
            carCrane.GetComponent<AudioListener>().enabled = false;
            towerCrane.GetComponent<AudioListener>().enabled = true;
            scriptTowerController.blockControllerCrane = true;
            scriptTPL.towerPaneController.SetActive(true);
            blockCamera_Bool = true;
            scriptTowerController.distanceCartUI.text = Vector3.Distance(scriptTowerController.pointCheckDistanceUI.position, scriptTowerController.boomCart.position).ToString("0.0") + "m";
            if (scriptTPL.autoInstallCounterweightUpPlatform == true)
            {
                scriptTowerController.counterweightUI_Int = 100;
                scriptTowerController.counterweightUI.text = scriptTowerController.counterweightUI_Int.ToString() + "%";
            }
            blockCrane_Int = 2;
        }
        else if(blockCrane_Int == 2)
        {
            carCrane.enabled = true;
            towerCrane.enabled = false;
            carCrane.GetComponent<AudioListener>().enabled = true;
            towerCrane.GetComponent<AudioListener>().enabled = false;
            scriptTowerController.blockControllerCrane = false;
            scriptTPL.towerPaneController.SetActive(false);
            blockCamera_Bool = false;
            blockCrane_Int = 1;
        }
    }
    private void ControllerListNext()
    {
        if (infoPanel_Bool == false)
        {
            if (numberListElement_Int == 1)
            {
                numberList.text = "1";
                listInfoPanel[0].SetActive(true);
            }
            else if (numberListElement_Int == 2)
            {
                numberList.text = "2";
                listInfoPanel[1].SetActive(true);
                listInfoPanel[0].SetActive(false);
            }
            else if (numberListElement_Int == 3)
            {
                numberList.text = "3";
                listInfoPanel[2].SetActive(true);
                listInfoPanel[1].SetActive(false);
            }
            else if (numberListElement_Int == 4)
            {
                numberList.text = "4";
                listInfoPanel[3].SetActive(true);
                listInfoPanel[2].SetActive(false);
            }
            else if (numberListElement_Int == 5)
            {
                numberList.text = "5";
                listInfoPanel[4].SetActive(true);
                listInfoPanel[3].SetActive(false);
            }
            else if (numberListElement_Int == 6)
            {
                numberList.text = "6";
                listInfoPanel[5].SetActive(true);
                listInfoPanel[4].SetActive(false);
            }
        }
        else if (infoControllerCrane_Bool == false)
        {
            if (numberListControll_Int == 1)
            {
                numberList.text = "1";
                listInfoPanelControll[0].SetActive(true);
            }
            else if (numberListControll_Int == 2)
            {
                numberList.text = "2";
                listInfoPanelControll[1].SetActive(true);
                listInfoPanelControll[0].SetActive(false);
            }
            else if (numberListControll_Int == 3)
            {
                numberList.text = "3";
                listInfoPanelControll[2].SetActive(true);
                listInfoPanelControll[1].SetActive(false);
            }
        }
    }
    private void ControllerListBack()
    {
        if (infoPanel_Bool == false)
        {
            if (numberListElement_Int == 5)
            {
                numberList.text = "5";
                listInfoPanel[5].SetActive(false);
                listInfoPanel[4].SetActive(true);
            }
            else if (numberListElement_Int == 4)
            {
                numberList.text = "4";
                listInfoPanel[4].SetActive(false);
                listInfoPanel[3].SetActive(true);
            }
            else if (numberListElement_Int == 3)
            {
                numberList.text = "3";
                listInfoPanel[3].SetActive(false);
                listInfoPanel[2].SetActive(true);
            }
            else if (numberListElement_Int == 2)
            {
                numberList.text = "2";
                listInfoPanel[2].SetActive(false);
                listInfoPanel[1].SetActive(true);
            }
            else if (numberListElement_Int == 1)
            {
                numberList.text = "1";
                listInfoPanel[1].SetActive(false);
                listInfoPanel[0].SetActive(true);
            }
        }
        else if(infoControllerCrane_Bool == false)
        {
            if (numberListControll_Int == 3)
            {
                numberList.text = "3";
                listInfoPanelControll[3].SetActive(false);
                listInfoPanelControll[2].SetActive(true);
            }
            else if (numberListControll_Int == 2)
            {
                numberList.text = "2";
                listInfoPanelControll[2].SetActive(false);
                listInfoPanelControll[1].SetActive(true);
            }
            else if (numberListControll_Int == 1)
            {
                numberList.text = "1";
                listInfoPanelControll[1].SetActive(false);
                listInfoPanelControll[0].SetActive(true);
            }
        }
    }
}
