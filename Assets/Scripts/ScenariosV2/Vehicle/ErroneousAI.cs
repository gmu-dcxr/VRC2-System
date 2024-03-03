using System;
using UnityEngine;
using VRC2.Scenarios.ScenarioFactory;
using VRC2.ScenariosV2.Tool;

namespace VRC2.ScenariosV2.Vehicle
{
    public class ErroneousAI : Base
    {
        // private BaselineS1 _baselineS1;
        //
        // public BaselineS1 baselineS1
        // {
        //     get
        //     {
        //         if (_baselineS1 == null)
        //         {
        //             _baselineS1 = FindObjectOfType<BaselineS1>();
        //         }
        //
        //         return _baselineS1;
        //     }
        // }

        private Crane _crane;

        public Crane crane
        {
            get
            {
                if (_crane == null)
                {
                    _crane = FindObjectOfType<Crane>();
                }

                return _crane;
            }
        }

        #region Callbacks

        public void ErroneousAI_accidents_1(object[] parameters)
        {
            BluePrint("Invoked ErroneousAI_accidents_1");
            // Crane_accident_3, but No warnings
            crane.Crane_accidents_3(new object[] { false });
        }


        public void ErroneousAI_accidents_2(object[] parameters)
        {
            BluePrint("Invoked ErroneousAI_accidents_2");
            // Nothing happens but a warning.
            // Warning: A cargo is going to pass overhead.
            // TODO: test
            PlayAudioOnly(2);
        }


        public void ErroneousAI_accidents_3(object[] parameters)
        {
            BluePrint("Invoked ErroneousAI_accidents_3");
            // A hook (without a load) is passing overhead in the opposite direction. Crane_accident_4.
            crane.Crane_accidents_4(null);
        }


        public void ErroneousAI_accidents_4(object[] parameters)
        {
            BluePrint("Invoked ErroneousAI_accidents_4");
            // A load is being hoisted and is going to pass above players. Crane_accident_1, but No warnings.
            crane.Crane_accidents_1(new object[] { false });
        }

        #endregion
    }
}