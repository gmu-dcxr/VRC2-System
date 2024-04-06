using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC2.Utility;
using VRC2.ScenariosV2.Base;
using YamlDotNet.Serialization;
using Incident = VRC2.ScenariosV2.Base.Incident;

namespace VRC2.ScenariosV2.Vehicle
{
    public class Base : MonoBehaviour
    {
        #region Attributes

        private string name;
        private string desc;

        private List<string> variables;

        // private string @gameObject;
        private YamlParser.Incidents incidents;

        #endregion

        #region Parsed Attributes

        private List<Incident> _normals;
        private List<Incident> _accidents;

        // because of the execution order, sometime it will be called before it's done
        private List<Incident> normals
        {
            get
            {
                if (!parsed)
                {
                    ParseYamlFile();
                }

                return this._normals;
            }
        }

        private List<Incident> accidents
        {
            get
            {
                if (!parsed)
                {
                    ParseYamlFile();
                }

                return this._accidents;
            }
        }

        #endregion

        // warning controller
        private WarningController _warningController;

        public WarningController warningController
        {
            get
            {
                if (_warningController == null)
                {
                    _warningController = GameObject.FindFirstObjectByType<WarningController>();
                }

                return _warningController;
            }
        }

        public virtual string ClsName
        {
            get => GetType().Name;
        }

        public string DefaultYamlFile
        {
            get => $"{ClsName}.yml";
        }

        private bool parsed = false;

        #region Adaption for invoke with parameters

        public bool showWaring(object[] parameters)
        {
            return (bool)parameters[0];
        }

        #endregion

        #region Methods

        public virtual void ParseYamlFile()
        {
            // ensure it's parsed once.
            if (parsed) return;

            ParseYamlFile(DefaultYamlFile);
            parsed = true;
        }

        public virtual void ParseYamlFile(string name)
        {
            var path = Helper.GetConfigureFile(Application.dataPath, name);
            Debug.LogWarning($"ParseYamlFile: {path}");

            var text = System.IO.File.ReadAllText(path);
            print(text);

            var deser = new DeserializerBuilder().Build();
            var v = deser.Deserialize<YamlParser.Vehicle>(text);

            ParseYamlVehicle(v);
        }

        public virtual void ParseYamlVehicle(YamlParser.Vehicle v)
        {
            this.name = v.name;
            this.desc = v.desc;
            this.variables = v.variables;

            // parse normals
            if (v.incidents.normals != null)
            {
                this._normals = new List<Incident>();
                var count = v.incidents.normals.Count;
                for (var i = 0; i < count; i++)
                {
                    var inci = v.incidents.normals[i];
                    // Incident incident = new Incident(); --- this won't work
                    Incident incident = gameObject.AddComponent<Incident>();
                    incident.ParseYamlIncident(inci, ConditionType.Normal);
                    // TODO: refactor
                    // update callback
                    incident.callback = GetVehicleCallbackName(true, inci.id);
                    // update vehicle
                    incident.vehicle = this;

                    this._normals.Add(incident);
                }
            }

            // parse accidents
            if (v.incidents.accidents != null)
            {
                this._accidents = new List<Incident>();
                var count = v.incidents.accidents.Count;
                for (var i = 0; i < count; i++)
                {
                    var inci = v.incidents.accidents[i];
                    Incident incident = gameObject.AddComponent<Incident>();
                    incident.ParseYamlIncident(inci, ConditionType.Accident);

                    // update callback
                    incident.callback = GetVehicleCallbackName(false, inci.id);
                    // update vehicle
                    incident.vehicle = this;

                    this._accidents.Add(incident);
                }
            }
        }



        #endregion

        #region Helper functions

        public Incident GetIncident(int idx, bool normal)
        {
            var li = this.normals;
            if (!normal)
            {
                li = this.accidents;
            }

            if (li == null) return null;

            var count = li.Count;
            for (var i = 0; i < count; i++)
            {
                var inci = li[i];
                if (inci.id == idx)
                {
                    return inci;
                }
            }

            Debug.LogError($"Can not find Incident#{idx}");
            return null;
        }

