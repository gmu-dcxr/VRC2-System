using System;
using Fusion;
using TMPro;
using UnityEngine;

namespace VRC2.Events
{
    public class WaterLevelLabelUpdater : NetworkBehaviour
    {
        [Header("Label")] public TextMeshPro textMeshPro;

        private bool authorityConfirmed;

        private NetworkRunner _runner
        {
            get => GlobalConstants.networkRunner;
        }

        private void Start()
        {
            authorityConfirmed = false;
        }

        private void Update()
        {
            UpdateInputAuthority();

            var d = GetDegree();
            textMeshPro.text = $"{d}";
        }

        int GetDegree()
        {
            var t = gameObject.transform.rotation.eulerAngles;
            return (int)(Math.Abs(t.x % 90));
        }

        void UpdateInputAuthority()
        {
            if (authorityConfirmed || _runner == null || !_runner.IsRunning) return;

            if (_runner.IsClient)
            {
                Debug.LogWarning("Override the input authority for water level");
                var no = gameObject.GetComponent<NetworkObject>();
                // set input authority to P2
                no.AssignInputAuthority(GlobalConstants.localPlayer);
            }

            authorityConfirmed = true;
        }
    }
}