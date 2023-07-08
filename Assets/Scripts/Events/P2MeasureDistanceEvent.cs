using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Fusion;
using Oculus.Interaction;

namespace VRC2.Events
{

    public class P2MeasureDistanceEvent : BaseEvent
    {
        public GameObject wall;

        private DistanceMeasurer _distanceMeasurer;
        private RayInteractable _rayInteractable;

        void Start()
        {
            _distanceMeasurer = wall.GetComponent<DistanceMeasurer>();
            _rayInteractable = wall.GetComponent<RayInteractable>();

            // disable at the start
            _distanceMeasurer.enabled = false;
            _distanceMeasurer.enabled = false;
        }

        public override void Execute()
        {
            Switch();
        }

        void Switch()
        {
            var enabled = _distanceMeasurer.enabled;
            _distanceMeasurer.enabled = !enabled;
            _rayInteractable.enabled = !enabled;
        }
    }
}