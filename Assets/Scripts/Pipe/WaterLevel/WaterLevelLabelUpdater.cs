using System;
using TMPro;
using UnityEngine;

namespace VRC2.Events
{
    public class WaterLevelLabelUpdater : MonoBehaviour
    {
        [Header("Label")] public TextMeshPro textMeshPro;

        private void Start()
        {

        }

        private void Update()
        {
            var d = GetDegree();
            textMeshPro.text = $"{d}";
        }

        int GetDegree()
        {
            var t = gameObject.transform.rotation.eulerAngles;
            return (int)(Math.Abs(t.x % 90));
        }
    }
}