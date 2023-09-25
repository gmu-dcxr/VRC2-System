using System;
using UnityEngine;

namespace VRC2.Character
{
    public class GenderSelector : MonoBehaviour
    {
        [Header("Render")] public SkinnedMeshRenderer renderer;
        public SkinnedMeshRenderer handRenderer;

        [Space(30)] [Header("Male")] public Material skinMale;
        public Material eyebrowMale;
        public Material eyelashMale;
        public Material handMale;

        [Space(30)] [Header("Female")] public Material skinFemale;
        public Material eyebrowFemale;
        public Material eyelashFemale;
        public Material handFemale;

        public GameObject hair;


        private Material[] _maleMaterials;
        private Material[] _femaleMaterials;

        private Material[] _maleHandMaterials;
        private Material[] _femaleHandMaterials;

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

        private Material[] maleHandMaterials
        {
            get
            {
                if (_maleHandMaterials == null)
                {
                    _maleHandMaterials = new Material[1];
                    _maleHandMaterials[0] = handMale;
                }

                return _maleHandMaterials;
            }
        }

        private Material[] femaleHandMaterials
        {
            get
            {
                if (_femaleHandMaterials == null)
                {
                    _femaleHandMaterials = new Material[1];
                    _femaleHandMaterials[0] = handFemale;
                }

                return _femaleHandMaterials;
            }
        }

        private void Start()
        {
            ChangeToFemale();
            // ChangeToMale();
        }

        public void ChangeToMale()
        {
            renderer.materials = maleMaterials;
            handRenderer.materials = maleHandMaterials;
            hair.SetActive(false);
        }

        public void ChangeToFemale()
        {
            renderer.materials = femaleMaterials;
            handRenderer.materials = femaleHandMaterials;
            hair.SetActive(true);
        }
    }
}