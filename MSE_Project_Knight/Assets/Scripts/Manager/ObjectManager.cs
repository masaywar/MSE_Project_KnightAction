using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    private Dictionary<string, Dictionary<GameObject, ScriptObject>> m_allObjectDictionary;


    public Dictionary<string, Dictionary<GameObject, ScriptObject>> allObjectDictionary 
    {
        get
        {
            if (m_allObjectDictionary == null)
                m_allObjectDictionary = new Dictionary<string, Dictionary<GameObject, ScriptObject>>();

            return m_allObjectDictionary;
        }   
    }

    public T FindByTag<T>(string tag, GameObject go) where T : ScriptObject
    {
        if (!allObjectDictionary.ContainsKey(tag))
            return null;

        if (!allObjectDictionary[tag].ContainsKey(go))
            return null;

        return (T) allObjectDictionary[tag][go];
    }
}
