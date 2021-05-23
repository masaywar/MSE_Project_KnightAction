using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitUI : UIWindow
{
    public Button quit;
    public Button back;


    private void Start()
    {
        Close();
    }

    public void OnClicQuit() 
    {
        //SAVE()
        //if save done
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnCLickBack()
    {
        Close();
    }
}
