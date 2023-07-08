using UnityEngine;

namespace VRC2.Events
{
    public class P1CheckGlueEvent : BaseEvent
    {
        public GameObject experimenter;
        public GameObject glue;
        public GameObject destination;

        private Vector3 startPosition;
        private Vector3 gluePosition;
        private Vector3 destPosition;

        void Start()
        {
            startPosition = experimenter.transform.position;
            gluePosition = glue.transform.position;
            destPosition = destination.transform.position;
        }

        public void MoveToDestination()
        {
            Debug.Log("MoveToDestination");
            experimenter.transform.position = destPosition;
        }

        public void RefillGlue()
        {
            Debug.Log("RefillGlue");
            GlobalConstants.currentGlueCapacitiy = GlobalConstants.glueInitialCapacity;
        }

        public void BackToGluePosition()
        {
            Debug.Log("BackToGluePosition");
            experimenter.transform.position = gluePosition;
        }

        public void MoveAway()
        {
            Debug.Log("MoveAway");
            experimenter.transform.position = startPosition;
        }

        public override void Execute()
        {
            MoveToDestination();
            BackToGluePosition();
            RefillGlue();
            MoveAway();
        }
    }
}