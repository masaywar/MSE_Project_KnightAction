using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : Object
{
    private UIManager cachedUIManager;

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
}
