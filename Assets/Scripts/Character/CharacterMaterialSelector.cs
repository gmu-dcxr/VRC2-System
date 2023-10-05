using System;
using UnityEngine;

namespace VRC2.Character
{
    public class CharacterMaterialSelector : MonoBehaviour
    {
        [Header("Renders")] public SkinnedMeshRenderer faceRenderer;
        public SkinnedMeshRenderer handRenderer;

        [Space(30)] [Header("Male")] public Material skinMale;
        public Material skinMale2;
        public Material eyebrowMale;
        public Material eyelashMale;

        [Space(30)] [Header("Female")] public Material skinFemale;
        public Material eyebrowFemale;
        public Material eyelashFemale;

        [Space(30)] [Header("Hair")] public GameObject hair1;
        public GameObject hair2;
        
        private GameObject currentHair;

        [Space(30)] [Header("Hands")] public Material hand1;
        public Material hand2;
        public Material handfemale;


        private Material[] _maleMaterials;
        private Material[] _femaleMaterials;
        private Material[] _skinMaterials;
        private Material[] _handMaterials;
        private GameObject[] _hairStyles;

        [HideInInspector] public int hairStyleIndex = 0; //-1 means bald
        [HideInInspector] public bool isFemale = true;
        [HideInInspector] public int skinStyleIndex = 0;
        [HideInInspector] public int handStyleIndex = 0;

        private Material[] maleMaterials
        {
            get
            {
                if (_maleMaterials == null)
                {
                    _maleMaterials = new Material[3];
                    _maleMaterials[0] = skinMale;
                    _maleMaterials[1] = eyebrowMale;
                    _maleMaterials[2] = eyelashMale;
                }

                return _maleMaterials;
            }
        }

        private Material[] femaleMaterials
        {
            get
            {
                if (_femaleMaterials == null)
                {
                    _femaleMaterials = new Material[3];
                    _femaleMaterials[0] = skinFemale;
                    _femaleMaterials[1] = eyebrowFemale;
                    _femaleMaterials[2] = eyelashFemale;
                }

                return _femaleMaterials;
            }
        }

        private Material[] skinMaterials
        {
            get
            {
                if (_skinMaterials == null)
                {
                    _skinMaterials = new Material[3];
                    _skinMaterials[0] = skinFemale;
                    _skinMaterials[1] = skinMale;
                    _skinMaterials[2] = skinMale2;
                }

                return _skinMaterials;
            }
        }

        private Material[] handMaterials
        {
            get
            {
                if (_handMaterials == null)
                {
                    _handMaterials = new Material[3];
                    _handMaterials[0] = handfemale;
                    _handMaterials[1] = hand1;
                    _handMaterials[2] = hand2;
                }

                return _handMaterials;
            }
        }

        private GameObject[] hairStyles
        {
            get
            {
                if (_hairStyles == null)
                {
                    _hairStyles = new GameObject[2];
                    _hairStyles[0] = hair1;
                    _hairStyles[1] = hair2;
                }

                return _hairStyles;
            }
        }

        private void Start()
        {
            currentHair = hair1;
            // init styles
            hairStyleIndex = 0;
            isFemale = true;
            skinStyleIndex = 0;
        }

        public void ChangeToMale()
        {
            // faceRenderer.material = skinMale;
            // handRenderer.material = hand1;
            // currentHair.SetActive(false);

            isFemale = false;
            skinStyleIndex = 1;
            
            UpdateAppearance();
        }

        public void ChangeToFemale()
        {
            // faceRenderer.materials = femaleMaterials;
            // handRenderer.material = handfemale;
            // currentHair.SetActive(true);

            isFemale = true;
            skinStyleIndex = 0;
            UpdateAppearance();
        }

        public void ChangeHair(int i)
        {
            // GameObject[] hairs = hairStyles;
            // currentHair.SetActive(false);
            //
            // currentHair = hairs[i];
            //
            // currentHair.SetActive(true);

            hairStyleIndex = i;
            UpdateAppearance();
        }

        public void GoBald()
        {
            // currentHair.SetActive(false);
            hairStyleIndex = -1;
            UpdateAppearance();
        }

        public void ChangeSkin(int i)
        {
            skinStyleIndex = i;
            UpdateAppearance();
        }

        public void UpdateAppearance(bool female, int hair, int skin)
        {
            isFemale = female;
            hairStyleIndex = hair;
            skinStyleIndex = skin;

            UpdateAppearance(false);
        }

        void UpdateGlobalConstants()
        {
            if (isFemale)
            {
                // update global constants
                GlobalConstants.playerGender = PlayerGender.Female;
            }
            else
            {
                GlobalConstants.playerGender = PlayerGender.Male;
            }

            GlobalConstants.playerHairIndex = hairStyleIndex;
            GlobalConstants.playerSkinIndex = skinStyleIndex;
        }


        private void UpdateAppearance(bool updateConstants = true)
        {
            if (updateConstants)
            {
                UpdateGlobalConstants();   
            }
            
            // fix current hair
            if (currentHair == null)
            {
                currentHair = hair1;
            }
            
            if (isFemale)
            {
                faceRenderer.materials = femaleMaterials;
                handRenderer.material = handfemale;
                currentHair.SetActive(true);
            }
            else
            {
                faceRenderer.material = skinMale;
                handRenderer.material = hand1;
                currentHair.SetActive(false);
            }

            // hide hairs
            hair1.SetActive(false);
            hair2.SetActive(false);

            if (hairStyleIndex >= 0)
            {
                currentHair = hairStyles[hairStyleIndex];
            }

            // change hair, no hairs for male
            if (hairStyleIndex >= 0 && isFemale)
            {
                currentHair.SetActive(true);
            }
            else
            {
                currentHair.SetActive(false);
            }

            // change skin
            Material[] skins = skinMaterials;
            Material[] hands = handMaterials;
            Material[] newSkin = maleMaterials;
            newSkin[0] = skins[skinStyleIndex];
            handRenderer.material = hands[skinStyleIndex];
            faceRenderer.materials = newSkin;
        }
    }
}