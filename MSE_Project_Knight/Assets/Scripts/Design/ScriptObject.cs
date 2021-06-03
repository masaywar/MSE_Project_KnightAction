using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptObject : MonoBehaviour
{
    public string prefabName;
    public float speed;

    public RectTransform rectTransform;
    public Rigidbody2D rigidbody;
    public Collider2D collider;
    public bool isDead;

    public Dictionary<string, List<ScriptObject>> m_cachedAllObjectDict;


    private void Awake()
    {
        m_cachedAllObjectDict = ObjectManager.Instance.allObjectDict;
        rectTransform = GetComponent<RectTransform>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

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
        if (collider != null)
            collider.enabled = true;

        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
    }
    private void OnDisable()
    {
        Initialize();
        if (ObjectManager.Instance.spawnedObjDict.ContainsKey(prefabName))
        {
            if (ObjectManager.Instance.spawnedObjDict[prefabName].Contains(this))
                ObjectManager.Instance.Despawn<ScriptObject>(this);
        }
       
        isDead = false;
    }
}
