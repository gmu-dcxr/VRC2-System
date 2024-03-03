using System;
using System.IO;
using UnityEngine;
using VRC2.Scenarios.ScenarioFactory;
using VRC2.ScenariosV2.Tool;

namespace VRC2.ScenariosV2.Vehicle
{
    public class Irrelevant : Base
    {
        #region Audio filename
        private string GetAudioFileName(int key)
        {
            return $"{ClsName}_{key}.wav";
        }

        #endregion

        #region Callbacks

        public void Irrelevant_accidents_1(object[] parameters)
        {
            BluePrint("Invoke Irrelevant_accidents_1");
            var filename = GetAudioFileName(1);
            BluePrint(filename);
            warningController.PlayAudioClip(filename, null);
        }

        public void Irrelevant_accidents_2(object[] parameters)
        {
            BluePrint("Invoke Irrelevant_accidents_2");
            var filename = GetAudioFileName(2);
            warningController.PlayAudioClip(filename, null);
        }

        public void Irrelevant_accidents_3(object[] parameters)
        {
            BluePrint("Invoke Irrelevant_accidents_3");
            var filename = GetAudioFileName(3);
            warningController.PlayAudioClip(filename, null);
        }

        public void Irrelevant_accidents_4(object[] parameters)
        {
            var filename = GetAudioFileName(4);
            warningController.PlayAudioClip(filename, null);
        }

        public void Irrelevant_accidents_5(object[] parameters)
        {
            var filename = GetAudioFileName(5);
            warningController.PlayAudioClip(filename, null);
        }

        public void Irrelevant_accidents_6(object[] parameters)
        {
            var filename = GetAudioFileName(6);
            warningController.PlayAudioClip(filename, null);
        }

        public void Irrelevant_accidents_7(object[] parameters)
        {
            var filename = GetAudioFileName(7);
            warningController.PlayAudioClip(filename, null);
        }

        public void Irrelevant_accidents_8(object[] parameters)
        {
            var filename = GetAudioFileName(8);
            warningController.PlayAudioClip(filename, null);
        }

        public void Irrelevant_accidents_9(object[] parameters)
        {
            var filename = GetAudioFileName(9);
            warningController.PlayAudioClip(filename, null);
        }

        public void Irrelevant_accidents_10(object[] parameters)
        {
            var filename = GetAudioFileName(10);
            warningController.PlayAudioClip(filename, null);
        }
        
        public void Irrelevant_accidents_11(object[] parameters)
        {
            // TODO: generate files and update implementation
            BluePrint("Irrelevant_accidents_11");
            print(showWaring(parameters));
        }

        #endregion
    }
}