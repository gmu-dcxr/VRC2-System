using UnityEngine;

namespace VRC2.Events
{
    public class P1CheckClampEvent : BaseEvent
    {
        public GameObject experimenter;
        public GameObject clamp;
        public GameObject destination;

        private Vector3 startPosition;
        private Vector3 clampPosition;
        private Vector3 destPosition;

        void Start()
        {
            startPosition = experimenter.transform.position;
            clampPosition = clamp.transform.position;
            destPosition = destination.transform.position;
        }

        public void MoveToDestination()
        {
            Debug.Log("MoveToDestination");
            experimenter.transform.position = destPosition;
        }

        public void RefillClamp()
        {
            Debug.Log("RefillClamp");
            GlobalConstants.currentClampCount = GlobalConstants.clampInitialCount;
        }

        public void BackToClampPosition()
        {
            Debug.Log("BackToClampPosition");
            experimenter.transform.position = clampPosition;
        }

        public void MoveAway()
        {
            Debug.Log("MoveAway");
            experimenter.transform.position = startPosition;
        }

        public override void Execute()
        {
            MoveToDestination();
            BackToClampPosition();
            RefillClamp();
            MoveAway();
        }
    }
}