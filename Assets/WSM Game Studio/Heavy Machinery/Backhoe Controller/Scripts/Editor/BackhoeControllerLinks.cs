using System.IO;
using UnityEditor;
using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    public class BackhoeControllerLinks
    {
        [MenuItem("WSM Game Studio/Heavy Machinery/Backhoe Controller/Documentation")]
        static void OpenDocumentation()
        {
            string documentationFolder = "WSM Game Studio/Heavy Machinery/Backhoe Controller/_Documentation/Backhoe Controller v1.0.pdf";
            DirectoryInfo info = new DirectoryInfo(Application.dataPath);
            string documentationPath = Path.Combine(info.Name, documentationFolder);
            Application.OpenURL(documentationPath);
        }

        [MenuItem("WSM Game Studio/Heavy Machinery/Backhoe Controller/Write a Review")]
        static void Review()
        {
            Application.OpenURL("https://assetstore.unity.com/packages/slug/170831");
        }
    } 
}
