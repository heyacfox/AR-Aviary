using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class jenkinsBuild {

    static void buildAndroid()
    {
        List<string> scenes = getScenesForBuildGame();
        string pathToDeploy = $"E:/Deployments/android-devbuild.apk";
        BuildPlayerOptions bpo = new BuildPlayerOptions();
        bpo.scenes = scenes.ToArray();
        bpo.target = BuildTarget.Android;
        //bpo.options = BuildOptions.Development;
        bpo.options = BuildOptions.None;
        bpo.locationPathName = pathToDeploy;
        BuildPipeline.BuildPlayer(bpo);

    }

    static List<string> getScenesInFolder(string folder)
    {
        List<string> scenesFolder = new List<string>();
        foreach (string file in System.IO.Directory.GetFiles("Assets/" + folder))
        {
            if (file.EndsWith(".unity"))
            {
                scenesFolder.Add(file);
            }
        }
        return scenesFolder;
    }

    static List<string> getScenesForBuildGame()
    {
        List<string> scenes = new List<string>();
        scenes.AddRange(getScenesInFolder("Scenes/"));
        return scenes;
    }
}
