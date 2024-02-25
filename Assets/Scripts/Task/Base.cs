using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityUITable;
using VRC2.Events;
using VRC2.Utility;
using TaskData = VRC2.Task.YamlParser.Task;
using InfoData = VRC2.Task.YamlParser.Info;
using TableRow = VRC2.Task.YamlParser.TableRow;
using Table = UnityUITable.Table;

namespace VRC2.Task
{
    public class Base : MonoBehaviour
    {
        [Header("Filename")] [Tooltip("Filename under Assets/Conf, e.g. Task/Training.yml")]
        public string filename;

        // TODO: refactor based on the task name
        [Space(30)] [Header("Instruction Picture")]
        public string folder;

        public string sheetRule;
        public string sheetPipe;

        #region Instruction related components

        // find them during the runtime
        private InstructionSheetGrabbingCallback _sheetCallback;

        public InstructionSheetGrabbingCallback sheetCallback
        {
            get
            {
                if (_sheetCallback == null)
                {
                    _sheetCallback = FindObjectOfType<InstructionSheetGrabbingCallback>();
                }

                return _sheetCallback;
            }
        }


        public Table table
        {
            get => sheetCallback.srcTable;
        }

        public Text constructionRule
        {
            get => sheetCallback.srcRule;
        }

        public ImageAsTexture imageAsTexture
        {
            get => sheetCallback.srcIAT;
        }

        #endregion

        private string srcText;

        // data to show in the table
        [HideInInspector] public List<TableRow> rows;


        #region Private variables

        private TaskData task;
        private TaskData taskChange = null; // task for changed order

        private TaskData curTask = null; // current task

        private readonly string ruleSeparator = " -> ";


        public virtual string GetFilename()
        {
            return filename;
        }

        public virtual string GetChangeOrderFilename()
        {
            var f = GetFilename();
            return f.Split('.')[0] + "C.yml";
        }

        #endregion


        private void Start()
        {
            srcText = constructionRule.text;

            ParseYmlFile();
            // parse change order task
            ParseChangeOrder();

            // UpdateTableRule(false);
        }

        public void UpdateTable(ref Table t)
        {
            // hack it
            t.targetCollection.target = gameObject;
            t.targetCollection.componentName = GetType().Name; // class name, e.g., Training
            t.targetCollection.memberName = "rows"; // this is to save the data

            // BUG: It's better to set up columns in Inspector window

            // // add columns, corresponding to `TableRow` class
            // var c1 = new TableColumnInfo();
            // c1.columnTitle = "Segment";
            // c1.fieldName = "segment";
            // c1.autoWidth = true;
            //
            // var c2 = new TableColumnInfo();
            // c2.columnTitle = "Spec";
            // c2.fieldName = "spec";
            // c2.autoWidth = true;
            //
            // // clear all first
            // table.columns.Clear();
            //
            // table.columns.Add(c1);
            // table.columns.Add(c2);

            // initialize it
            t.Initialize();
        }

        public void ParseYmlFile()
        {
            var path = Helper.GetConfigureFile(Application.dataPath, GetFilename());

            task = Helper.ParseYamlFile<TaskData>(path);
            print(task);
            // set current task
            curTask = task;
        }

        public void ParseChangeOrder()
        {
            var path = Helper.GetConfigureFile(Application.dataPath, GetChangeOrderFilename());

            if (!File.Exists(path))
            {
                Debug.LogWarning($"File not found: {path}");
                return;
            }

            taskChange = Helper.ParseYamlFile<TaskData>(path);
            print(taskChange);
        }

        public void FormatInfoData(bool p1)
        {
            List<string> keys;
            List<InfoData> data;
            if (p1)
            {
                (keys, data) = GetP1Info();
            }
            else
            {
                (keys, data) = GetP2Info();
            }

            // format it
            if (rows == null)
            {
                rows = new List<TableRow>();
            }

            rows.Clear();

            var count = data.Count;
            for (var i = 0; i < count; i++)
            {
                var segment = data[i].segment;
                var spec = data[i].FormatSpec();

                rows.Add(new TableRow() { segment = segment, spec = spec });
            }
        }

