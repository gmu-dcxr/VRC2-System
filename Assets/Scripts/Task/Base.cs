using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUITable;
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


        [Header("Table")] public Table table;

        // data to show in the table
        [HideInInspector] public List<TableRow> rows;


        #region Private variables

        private TaskData task;

        private readonly string ruleSeparator = " -> ";


        public virtual string GetFilename()
        {
            return filename;
        }

        #endregion


        private void Start()
        {
            ParseYmlFile();

            FormatInfoData(false);
            UpdateTable();
        }

        private void UpdateTable()
        {
            // hack it
            table.targetCollection.target = gameObject;
            table.targetCollection.componentName = GetType().Name; // class name, e.g., Training
            table.targetCollection.memberName = "rows"; // this is to save the data

            // add columns, corresponding to `TableRow` class
            var c1 = new TableColumnInfo();
            c1.columnTitle = "Segment";
            c1.fieldName = "segment";
            c1.autoWidth = true;

            var c2 = new TableColumnInfo();
            c2.columnTitle = "Spec";
            c2.fieldName = "spec";
            c2.autoWidth = true;

            table.columns.Add(c1);
            table.columns.Add(c2);

            // initialize it
            table.Initialize();
        }

        public void ParseYmlFile()
        {
            var path = Helper.GetConfigureFile(Application.dataPath, GetFilename());

            task = Helper.ParseYamlFile<TaskData>(path);
            print(task);
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

        public (List<string>, List<InfoData>) GetP1Info()
        {
            var keys = task.P1;
            var data = GetInfo(keys);
            return (keys, data);
        }

        public (List<string>, List<InfoData>) GetP2Info()
        {
            var keys = task.P2;
            var data = GetInfo(keys);
            return (keys, data);
        }

        public Texture2D LoadTexture()
        {
            var folder = task.folder;
            var filename = task.image;
            return GlobalConstants.LoadTexture(folder, filename);
        }

        public string GetConstructRule()
        {
            var rule = task.rule;
            return string.Join(ruleSeparator, rule);
        }

        private List<InfoData> GetInfo(List<string> keys)
        {
            List<InfoData> result = new List<InfoData>();
            var count = task.info.Count;
            var keysCount = keys.Count;
            for (var i = 0; i < count; i++)
            {
                var info = task.info[i];
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
    }
}