using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorScript : EditorWindow
{
    [MenuItem("Custom/BuildSymbol")]
    private static void Build()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "UNITY_ANDROID");
    }

    [MenuItem("Custom/TestSymbol")]
    private static void Test()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "TEST");
    }
}
