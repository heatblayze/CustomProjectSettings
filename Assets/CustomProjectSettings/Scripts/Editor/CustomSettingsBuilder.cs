using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;

namespace CustomProjectSettings
{
    public class CustomSettingsBuilder : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            string rootFolder = Directory.GetParent(Application.dataPath) + "/CustomProjectSettings/";
            string targetFolder = Application.dataPath + "/Resources/Settings/";

            //NO BUENO EXISTING FOLDER
            if (Directory.Exists(targetFolder))
                Directory.Delete(targetFolder, true);

            Directory.CreateDirectory(targetFolder);

            //Try to copy all files from target directory
            if (Directory.Exists(rootFolder))
                Copy(rootFolder, targetFolder);
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            string folder = Application.dataPath + "/Resources/Settings/";
            //just straight up delete that fam
            Directory.Delete(folder, true);
        }

        void Copy(string sourceDir, string targetDir)
        {
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                if (Path.GetExtension(file) != "json")
                    continue;
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));
                AssetDatabase.ImportAsset("Assets/Resources/Settings/" + Path.GetFileName(file), ImportAssetOptions.ForceUpdate);
            }
        }
    }
}