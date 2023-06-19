using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC2;

namespace VRC2
{
    public class ModalDialogGetter : MonoBehaviour
    {
        [SerializeField] private ModalDialog _modalDialog;

        public ModalDialog ModalDialog
        {
            get => _modalDialog;
            // set => _modalDialog = value;
        }
    }
}