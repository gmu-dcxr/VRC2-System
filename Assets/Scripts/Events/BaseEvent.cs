using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace VRC2.Events
{
    [RequireComponent(typeof(ModalDialogManager))]
    public class BaseEvent : NetworkBehaviour
    {

        private ModalDialogManager _dialogManager;

        public ModalDialogManager dialogManager
        {
            get { return _dialogManager; }
        }
        
        public void Start()
        {
            _dialogManager = gameObject.GetComponent<ModalDialogManager>();
        }
        
        public virtual void Execute()
        {

        }
    }
}
