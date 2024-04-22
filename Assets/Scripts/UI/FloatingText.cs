using UnityEngine;

namespace VRC2.UI
{
    public class FloatingText : MonoBehaviour
    {
        // local position
        public Vector3 offset;

        private OVRCameraRig _cameraRig;

        public OVRCameraRig cameraRig
        {
            get
            {
                if (_cameraRig == null)
                {
                    _cameraRig = FindObjectOfType<OVRCameraRig>();
                }

                return _cameraRig;
            }
        }

        private Transform _camera => cameraRig.centerEyeAnchor;

        void Start()
        {
        }

        void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _camera.position);
            transform.localPosition = offset;
        }
    }
}