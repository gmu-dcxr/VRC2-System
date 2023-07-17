using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRC2
{
    public class QuadImageManager : MonoBehaviour
    {
        [Header("Image")] [Tooltip("Folder name under the Assets/Resource folder")]
        public string folder;

        [Tooltip("No filetype extension, e.g., using `image` not `image.png`")]
        public string filename;

        private Texture2D _texture;

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
            print($"set image texture: {name}");
            _texture = Resources.Load<Texture2D>(name);
            _texture.alphaIsTransparency = true;
            Debug.Log(_texture.dimension);
            gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = _texture;
        }
    }
}