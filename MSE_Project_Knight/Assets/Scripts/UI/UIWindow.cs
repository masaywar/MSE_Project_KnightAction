using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIWindow : ScriptObject
{
    public CanvasGroup canvasGroup;

    private UIManager cachedUIManager;
    private float width;
    private float height;

    private void Awake()
    {
        cachedUIManager = UIManager.Instance;
        canvasGroup = GetComponent<CanvasGroup>();

        if (!cachedUIManager.uiWindowDict.ContainsKey(this.name))
        {
            cachedUIManager.uiWindowDict.Add(this.name, this);
        }

        Open();
    }

    public void UnableBlockRaycast()
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void EnableBlockRaycast()
    {
        canvasGroup.blocksRaycasts = true;
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        cachedUIManager.BlockRaycastWithout(this);
        cachedUIManager.openedWindowList.Add(this);
        canvasGroup.blocksRaycasts = true;
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
        cachedUIManager.openedWindowList.Remove(this);
        cachedUIManager.GetTop().EnableBlockRaycast();
    }

    public bool IsTop()
    {
        return cachedUIManager.GetTop() == this;
    }

    public bool IsOpen()
    {
        return gameObject.activeInHierarchy;
    }

    private void OnDestroy()
    {
        cachedUIManager.openedWindowList.Remove(this);
        cachedUIManager.uiWindowDict.Remove(name);
    }
}
