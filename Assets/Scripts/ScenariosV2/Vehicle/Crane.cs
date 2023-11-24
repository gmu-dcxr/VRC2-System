using System;
using UnityEngine;

namespace VRC2.ScenariosV2.Vehicle
{
    public class Crane : Base
    {
        #region Callbacks

        public void Crane_normals_1()
        {
            print("Invoked");
        }

        public void Crane_normals_2()
        {
            
        }

        public void Crane_accidents_1()
        {
            print("Invoked Crane_accidents_1");
        }

        public void Crane_accidents_2()
        {
            print("Invoked Crane_accidents_2");
        }

        #endregion
    }
}