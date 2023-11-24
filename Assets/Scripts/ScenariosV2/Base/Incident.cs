using System;
using ExitGames.Client.Photon.StructWrapping;
using Unity.VisualScripting;
using UnityEngine;
using VRC2.Scenarios;
using VRC2.ScenariosV2.Tool;

namespace VRC2.ScenariosV2.Base
{
    [Serializable, Inspectable]
    public class Incident : MonoBehaviour
    {
        #region Attributes

        #region Raw from yaml file

        private YamlParser.Incident _incident;

        private string rawTime
        {
            get => _incident.time;
        }

        private string rawDesc
        {
            get => _incident.desc;
        }

        private string rawWarning
        {
            get => _incident.warning;
        }

        #endregion

        private ConditionType _conditionType;
        private AccidentType _accidentType;

        public int id
        {
            get => _id;
        }

        public string time
        {
            get => rawTime;
        }

        private int _id;
        private string _name;
        private string _desc;
        private string _warning;
        private string _wfile; // warning sound file
        private int _stime;
        private int _duration;
        private int _endtime;

        [HideInInspector] public Vehicle.Base vehicle;
        [ReadOnly] public string callback;

        public int startTime
        {
            get => _stime;
        }

        #endregion

        #region Methods

        private ConditionType ParseCondition(string condition)
        {
            if (condition.Equals(Utils.GetDisplayName<ConditionType>(ConditionType.Accident)))
            {
                return ConditionType.Accident;
            }

            return ConditionType.Normal;
        }

        private AccidentType ParseAccidentType(string accident)
        {
            if (accident == null) return AccidentType.None;

            if (accident.Equals(Utils.GetDisplayName<AccidentType>(AccidentType.Potential)))
            {
                return AccidentType.Potential;
            }

            return AccidentType.NearlyHappen;

        }

        public virtual void ParseYamlIncident(YamlParser.Incident incident)
        {
            _incident = incident;

            _id = incident.id;
            // no name
            _conditionType = ParseCondition(incident.condition);
            _accidentType = ParseAccidentType(incident.type);
        }


        public void RunIncident()
        {
            var @namespace = "VRC2.ScenariosV2.Vehicle";
            var myClassType = Type.GetType($"{@namespace}.{vehicle.ClsName}");
            var method = myClassType.GetMethod(callback);
            // callback
            if (method != null)
            {
                // invoke
                method.Invoke(vehicle, null);
            }
        }

        #endregion

        #region API

        public void UpdateStartTime(string s)
        {
            Helper.ParseTime(s, ref _stime, ref _endtime);
        }

        #endregion
    }
}