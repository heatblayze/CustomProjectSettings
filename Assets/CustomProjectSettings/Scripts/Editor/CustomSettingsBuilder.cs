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
            //Force save the settings before build
            ProjectSave.OnWillSaveAssets(null);

            string rootFolder = Directory.GetParent(Application.dataPath) + "/CustomProjectSettings/";
            string targetFolder = Application.dataPath + "/Resources/CustomSettings/";

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
            string folder = Application.dataPath + "/Resources/CustomSettings/";
            //just straight up delete that fam
            Directory.Delete(folder, true);
        }

        void Copy(string sourceDir, string targetDir)
        {
            var files = Directory.GetFiles(sourceDir);
            foreach (var file in files)
            {
                string ext = Path.GetExtension(file);
                if (ext != ".json")
                    continue;
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));
                AssetDatabase.ImportAsset("Assets/Resources/CustomSettings/" + Path.GetFileName(file), ImportAssetOptions.ForceUpdate);
            }
        }
    }
}