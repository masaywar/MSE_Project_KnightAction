using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PatternGenerator), true)]
public class PatternGUIInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PatternGenerator patternGenerator = (PatternGenerator)target;

        if (GUILayout.Button("Generate Mesh"))
            patternGenerator.Generate();
    }
}
