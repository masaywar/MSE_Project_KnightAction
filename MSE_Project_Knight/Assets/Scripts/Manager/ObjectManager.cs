using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    private Dictionary<string, Dictionary<GameObject, Object>> m_allObjectDictionary;
    public Dictionary<string, Dictionary<GameObject, Object>> allObjectDictionary 
    {
        get
        {
            if (m_allObjectDictionary == null)
                m_allObjectDictionary = new Dictionary<string, Dictionary<GameObject, Object>>();

            return m_allObjectDictionary;
        }   
    }
}
