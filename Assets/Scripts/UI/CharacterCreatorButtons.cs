using System;
using UnityEngine;
using UnityEngine.UI;

namespace VRC2.UI
{
    public class CharacterCreatorButtons : MonoBehaviour
    {
        [Header("Buttons")]
        public GameObject genders;
        public GameObject hairs;
        public GameObject skins;

        [Space(30)] [Header("UI")] public GameObject genderUI;
        public GameObject hairUI;
        public GameObject skinUI;

        private void Start()
        {
            genders.GetComponent<Button>().onClick.AddListener(OnGenderClicked);
            hairs.GetComponent<Button>().onClick.AddListener(OnHairsClicked);
            skins.GetComponent<Button>().onClick.AddListener(OnSkinsClicked);
        }

        void SetActiveness(bool gender, bool hair, bool skin)
        {
            genderUI.SetActive(gender);
            hairUI.SetActive(hair);
            skinUI.SetActive(skin);
        }

        void OnGenderClicked()
        {
            SetActiveness(!genderUI.activeSelf, false, false);
        }

        void OnHairsClicked()
        {
            SetActiveness(false, !hairUI.activeSelf, false);
        }

        void OnSkinsClicked()
        {
            SetActiveness(false, false, !skinUI.activeSelf);
        }
    }
}