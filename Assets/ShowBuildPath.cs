using UnityEditor;
using UnityEngine;

public static class ShowBuildPath
{
    [MenuItem("Tools/Show Build Path")]
    static void Show()
    {
        Debug.Log(EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget));
    }
}