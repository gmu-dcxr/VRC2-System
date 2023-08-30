using UnityEngine;
using UnityEngine.UIElements;

namespace VRC2.Extention
{
    public class BackupableTransform
    {
        public Vector3 position { get; set; }

        public Quaternion rotation { get; set; }

        public BackupableTransform()
        {
            position = Vector3.zero;
            rotation = Quaternion.identity;
        }

        public void Restore(ref GameObject go)
        {
            go.transform.position = this.position;
            go.transform.rotation = this.rotation;
        }

        public static BackupableTransform Clone(GameObject go)
        {
            var t = go.transform;
            BackupableTransform bt = new BackupableTransform();
            bt.position = t.position;
            bt.rotation = t.rotation;

            return bt;
        }
    }
}