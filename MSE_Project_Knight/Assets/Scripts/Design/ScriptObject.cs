using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptObject : MonoBehaviour
{
    public Dictionary<string, Dictionary<GameObject, ScriptObject>> m_cachedAllObjectDict;
    public RectTransform rectTransform;

    private void Awake()
    {
        m_cachedAllObjectDict = ObjectManager.Instance.allObjectDictionary;
        rectTransform = GetComponent<RectTransform>();
        string tag = gameObject.tag;

        if (m_cachedAllObjectDict.ContainsKey(tag))
        {
            if (!m_cachedAllObjectDict[tag].ContainsKey(gameObject))
            {
                m_cachedAllObjectDict[tag].Add(gameObject, this);
            }
        }

        else 
        {
            m_cachedAllObjectDict[tag] = new Dictionary<GameObject, ScriptObject>();
            m_cachedAllObjectDict[tag].Add(gameObject, this);
        }
    }

    public T FindByTag<T>(string tag, T obj) where T : ScriptObject
    {
        if (!m_cachedAllObjectDict.ContainsKey(tag)) 
        {
            return null;
        }

        if (!m_cachedAllObjectDict[tag].ContainsKey(obj.gameObject)) 
        {
            return null;
        }

        Dictionary<GameObject, ScriptObject> tempDict = m_cachedAllObjectDict[tag];
        return (T)tempDict[gameObject];
    }

    public T FindByObject<T>(T obj) where T : ScriptObject
    {
        GameObject findGo = obj.gameObject;
        string tag = findGo.tag;

        if (!m_cachedAllObjectDict.ContainsKey(tag))
        {
            return null;
        }

        if (!m_cachedAllObjectDict[tag].ContainsKey(findGo))
        {
            return null;
        }

        Dictionary<GameObject, ScriptObject> tempDict = m_cachedAllObjectDict[tag];
        return (T)tempDict[findGo];
    }
}
