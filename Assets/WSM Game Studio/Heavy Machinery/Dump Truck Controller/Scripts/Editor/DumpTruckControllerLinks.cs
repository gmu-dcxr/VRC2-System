using System.IO;
using UnityEditor;
using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    public class DumpTruckControllerLinks
    {
        [MenuItem("WSM Game Studio/Heavy Machinery/Dump Truck Controller/Documentation")]
        static void OpenDocumentation()
        {
            string documentationFolder = "WSM Game Studio/Heavy Machinery/Dump Truck Controller/_Documentation/Dump Truck Controller v1.0.pdf";
            DirectoryInfo info = new DirectoryInfo(Application.dataPath);
            string documentationPath = Path.Combine(info.Name, documentationFolder);
            Application.OpenURL(documentationPath);
        }

        [MenuItem("WSM Game Studio/Heavy Machinery/Dump Truck Controller/Write a Review")]
        static void Review()
        {
            Application.OpenURL("https://assetstore.unity.com/packages/slug/174975");
        }
    } 
}
