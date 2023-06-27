using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRC2
{

    public class InstructionImageManager : MonoBehaviour
    {
        [Header("Quad")] public GameObject quad;

        [Header("Image")] 
        [Tooltip("Folder name under the Assets/Resource folder")]
        public string folder;

        [Tooltip("No filetype extension, e.g., using `image` not `image.png`")]
        public string filename;

        private Texture _texture;
        // Start is called before the first frame update
        void Start()
        {
            SetImageAsTexture();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetImageAsTexture()
        {
            var name = $"{folder}/{filename}";
            _texture = Resources.Load<Texture2D>(name);
            Debug.Log(_texture.dimension);
            quad.GetComponent<MeshRenderer>().materials[0].mainTexture = _texture;
        }
    }
}