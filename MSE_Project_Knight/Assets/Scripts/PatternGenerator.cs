using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Pattern")]
public class PatternGenerator : ScriptableObject
{
    public string path;

    [SerializeField]
    private List<Pattern> patternContainer = new List<Pattern>();                           

    public void Generate()
    {
        if (patternContainer.Count > 0)
            patternContainer.Clear();

        TextAsset[] textAssets = Resources.LoadAll<TextAsset>(path);

        textAssets.ForEach(textAsset =>{
            Pattern pattern = new Pattern(textAsset.text);
            patternContainer.Add(pattern);
        });
    }

    public Pattern GetPattern(int index)
    {
        if (index < patternContainer.Count)
            return patternContainer[index];

        return null;
    }

    public Pattern[] GetAllPatterns()
    {
        return patternContainer.ToArray();
    }
}
