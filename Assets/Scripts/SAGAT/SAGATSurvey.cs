using System;
using UnityEngine;
using VRC2.Utility;
using VRC2.Scenarios;
using VRC2.ScenariosV2.Tool;
using YamlDotNet.Serialization;

namespace VRC2.SAGAT
{
    public class SAGATSurvey : MonoBehaviour
    {
        [ReadOnly]public string filename;

        // current questions index
        private int index = 0;

        private YamlParser.SAGAT sagat;

        private void Start()
        {
            // filename = "Environment.yml";
            // ParseYamlFile(filename);
        }

        private void ParseYamlFile(string name)
        {
            print($"Parse SAGAT File: {name}");
            var path = Helper.GetSurveyFile(Application.dataPath, name);
            print(path);

            var text = System.IO.File.ReadAllText(path);
            print(text);

            var deser = new DeserializerBuilder().Build();
            sagat = deser.Deserialize<YamlParser.SAGAT>(text);

            print(sagat);
        }

        #region API

        public void LoadFile(string file)
        {
            this.filename = file;
            ParseYamlFile(this.filename);
        }

        public YamlParser.Question NextQuestion()
        {
            this.index += 1;
            if (this.index >= sagat.Count())
            {
                this.index = sagat.Count() - 1;
            }
            return sagat.Get(this.index);
        }

        public YamlParser.Question PrevQuestion()
        {
            this.index -= 1;
            if (this.index < 0)
            {
                this.index = 0;
            }
            return sagat.Get(this.index);
        }

        public YamlParser.Question First()
        {
            this.index = 0;
            return sagat.Get(0);
        }

        public void ResetQuestion()
        {
            this.index = 0;
        }

        public bool IsLast()
        {
            return this.index == sagat.Count() - 1;
        }

        #endregion
    }
}