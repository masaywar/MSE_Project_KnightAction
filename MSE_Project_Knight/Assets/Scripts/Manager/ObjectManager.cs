using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    [SerializeField]
    private string[] prefabPath;
    [SerializeField]
    private int poolingNum;


    private Dictionary<string, List<ScriptObject>> m_allObjectDict;
    private Dictionary<string, List<ScriptObject>> m_despawnedObjDict;
    private Dictionary<string, List<ScriptObject>> m_spawnedObjDict;

    public Dictionary<string, List<ScriptObject>> allObjectDict
    {
        get
        {
            if (m_allObjectDict == null)
            {
                m_allObjectDict = new Dictionary<string, List<ScriptObject>>();

            }
            return m_allObjectDict;
        }
    }
    public Dictionary<string, List<ScriptObject>> spawnedObjDict
    {
        get
        {
            if (m_spawnedObjDict == null)
            {
                m_spawnedObjDict = new Dictionary<string, List<ScriptObject>>();
            }

            return m_spawnedObjDict;
        }
    }
    public Dictionary<string, List<ScriptObject>> despawnedObjDict
    {
        get 
        {
            if (m_despawnedObjDict == null)
            {
                m_despawnedObjDict = new Dictionary<string, List<ScriptObject>>();
            }
            return m_despawnedObjDict;
        }
    }

    private void Awake()
    {
        int idx = 0;
        foreach (var path in prefabPath)
        {
            string newPath = path.Replace(@"\", @"/");

            try
            {
                string[] files = Directory.GetFiles(newPath, $"*.prefab");

                foreach (var file in files)
                {
                    string newFile = file.Replace(@"\", @"/").Replace($"Assets/Resources/", "").Replace($".prefab", "");
                    string objName = newFile.Split('/').GetTop();

                    print(newFile);
                    print(objName);

                    spawnedObjDict[objName] = new List<ScriptObject>();
                    despawnedObjDict[objName] = new List<ScriptObject>();

                    GameObject go = new GameObject(objName);
                    go.transform.SetParent(this.transform);

                    for (int k = 0; k < poolingNum; k++) 
                    {
                        ScriptObject obj = Instantiate(Resources.Load<ScriptObject>(newFile));
                        obj.prefabName = objName;
                        obj.transform.SetParent(go.transform);
                        obj.gameObject.SetActive(false);
                    }
                }
            }

            catch
            {
                Debug.Log("Wrong Directory Path!");
            }

            idx += 1;
        }
    }

    public T Find<T>(ScriptObject obj) where T : ScriptObject
    {
        string name = obj.prefabName;
        if (!m_allObjectDict.TryGetValue(name, out var value))
        {
            foreach (var e in value)
            {
                if (e.Equals(obj))
                {
                    return (T)e;
                }
            }
        }

        Debug.Log(string.Format("{0} is not found", obj.name));
        return default(T);
    }

    public T FindByName<T>(string name, GameObject go) where T:ScriptObject
    {
        if (!m_allObjectDict.TryGetValue(name, out var value))
        {
            foreach (var e in value)
            {
                if (e.Equals(go))
                {
                    return (T)e;
                }
            }
        }
        return default(T);
    }

    public T Spawn<T>(string type, Vector3 position, Quaternion rotation, Transform parent) where T:ScriptObject
    {
        if (despawnedObjDict.TryGetValue(type, out var value))
        {
            if (value.Count == 0)
                return default(T);
        }

        T go = (T)value.Top();

        value.Remove(go);
        spawnedObjDict[type].Add(go);

        go.transform.position = position;
        go.transform.rotation = rotation;
        go.transform.SetParent(parent);
        go.gameObject.SetActive(true);

        return go;
    }

    public T Spawn<T>(string type) where T : ScriptObject
    {
        return Spawn<T>(type, Vector3.zero, Quaternion.identity, null);
    }

    public T Spawn<T>(string type, Transform parent) where T : ScriptObject
    {
        return Spawn<T>(type, Vector3.zero, Quaternion.identity, parent);
    }
    public T Spawn<T>(string type, Vector3 position) where T : ScriptObject
    {
        return Spawn<T>(type, position, Quaternion.identity, null);
    }

    public T Spawn<T>(string type, Vector3 position, Transform parent) where T : ScriptObject
    {
        return Spawn<T>(type, position, Quaternion.identity, parent);
    }

    public T Spawn<T>(string type, Vector3 position, Quaternion rotation) where T : ScriptObject
    {
        return Spawn<T>(type, position, rotation, null);
    }

    public void Despwan<T>(ScriptObject obj) where T : ScriptObject
    {
        string name = obj.prefabName;
        if (spawnedObjDict.TryGetValue(name, out var value))
        {
            value.Remove(obj);
            despawnedObjDict[name].Add(obj);
        }
    }
}
