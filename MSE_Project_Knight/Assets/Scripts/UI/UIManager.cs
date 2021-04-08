using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, UIWindow> _uiWindowDict;
    public Dictionary<string, UIWindow> uiWindowDict 
    {
        get 
        {
            if (_uiWindowDict == null)
                _uiWindowDict = new Dictionary<string, UIWindow>();

            return _uiWindowDict;
        }
        
    }
    
    private void Awake() 
    {
        this.gameObject.SetActive(false);
    }

    public T FindByWindowName<T>(string name) where T : UIWindow
    {
        UIWindow window = null;
        if (uiWindowDict.ContainsKey(name))
            window = uiWindowDict[name];

        return (T)window;
    }
}
