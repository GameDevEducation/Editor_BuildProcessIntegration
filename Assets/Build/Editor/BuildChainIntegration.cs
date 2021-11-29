using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System;
using System.Diagnostics;

public class BuildChainIntegration_PreProcess : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        // We can abandon/stop a build by throwing BuildFailedException
        //throw new BuildFailedException("Preprocess checks failed!");

        UpdateBuildMetadata();
    }

    void UpdateBuildMetadata()
    {
        string[] foundGUIDS = AssetDatabase.FindAssets($"t:{typeof(BuildMetadata)}");

        // did we find multiple metadata assets?
        if (foundGUIDS.Length > 1)
            UnityEngine.Debug.LogError("Found multiple BuildMetadata assets. Only the first will be used.");

        // load or create the metadata
        BuildMetadata metadata;
        if (foundGUIDS.Length == 0)
        {
            metadata = ScriptableObject.CreateInstance<BuildMetadata>();

            string assetPath = System.IO.Path.Combine(System.IO.Path.Combine("Assets", "Build"),
                                                      "BuildMetadata.asset");
            AssetDatabase.CreateAsset(metadata, assetPath);
        }
        else
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(foundGUIDS[0]);
            metadata = AssetDatabase.LoadAssetAtPath<BuildMetadata>(assetPath);
        }

        metadata.Version        = Application.version;
        metadata.BuildNumber    = PlayerSettings.iOS.buildNumber;
        metadata.BuildTime      = DateTime.Now.ToString("dd-MMM-yyyy @ HH:mm:ss");

        GetRepoInformation(metadata);

        EditorUtility.SetDirty(metadata);
        AssetDatabase.SaveAssetIfDirty(metadata);
    }

    void GetRepoInformation(BuildMetadata metadata)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo("git");
        startInfo.UseShellExecute = false;
        startInfo.WorkingDirectory = Application.dataPath;
        startInfo.RedirectStandardInput = true;
        startInfo.RedirectStandardOutput = true;

        // retrieve the branch name
        startInfo.Arguments = "branch --show-current";
        Process process = new Process();
        process.StartInfo = startInfo;
        process.Start();

        string line = process.StandardOutput.ReadLine();
        metadata.Branch = string.IsNullOrWhiteSpace(line) ? "Unknown" : line;

        process.WaitForExit();

        // retrieve the last commit hash
        startInfo.Arguments = "rev-list --max-count=1 HEAD";
        process = new Process();
        process.StartInfo = startInfo;
        process.Start();

        line = process.StandardOutput.ReadLine();
        metadata.CommitHash = string.IsNullOrWhiteSpace(line) ? "Unknown" : line;

        process.WaitForExit();
    }
}

public class BuildChainIntegration_PostProcess : IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPostprocessBuild(BuildReport report)
    {
    }
}

