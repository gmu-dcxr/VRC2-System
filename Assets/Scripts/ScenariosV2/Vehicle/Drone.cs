using System;
using UnityEngine;
using VRC2.Scenarios.ScenarioFactory;
using VRC2.ScenariosV2.Tool;

namespace VRC2.ScenariosV2.Vehicle
{
    public class Drone : Base
    {
        #region Baselines

        [Header("Adaption")] private BaselineS2 _baselineS2;
        private BaselineS4 _baselineS4;

        public BaselineS2 baselineS2
        {
            get
            {
                if (_baselineS2 == null)
                {
                    _baselineS2 = FindObjectOfType<BaselineS2>();
                }

                return _baselineS2;
            }
        }

        public BaselineS4 baselineS4
        {
            get
            {
                if (_baselineS4 == null)
                {
                    _baselineS4 = FindObjectOfType<BaselineS4>();
                }

                return _baselineS4;
            }
        }

        #endregion

        void SetDroneControlAuthority(bool s2, bool s4)
        {
            baselineS2.controllingDrone = s2;
            baselineS4.controllingDrone = s4;
        }

        #region Callbacks

        private void ShowWarningS2(int idx)
        {
            baselineS2.ShowWarning(idx, audioSource);
        }

        private void ShowWarningS4(int idx)
        {
            baselineS4.ShowWarning(idx, audioSource);
        }

        public void Drone_normals_1(object[] parameters)
        {
            BluePrint("Invoked Drone_normals_1");
            SetDroneControlAuthority(true, false);
            // update leaveafter second to 30 seconds
            baselineS2.leaveAfter = 30f;
            // set change order to false
            baselineS2.changeOrder = false;
            ShowWarningS2(2);
            baselineS2.On_BaselineS2_2_Start();
        }

        public void Drone_normals_2(object[] parameters)
        {
            BluePrint("Invoked Drone_normals_2");
            SetDroneControlAuthority(true, false);
            // baselineS2.ShowWarning(3);
            // Drone Leaves automatically
        }

        public void Drone_normals_3(object[] parameters)
        {
            BluePrint("Invoked Drone_normals_3");
            SetDroneControlAuthority(true, false);
            // update leaveafter second to 20 seconds, inform change order
            baselineS2.leaveAfter = 20f;                           //5sec from spawning point to players, then it spends 20s above players delivering change order, leaves at 25th sec
            // this will inform task changes
            // set change order to true
            baselineS2.changeOrder = true;
            // baselineS2.ShowWarning(2);
            baselineS2.On_BaselineS2_2_Start();
        }


        public void Drone_normals_4(object[] parameters)
        {
            BluePrint("Invoked Drone_normals_4");
            SetDroneControlAuthority(true, false);
            ShowWarningS2(3);
            // Drone Leaves automatically
        }

        public void Drone_accidents_1(object[] parameters)
        {
            BluePrint("Invoked Drone_accidents_1");
            SetDroneControlAuthority(false, true);
            ShowWarningS4(2);
            baselineS4.On_BaselineS4_2_Start();
        }

        public void Drone_accidents_2(object[] parameters)
        {
            BluePrint("Invoked Drone_accidents_2");
            SetDroneControlAuthority(false, true);
            ShowWarningS4(3);
            baselineS4.On_BaselineS4_3_Start();
        }

        public void Drone_accidents_3(object[] parameters)
        {
            BluePrint("Invoked Drone_accidents_4");
            SetDroneControlAuthority(false, true);
            ShowWarningS4(4);
            baselineS4.On_BaselineS4_4_Start();
        }

        public void Drone_accidents_4(object[] parameters)
        {
            BluePrint("Invoked Drone_accidents_4");
            SetDroneControlAuthority(false, true);
            ShowWarningS4(5);
            baselineS4.On_BaselineS4_5_Start();
        }

        public void Drone_accidents_5(object[] parameters)
        {
            BluePrint("Invoked Drone_accidents_5");
            SetDroneControlAuthority(false, true);
            ShowWarningS4(6);
            baselineS4.On_BaselineS4_6_Start();
        }

        public void Drone_accidents_6(object[] parameters)
        {
            BluePrint("Invoked Drone_accidents_6");
            SetDroneControlAuthority(false, true);
            ShowWarningS4(7);
            baselineS4.On_BaselineS4_7_Start();
        }

        public void Drone_accidents_7(object[] parameters)
        {
            BluePrint("Invoked Drone_accidents_7");
            SetDroneControlAuthority(false, true);
            ShowWarningS4(6);
            baselineS4.On_BaselineS4_6_Start();
        }

        public void Drone_accidents_8(object[] parameters)
        {
            BluePrint("Invoked Drone_accidents_8");
            SetDroneControlAuthority(false, true);
            ShowWarningS4(7);
            baselineS4.On_BaselineS4_7_Start();
        }

        #endregion
    }
}