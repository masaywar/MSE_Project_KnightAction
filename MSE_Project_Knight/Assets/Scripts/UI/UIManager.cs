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

    [SerializeField]
    private List<UIWindow> _openedWindowList;
    public List<UIWindow> openedWindowList
    {
        get
        {
            if (_openedWindowList == null)
                _openedWindowList = new List<UIWindow>();

            return _openedWindowList;
        }
    }
    public UIWindow cachedWindow;


    public T GetActiveWindow<T>(T window) where T : UIWindow
    {
        if (window == null)
            return null;

        return (T)openedWindowList.Find(w => w.gameObject == window.gameObject);
    }

    public T GetActiveWindow<T>(string name) where T : UIWindow
    {
        var windows = openedWindowList.FindAll(window => window.name == name);

        if (windows.Count > 1)
        {
            print("UIWindow Error! Each UI has to exist onle one.");
            return null;
        }

        var window = windows[windows.Count - 1];
        return (T)window;
    }

    public T GetWindow<T>(string name) where T : UIWindow
    {
        if (uiWindowDict.TryGetValue(name, out var window))
        {
            return (T)window;
        }

        return null;
    }

    public void RemoveWindow<T>(T window) where T : UIWindow
    {
        if (window == null)
            return;

        openedWindowList.Remove(window);
    }

    public UIWindow GetTop()
    {
        if (openedWindowList.Count > 0)
            return openedWindowList[openedWindowList.Count-1];

        return null;
    }

    public void BlockRaycastWithout(UIWindow window)
    {
        if (window != null && openedWindowList.Count > 0)
        {
            openedWindowList.ForEach(w => {
                if (w.gameObject != window.gameObject)
                {
                    w.UnableBlockRaycast();
                }
            });
        }
    }

    public T FindByWindowName<T>(string name) where T : UIWindow
    {
        T window = null;
        if (uiWindowDict.ContainsKey(name))
            window = (T)uiWindowDict[name];

        return window;
    }
}
