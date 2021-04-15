using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptObject : MonoBehaviour
{
    public string prefabName;
    public RectTransform rectTransform;
    public Dictionary<string, List<ScriptObject>> m_cachedAllObjectDict;

    private void Awake()
    {
        m_cachedAllObjectDict = ObjectManager.Instance.allObjectDict;
        rectTransform = GetComponent<RectTransform>();

        if (m_cachedAllObjectDict.TryGetValue(prefabName, out var value))
        {
            value.Add(this);
        }
        else 
        {
            m_cachedAllObjectDict[prefabName] = new List<ScriptObject> { this };
        }
    }

    public virtual void Initialize()
    {
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
    }

    private void OnEnable() 
    {
        
    }

    private void OnDisable()
    {
        Initialize();
        ObjectManager.Instance.Despwan<ScriptObject>(this);
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
