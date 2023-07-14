using System.IO;
using UnityEditor;
using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    public class ForkliftControllerLinks
    {
        [MenuItem("WSM Game Studio/Heavy Machinery/Forklift Controller/Documentation")]
        static void OpenDocumentation()
        {
            string documentationFolder = "WSM Game Studio/Heavy Machinery/Forklift Controller/_Documentation/Forklift Controller v1.0.pdf";
            DirectoryInfo info = new DirectoryInfo(Application.dataPath);
            string documentationPath = Path.Combine(info.Name, documentationFolder);
            Application.OpenURL(documentationPath);
        }

        [MenuItem("WSM Game Studio/Heavy Machinery/Forklift Controller/Write a Review")]
        static void Review()
        {
            Application.OpenURL("https://assetstore.unity.com/packages/slug/162826");
        }
    } 
}
