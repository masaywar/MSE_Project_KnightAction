using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    public List<Transform> childrenTransform = new List<Transform>();

    [System.Serializable]
    private class PoolPrefab
    {
        public string path;
        public int poolNum;

        public PoolPrefab(string path, int poolNum) 
        {
            this.path = path;
            this.poolNum = poolNum;
        }
    }
    private bool isLoaded = false;

    [SerializeField]
    private List<PoolPrefab> prefabPath;

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
    
    public void Initialize()
    {
        prefabPath = new List<PoolPrefab> {
            new PoolPrefab("Prefab/PlayerSprite/knightSprite", 1),
            new PoolPrefab("Prefab/PlayerSprite/archerSprite", 1),
            new PoolPrefab("Prefab/PlayerSprite/mageSprite", 1),
            new PoolPrefab("Prefab/Player/knight", 1),
            new PoolPrefab("Prefab/Player/archer", 1),
            new PoolPrefab("Prefab/Player/mage", 1),
            new PoolPrefab("Prefab/PlayerIcon/knightIcon", 1),
            new PoolPrefab("Prefab/PlayerIcon/archerIcon", 1),
            new PoolPrefab("Prefab/PlayerIcon/mageIcon", 1),
            new PoolPrefab("Prefab/Enemy/UpDestroyableEnemy", 250),
            new PoolPrefab("Prefab/Enemy/DownDestroyableEnemy", 250),
            new PoolPrefab("Prefab/Enemy/UnDestroyableEnemy", 50)};

        DontDestroyOnLoad(this);
        StartCoroutine(LoadPrefabs());
    }

    public IEnumerator LoadPrefabs()
    {
        foreach (var prefab in prefabPath)
        {
            GameObject parentGo = new GameObject();
            ScriptObject go = Resources.Load<ScriptObject>(prefab.path);

            go.name = prefab.path.Split('/').GetTop();
            go.prefabName = go.name;

            parentGo.name = go.name;

            spawnedObjDict[go.name] = new List<ScriptObject>();
            despawnedObjDict[go.name] = new List<ScriptObject>();

            for (int k = 0; k < prefab.poolNum; k++)
            {
                ScriptObject obj = Instantiate(go);
                InitSpawnedObject(obj, parentGo.transform);
            }

            parentGo.transform.SetParent(transform);
            childrenTransform.Add(parentGo.transform);

            yield return null;
        }

        isLoaded = true;
        yield return null;
    }

    public bool IsLoaded() 
    {
        return isLoaded;
    }

    private void InitSpawnedObject(ScriptObject spawned, Transform parentGo) 
    {
        spawned.transform.SetParent(parentGo);
        Despawn<ScriptObject>(spawned);
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

    public T FindByName<T>(string name, GameObject go) where T : ScriptObject
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

    private void SetTransform(Transform spawnedTransform, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (spawnedTransform == null)
            return;

        spawnedTransform.position = position;
        spawnedTransform.rotation = rotation;

        if (parent != null)
            spawnedTransform.SetParent(parent);
    }

    public T Spawn<T>(string type, Vector3 position, Quaternion rotation, Transform parent) where T : ScriptObject
    {
        if (!despawnedObjDict.TryGetValue(type, out var value))
        {
            return default(T);
        }

        T spawnedObject = (T)value.Top();

        value.Remove(spawnedObject);
        spawnedObjDict[type].Add(spawnedObject);

        SetTransform(spawnedObject.transform, position, rotation, parent);
        spawnedObject.gameObject.SetActive(true);

        return spawnedObject;
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

    public void Despawn<T>(ScriptObject obj) where T : ScriptObject
    {
        string prefabName = obj.prefabName;
        if (spawnedObjDict.TryGetValue(prefabName, out var value))
        {
            value.Remove(obj);
            despawnedObjDict[prefabName].Add(obj);
        }
     
        obj.gameObject.SetActive(false);
    }

    public void DespawnAllWithName<T>(string key) where T : ScriptObject
    {
        if (spawnedObjDict.TryGetValue(key, out var value))
        {
            List<ScriptObject> despawnList = new List<ScriptObject>();

            value.ForEach(obj =>
            {
                despawnList.Add(obj);
            });

            despawnList.ForEach(obj => Despawn<ScriptObject>(obj));
        }
    }

    public void DespawnAll()
    {
        foreach (var key in spawnedObjDict.Keys) 
        {
            if (spawnedObjDict.TryGetValue(key, out var value))
            {
                List<ScriptObject> despawnList = new List<ScriptObject>();

                value.ForEach(obj =>
                {
                    despawnList.Add(obj);
                });

                despawnList.ForEach(obj => Despawn<ScriptObject>(obj));
            }
        }
    }
}