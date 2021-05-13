using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : UIWindow
{
    public RectTransform gameOverPanel;

    private void Update()
    {
        if (InGameController.Instance.player.state == Player.State.Over)
        {
            gameOverPanel.gameObject.SetActive(true);
        }
    }
}
