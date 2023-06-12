using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRC2
{
    public class PipeMenuHandler : MonoBehaviour
    {
        [Header("Dialog Window")] [SerializeField]
        private ModalDialog modalDialog;

        private void Start()
        {
            // disable modal dialog first
            modalDialog.enabled = false;
        }

        public void OnPickAPipe()
        {
            Debug.Log("You clicked Pick A pipe");
        }
    }
}