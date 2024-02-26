using System.Collections.Generic;
using JetBrains.Annotations;

namespace VRC2.SAGAT
{
    public class YamlParser
    {
        #region SAGAT

        public class Question
        {
            public int id { get; set; }
            public string label { get; set; }
            public int level { get; set; }
            public string question { get; set; }

            [CanBeNull] public List<string> options { get; set; }

            public static Question None()
            {
                var q = new Question();
                q.id = -1;
                return q;
            }

            public bool IsNone()
            {
                return this.id == -1;
            }
        }

        public class SAGAT
        {
            public string name { get; set; }
            public string desc { get; set; }
            public List<Question> questions { get; set; }

            public Question Get(int idx)
            {
                if (questions == null || idx >= questions.Count || idx < 0)
                {
                    return Question.None();
                }

                return questions[idx];
            }

            public int Count()
            {
                return this.questions.Count;
            }
        }

        #endregion
    }
}