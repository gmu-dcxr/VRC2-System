using System;
using UnityEngine;

namespace VRC2.Character
{
    public class CharacterMaterialSelector : MonoBehaviour
    {
        [Header("Renders")] 
        public SkinnedMeshRenderer faceRenderer;
        public SkinnedMeshRenderer handRenderer;

        [Space(30)] [Header("Male")] 
        public Material skinMale;
        public Material skinMale2;
        public Material eyebrowMale;
        public Material eyelashMale;

        [Space(30)] [Header("Female")] 
        public Material skinFemale;
        public Material eyebrowFemale;
        public Material eyelashFemale;

        [Space(30)] [Header("Hair")]
        public GameObject hair1;
        public GameObject hair2;

        private GameObject currentHair;

        [Space(30)] [Header("Hands")]
        public Material hand1;
        public Material hand2;
        public Material handfemale;


        private Material[] _maleMaterials;
        private Material[] _femaleMaterials;
        private Material[] _skinMaterials; 
        private Material[] _handMaterials;
        private GameObject[] _hairStyles;

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
            ChangeToFemale();
        }

        public void ChangeToMale()
        {
            faceRenderer.material = skinMale;
            handRenderer.material = hand1;
            currentHair.SetActive(false);
        }

        public void ChangeToFemale()
        {
            faceRenderer.materials = femaleMaterials;
            handRenderer.material = handfemale;
            currentHair.SetActive(true);
        }

        public void ChangeHair(int i) 
        {
            GameObject[] hairs = hairStyles;
            currentHair.SetActive(false);

            currentHair = hairs[i];
            
            currentHair.SetActive(true);
        } 
     
        public void GoBald() 
        {
            currentHair.SetActive(false);
        }

        public void ChangeSkin(int i) 
        {
            Material[] skins = skinMaterials; 
            Material[] hands = handMaterials;
            Material[] newSkin = maleMaterials;
            newSkin[0] = skins[i];
            handRenderer.material = hands[i];
            faceRenderer.materials = newSkin;
        }
    }
}