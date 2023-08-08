using UnityEngine;

namespace VRC2.Crane
{
    public class VRCraneController : TowerControllerCrane
    {
        // left controller: forward/backward - left/right
        // right controller: up/down - rotation: index trigger: seize/release

        void Start()
        {
            base.Start();
        }

        void Update()
        {
            base.Update();
        }

        void FixedUpdate()
        {
            base.FixedUpdate();
        }

        void LateUpdate()
        {
            base.LateUpdate();
        }

        public override bool isForwardBoomCartUp()
        {
            return false;
        }

        public override bool isBackBoomCart()
        {
            return false;
        }

        public override bool isBackBoomCartUp()
        {
            return false;
        }

        public override bool isLeftCrane()
        {
            return false;
        }

        public override bool isLeftCraneUp()
        {
            return false;
        }

        public override bool isRightCrane()
        {
            return false;
        }

        public override bool isRightCraneUp()
        {
            return false;
        }

        public override bool isUpMovingHook()
        {
            return false;
        }

        public override bool isUpMovingHookUp()
        {
            return false;
        }

        public override bool isDownMovingHook()
        {
            return false;
        }

        public override bool isDownMovingHookUp()
        {
            return false;
        }

        public override bool isLeftRotationCargo()
        {
            return false;
        }

        public override bool isLeftRotationCargoUp()
        {
            return false;
        }

        public override bool isRightRotationCargo()
        {
            return false;
        }

        public override bool isRightRotationCargoUp()
        {
            return false;
        }

        public override bool isSeizeTheCargo()
        {
            return false;
        }
    }
}