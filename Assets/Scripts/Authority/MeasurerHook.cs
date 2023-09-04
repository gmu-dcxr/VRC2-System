using System;

namespace VRC2.Authority
{
    public class MeasurerHook : AuthorityHook
    {
        private DistanceMeasurer _distanceMeasurer;

        private void Start()
        {
            _distanceMeasurer = gameObject.GetComponent<DistanceMeasurer>();
        }

        public override void DisableP1()
        {
            gameObject.SetActive(false);
        }

        public override void DisableP2()
        {

        }

        public override void EnableP2()
        {
            // set it to invisible at first
            _distanceMeasurer.enabled = false;
        }
    }
}