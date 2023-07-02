using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC2;
using VRC2.Events;

namespace VRC2
{

    public class ModalDialogManager : MonoBehaviour
    {
        [SerializeField] private GameObject _dialogGameObject;

        private ModalDialog _modalDialog;

        public ModalDialog modalDialog
        {
            get
            {
                if (_modalDialog == null)
                {
                    _modalDialog = _dialogGameObject.GetComponent<ModalDialog>();
                }

                return _modalDialog;
            }
        }

        public bool checkResult
        {
            get => _modalDialog.checkResult;
            set => _modalDialog.checkResult = value;
        }

        public string content
        {
            get => _modalDialog.content;
            set => _modalDialog.content = value;
        }


        public bool IsShowing()
        {
            return _dialogGameObject.activeSelf;
        }

        public void Show(bool flag)
        {
            _dialogGameObject.SetActive(flag);
        }

        public void UpdateDialog(string title, string content, string btn1, string btn2,
            PipeInstallEvent ev = PipeInstallEvent.EmptyEvent)
        {
            _modalDialog.UpdateDialog(title, content, btn1, btn2, ev);
        }
    }
}