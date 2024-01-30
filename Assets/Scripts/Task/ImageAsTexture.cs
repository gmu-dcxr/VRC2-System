// this class is almost the same as QuadImageManager except fields are not ReadOnly.
using UnityEngine;
using UnityEngine.UI;

namespace VRC2.Task
{
    public class ImageAsTexture : MonoBehaviour
    {
        [Header("Image")] [Tooltip("Folder name under the Assets/Resource folder")]
        public string folder;

        [Tooltip("No filetype extension, e.g., using `image` not `image.png`")]
        public string filename;

        private Texture2D _texture;

        private MeshRenderer renderer
        {
            get => gameObject.GetComponent<MeshRenderer>();
        }

        private Image image
        {
            get => gameObject.GetComponent<Image>();
        }

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

            if (renderer != null)
            {
                renderer.materials[0].mainTexture = _texture;
            }
            else if (image != null)
            {
                // var spriteRenderer = GetComponent<SpriteRenderer>();
                // var sprite = Resources.Load<Sprite>(name);
                image.material.mainTexture = _texture;
                // spriteRenderer.sprite = sprite;
            }
        }

        public void UpdateFolderFilename(string folder, string filename)
        {
            this.folder = folder;
            this.filename = filename;
            SetImageAsTexture();
        }


        public void UpdateFilename(string fn)
        {
            filename = fn;
            SetImageAsTexture();
        }
    }
}