using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptObject : MonoBehaviour
{
    public string prefabName;
    public float speed;

    private float cachedSpeed;

    public RectTransform rectTransform;
    public Rigidbody2D rigidbody;
    public Collider2D collider;

    public Dictionary<string, List<ScriptObject>> m_cachedAllObjectDict;


    private void Awake()
    {
        m_cachedAllObjectDict = ObjectManager.Instance.allObjectDict;
        rectTransform = GetComponent<RectTransform>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        cachedSpeed = speed;

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

    public void Play()
    {
        speed = cachedSpeed;
    }

    public void Stop()
    {
        speed = 0;
    }

}
