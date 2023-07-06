using UnityEngine;

namespace VRC2
{
    public class CapacityMonitor: MonoBehaviour
    {


        public void OnUseGlue()
        {
            if (GlobalConstants.UseGlue())
            {
                // succeed
            }
            else
            {
                // fail because of used out
            }
        }

        public void OnUseBox()
        {
            // box
            if (GlobalConstants.UseBox())
            {
                // succeed
            }
            else
            {
                // fail because of used out
            }
        }

        public void OnUsePipe()
        {
            
        }

        public void OnUseClamp()
        {
            // clamp
            if (GlobalConstants.UseClamp())
            {
                // succeed
            }
            else
            {
                // fail because of used out
            }
        }
    }
}