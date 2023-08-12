// refer to: AutoMoveTowardsTargetProvider

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

namespace VRC2.Hack
{
    public class DistanceLimitedAutoMoveTowardsTargetProvider : MonoBehaviour, IMovementProvider
    {

        [SerializeField] private PoseTravelData _travellingData = PoseTravelData.DEFAULT;

        public PoseTravelData TravellingData
        {
            get { return _travellingData; }
            set { _travellingData = value; }
        }

        [SerializeField, Interface(typeof(IPointableElement))]
        private UnityEngine.Object _pointableElement;

        [Header("Distance Limitation")] [SerializeField]
        [Tooltip("Negative means no limitation")]
        private float maxDistance = 0.2f;

        public IPointableElement PointableElement { get; private set; }

        private bool _started;

        public List<AutoMoveTowardsTarget> _movers = new List<AutoMoveTowardsTarget>();
        
        //// hack
        private AutoMoveTowardsTarget _firstMover;
        [HideInInspector]
        public bool IsValid
        {
            get
            {
                if (_firstMover != null) return _firstMover.IsValid;
                
                return false;
            }
        }

        protected virtual void Awake()
        {
            PointableElement = _pointableElement as IPointableElement;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            this.AssertField(_pointableElement, nameof(_pointableElement));
            this.EndStart(ref _started);
        }

        private void LateUpdate()
        {
            for (int i = _movers.Count - 1; i >= 0; i--)
            {
                AutoMoveTowardsTarget mover = _movers[i];
                if (mover.Aborting)
                {
                    mover.Tick();
                    if (mover.Stopped)
                    {
                        _movers.Remove(mover);
                    }
                }
            }
        }

        public IMovement CreateMovement()
        {
            AutoMoveTowardsTarget mover = new AutoMoveTowardsTarget(_travellingData, PointableElement, maxDistance);
            //// hack
            _firstMover = mover;
            
            mover.WhenAborted += HandleAborted;
            return mover;
        }

        private void HandleAborted(AutoMoveTowardsTarget mover)
        {
            mover.WhenAborted -= HandleAborted;
            _movers.Add(mover);
        }

        #region Inject

        public void InjectAllAutoMoveTowardsTargetProvider(IPointableElement pointableElement)
        {
            InjectPointableElement(pointableElement);
        }

        public void InjectPointableElement(IPointableElement pointableElement)
        {
            PointableElement = pointableElement;
            _pointableElement = pointableElement as UnityEngine.Object;
        }

        #endregion
    }

    /// <summary>
    /// This IMovement stores the initial Pose, and in case
    /// of an aborted movement it will finish it itself.
    /// </summary>
    public class AutoMoveTowardsTarget : IMovement
    {
        private PoseTravelData _travellingData;
        private IPointableElement _pointableElement;

        //// hack
        public Pose Pose
        {
            get
            {
                if (_tween == null)
                {
                    return _source;
                }

                return _tween.Pose;
            }
        }

        // don't change object when grab (especially for pipe)
        public bool IsValid => _tween != null;
        
        public bool Stopped => _tween == null || _tween.Stopped;
        public bool Aborting { get; private set; }

        public Action<AutoMoveTowardsTarget> WhenAborted = delegate { };

        private UniqueIdentifier _identifier;
        public int Identifier => _identifier.ID;

        private Tween _tween;
        private Pose _target;
        private Pose _source;
        private bool _eventRegistered;

        private float _maxDistance;

        public AutoMoveTowardsTarget(PoseTravelData travellingData, IPointableElement pointableElement,
            float maxDistance)
        {
            _identifier = UniqueIdentifier.Generate();
            _travellingData = travellingData;
            _pointableElement = pointableElement;
            _maxDistance = maxDistance;
        }

        public void MoveTo(Pose target)
        {
            //// hack
            // get distance
            var distance = Vector3.Distance(_source.position, target.position);

            // don't move when distance exceeds maximum
            if (_maxDistance > 0 && distance > _maxDistance)
            {
                Debug.LogWarning(
                    $"Distance grab exceeds max distance ({_maxDistance.ToString("f2")}): {distance.ToString("f2")}");

                // make it not move
                _tween = null;
            }
            else
            {
                AbortSelfAligment();
                
                _target = target;
                _tween = _travellingData.CreateTween(_source, target);
                if (!_eventRegistered)
                {
                    _pointableElement.WhenPointerEventRaised += HandlePointerEventRaised;
                    _eventRegistered = true;
                }   
            }
        }

        public void UpdateTarget(Pose target)
        {
            //// hack
            if (_tween == null) return;
            
            _target = target;
            _tween.UpdateTarget(_target);
        }

        public void StopAndSetPose(Pose pose)
        {
            if (_eventRegistered)
            {
                _pointableElement.WhenPointerEventRaised -= HandlePointerEventRaised;
                _eventRegistered = false;
            }

            _source = pose;
            if (_tween != null && !_tween.Stopped)
            {
                GeneratePointerEvent(PointerEventType.Hover);
                GeneratePointerEvent(PointerEventType.Select);
                Aborting = true;
                WhenAborted.Invoke(this);
            }
        }

        public void Tick()
        {
            //// hack
            if (_tween == null) return;
            
            _tween.Tick();
            if (Aborting)
            {
                GeneratePointerEvent(PointerEventType.Move);
                if (_tween.Stopped)
                {
                    AbortSelfAligment();
                }
            }
        }

        private void HandlePointerEventRaised(PointerEvent evt)
        {
            if (evt.Type == PointerEventType.Select || evt.Type == PointerEventType.Unselect)
            {
                AbortSelfAligment();
            }
        }

        private void AbortSelfAligment()
        {
            if (Aborting)
            {
                Aborting = false;

                GeneratePointerEvent(PointerEventType.Unselect);
                GeneratePointerEvent(PointerEventType.Unhover);
            }
        }

        private void GeneratePointerEvent(PointerEventType pointerEventType)
        {
            PointerEvent evt = new PointerEvent(Identifier, pointerEventType, Pose);
            _pointableElement.ProcessPointerEvent(evt);
        }
    }
}