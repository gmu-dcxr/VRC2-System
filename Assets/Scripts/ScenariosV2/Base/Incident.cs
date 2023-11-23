using UnityEngine;

namespace VRC2.ScenariosV2.Base
{
    public class Incident: MonoBehaviour
    {
        #region Attributes

        private int _id;
        private string _name;
        private string _desc;
        private string _warning;
        private string _wfile; // warning sound file
        private int _stime;
        private int _duration;
        private int _endtime;

        #endregion

        #region Methods

        public virtual void InitializeIncident()
        {
            
        }

        public virtual void RunIncident()
        {
            
        }

        #endregion
    }
}