        public bool P1HasLength()
        {
            return curTask.P1.Contains("length");
        }

        public bool P2HasLength()
        {
            return !P1HasLength();
        }

        public bool P1HasRule()
        {
            return P1HasLength();
        }

        public bool P2HasRule()
        {
            return !P1HasRule();
        }

        public (List<string>, List<InfoData>) GetP1Info()
        {
            var keys = curTask.P1;
            var data = GetInfo(keys);
            return (keys, data);
        }

        public (List<string>, List<InfoData>) GetP2Info()
        {
            var keys = curTask.P2;
            var data = GetInfo(keys);
            return (keys, data);
        }

        public Texture2D LoadTexture()
        {
            var folder = curTask.folder;
            var filename = curTask.image;
            return GlobalConstants.LoadTexture(folder, filename);
        }

        // shown to who has the length information
        public string GetConstructRule()
        {
            var rule = curTask.rule;
            return string.Join(ruleSeparator, rule);
        }

        private List<InfoData> GetInfo(List<string> keys)
        {
            List<InfoData> result = new List<InfoData>();
            var count = curTask.info.Count;
            var keysCount = keys.Count;
            for (var i = 0; i < count; i++)
            {
                var info = curTask.info[i];
                var infoData = new InfoData();
                // update segment
                infoData.segment = info.segment;

                for (var j = 0; j < keysCount; j++)
                {
                    switch (keys[j])
                    {
                        case "color":
                            infoData.color = info.color;
                            break;
                        case "size":
                            infoData.size = info.size;
                            break;
                        case "type":
                            infoData.type = info.type;
                            break;
                        case "length":
                            infoData.length = info.length;
                            break;
                    }
                }

                result.Add(infoData);
            }

            return result;
        }

        public void UpdateRule(bool flag)
        {
            if (flag)
            {
                var text = srcText;
                text += GetConstructRule();
                constructionRule.text = text;
            }
            else
            {
                // set to empty
                constructionRule.text = "";
            }
        }

        public void UpdateSheet(bool rule)
        {
            // sheet rule is deprecated. Always use sheet pipe

            imageAsTexture.UpdateFolderFilename(folder, sheetPipe);
            // if (rule)
            // {
            //     // set sheet_rule picture
            //     imageAsTexture.UpdateFolderFilename(folder, sheetRule);
            // }
            // else
            // {
            //     imageAsTexture.UpdateFolderFilename(folder, sheetPipe);
            // }
        }

        public void UpdateTableRule(bool p1)
        {
            var rule = false;
            if (p1)
            {
                rule = P1HasRule();
            }
            else
            {
                rule = P2HasRule();
            }

            FormatInfoData(p1);
            // update table
            table.Initialize();
            // update rule
            UpdateRule(rule);
            // update sheet
            UpdateSheet(rule);
        }

        #region Debug

        private void OnGUI()
        {
            if (GUI.Button(new Rect(500, 100, 100, 50), "Table"))
            {
                table.Initialize();
            }

            if (GUI.Button(new Rect(500, 150, 100, 50), "Refresh"))
            {
                rows.RemoveAt(rows.Count - 1);
                table.Initialize();
            }

            if (GUI.Button(new Rect(600, 100, 100, 50), "P1"))
            {
                // only refresh data, not columns
                UpdateTableRule(true);
            }

            if (GUI.Button(new Rect(600, 150, 100, 50), "P2"))
            {
                UpdateTableRule(false);
            }

            if (GUI.Button(new Rect(500, 200, 200, 50), "Change"))
            {
                ChangeOrder(true);
            }
        }

        #endregion

        #region Change order

        public void ChangeOrder(bool p1)
        {
            // return is no corresponding changed task
            if (taskChange == null) return;

            // update task
            curTask = taskChange;

            // update sheet and table
            UpdateTableRule(p1);
        }

        #endregion

        #region API

        // update instruction on the table
        public void UpdateTableInstruction(bool p1)
        {
            UpdateTableRule(p1);
        }

        #endregion
    }
}