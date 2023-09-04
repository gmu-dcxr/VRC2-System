using UnityEngine;
using VRC2.Pipe;

namespace VRC2.Animations
{
    public class RobotDogArmGrabber : MonoBehaviour
    {
        private string robotTag = "RobotArmGripper";
        
        private void OnTriggerEnter(Collider other)
        {
            var pipeTag = GlobalConstants.pipeObjectTag;
            if (other.gameObject.CompareTag(pipeTag))
            {
                var flag = PipeHelper.IsSimpleStraightNotcutPipe(other.gameObject);
                print(flag);
                // transform.GetComponent<Rigidbody>().useGravity = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var pipeTag = GlobalConstants.pipeObjectTag;
            if (other.gameObject.CompareTag(pipeTag))
            {
                // transform.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }
}