        private List<Incident> GetTxIncidents(string tx, bool normal)
        {
            if (!this.variables.Contains(tx))
            {
                Debug.LogError($"{tx} is not in {this.variables}.");
                return null;
            }

            var li = this.normals;
            if (!normal)
            {
                li = this.accidents;
            }

            if (li == null) return null;

            List<Incident> result = new List<Incident>();

            var count = li.Count;
            for (var i = 0; i < count; i++)
            {
                var inci = li[i];
                if (inci.time.Contains(tx))
                {
                    result.Add(inci);
                }
            }

            return result;
        }

        public List<Incident> GetNormalIncident(string tx)
        {
            return GetTxIncidents(tx, true);
        }

        public List<Incident> GetAccidentIncident(string tx)
        {
            return GetTxIncidents(tx, false);
        }

        public Incident GetNormalIncident(int idx)
        {
            return GetIncident(idx, true);
        }

        public Incident GetAccidentIncident(int idx)
        {
            return GetIncident(idx, false);
        }

        #endregion

        #region Implementation Checker

        private string GetVehicleCallbackName(bool normal, int idx)
        {
            string name = GetType().Name;
            if (normal)
            {
                name += "_normals_";
            }
            else
            {
                name += "_accidents_";
            }

            name += $"{idx}";

            return name;
        }

        public virtual void CheckIncidentsImplementation()
        {
            bool pass = true;

            var ClsName = GetType().Name;

            var @namespace = "VRC2.ScenariosV2.Vehicle";
            var myClassType = Type.GetType($"{@namespace}.{ClsName}");

            if (normals != null)
            {
                foreach (var inc in normals)
                {
                    var name = GetVehicleCallbackName(true, inc.id);
                    if (myClassType.GetMethod(name) == null)
                    {
                        pass = false;
                        Debug.LogError($"[{ClsName}] missing method: {name}");
                    }
                }

            }

            if (accidents != null)
            {
                foreach (var inc in accidents)
                {
                    var name = GetVehicleCallbackName(false, inc.id);
                    if (myClassType.GetMethod(name) == null)
                    {
                        pass = false;
                        Debug.LogError($"[{ClsName}] missing method: {name}");
                    }
                }
            }


            Debug.LogWarning($"{ClsName} Check Vehicle Callbacks Result: {pass}");
        }

        #endregion

        public void Start()
        {
            ParseYamlFile();
            CheckIncidentsImplementation();
        }

        #region Print log

        public void ColorPrint(string color, string msg)
        {
            // Debug.Log("<color=red>This is a red message.</color>");
            // Debug.Log("<color=blue>This is a blue message.</color>");
            // Debug.Log("<color=#00FF00>This is a green message using hex code.</color>");
            // Debug.Log("<color=yellow>This is a yellow message.</color>");
            print($"<color={color}>{msg}</color>");
        }

        public void BluePrint(string msg)
        {
            ColorPrint("blue", msg);
        }

        public void RedPrint(string msg)
        {
            ColorPrint("red", msg);
        }

        public void GreenPrint(string msg)
        {
            ColorPrint("green", msg);
        }

        public void YellowPrint(string msg)
        {
            ColorPrint("yellow", msg);
        }


        #endregion

        #region Warning only for irrelevant

        #region Audio filename

        public string GetAudioFileName(int key)
        {
            return $"{ClsName}_{key}.wav";
        }

        public void PlayAudioOnly(int key)
        {
            var filename = GetAudioFileName(key);
            BluePrint(filename);
            warningController.PlayAudioClip(filename, null);
        }

        #endregion



        #endregion

        /// <summary>
        /// Show bare warning without calling any scenario
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="msg"></param>
        /// <param name="delay"></param>
        public void BareWarning(int idx, string msg, float? delay)
        {
            // TODO: test
            warningController.Show("Warning", ClsName, idx, msg, delay);
        }
    }
}