using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRC2.Events
{
    [RequireComponent(typeof(ModalDialogGetter))]
    public class BaseEvent : MonoBehaviour
    {

        private ModalDialog _modalDialog;

        public ModalDialog modalDialog
        {
            get { return _modalDialog; }
        }

        public void Awake()
        {
            _modalDialog = gameObject.GetComponent<ModalDialogGetter>().ModalDialog;
        }
        
        public virtual void Execute()
        {

        }
    }
}
