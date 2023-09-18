using System;
using UnityEngine;

namespace VRC2.Character
{
    public class GenderSelector : MonoBehaviour
    {
        [Header("Render")] public SkinnedMeshRenderer renderer;

        [Space(30)] [Header("Male")] public Material skinMale;
        public Material eyebrowMale;
        public Material eyelashMale;

        [Space(30)] [Header("Female")] public Material skinFemale;
        public Material eyebrowFemale;
        public Material eyelashFemale;

        public GameObject hair;


        private Material[] _maleMaterials;
        private Material[] _femaleMaterials;

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

        private void Start()
        {
            ChangeToFemale();
        }

        void ChangeToMale()
        {
            renderer.materials = maleMaterials;
            hair.SetActive(false);
        }

        void ChangeToFemale()
        {
            renderer.materials = femaleMaterials;
            hair.SetActive(true);
        }
    }
}