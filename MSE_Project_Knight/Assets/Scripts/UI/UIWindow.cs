using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : ScriptObject
{
    private UIManager cachedUIManager;
    private float width;
    private float height;

    private void Awake()
    {
        //All UI will be added to Dictionary of UIManager.
        //You can access any UIWindow with UIManager Dictionary.

        cachedUIManager = UIManager.Instance;
        if (!cachedUIManager.uiWindowDict.ContainsKey(this.name))
        {
            cachedUIManager.uiWindowDict.Add(this.name, this);
        }

    }

    // If touch event is existed, implement event method on class which inherit this class. 


    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
