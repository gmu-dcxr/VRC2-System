using System;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Unity.VisualScripting;
using UnityEngine;
using VRC2.Utility;
using VRC2.Scenarios;
using VRC2.ScenariosV2.Tool;
using Object = UnityEngine.Object;

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
        // [ReadOnly] public int scenarioIdx; // index defined in the scenario
        // [ReadOnly] public string scenarioTime; // time defined in the scenario
        
        // warning text
        public string warning => rawWarning;

        #region Index time dictionary

        private Dictionary<int, string> idxTimeDict; // one incident maybe called several times

        public void AddEntry(int idx, string time)
        {
            if (idxTimeDict == null)
            {
                idxTimeDict = new Dictionary<int, string>();
            }
            idxTimeDict.Add(idx, time);
        }

        public (int, string) GetEntry(int idx)
        {
            return (idx, idxTimeDict[idx]);
        }


        #endregion

        public int startTime
        {
            get => _stime;
        }

        #endregion
        
        #region Reference in Scenario V2

        // default is true
        [HideInInspector] public bool showWarning = true;
        #endregion

        #region Methods

        private ConditionType ParseCondition(string condition, ConditionType t)
        {
            // return the default value
            if (condition == null) return t;
            
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

        public virtual void ParseYamlIncident(YamlParser.Incident incident, ConditionType t)
        {
            _incident = incident;

            _id = incident.id;
            // no name
            _conditionType = ParseCondition(incident.condition, t);
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
                // invoke with parameters
                // refer: https://stackoverflow.com/questions/61855500/targetparametercountexception-c-sharp
                object[] parameters = new object[] {new object[]{ showWarning}};
                method.Invoke(vehicle, parameters);
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