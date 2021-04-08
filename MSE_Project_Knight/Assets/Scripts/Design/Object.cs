using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public Dictionary<string, Dictionary<GameObject, Object>> m_cachedAllObjectDict;

    private void Awake()
    {
        m_cachedAllObjectDict = ObjectManager.Instance.allObjectDictionary;
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
            m_cachedAllObjectDict[tag] = new Dictionary<GameObject, Object>();
            m_cachedAllObjectDict[tag].Add(gameObject, this);
        }
    }

    public T FindByTag<T>(string tag, T obj) where T : Object
    {
        if (!m_cachedAllObjectDict.ContainsKey(tag)) 
        {
            return null;
        }

        if (!m_cachedAllObjectDict[tag].ContainsKey(obj.gameObject)) 
        {
            return null;
        }

        Dictionary<GameObject, Object> tempDict = m_cachedAllObjectDict[tag];
        return (T)tempDict[gameObject];
    }

    public T FindByObject<T>(T obj) where T : Object
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

        Dictionary<GameObject, Object> tempDict = m_cachedAllObjectDict[tag];
        return (T)tempDict[findGo];
    }
}
