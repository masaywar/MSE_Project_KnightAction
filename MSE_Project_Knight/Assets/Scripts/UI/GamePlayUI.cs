using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : UIWindow
{
    private InGameController inGameController;

    public List<Button> buttonList = new List<Button>();

    private void Start()
    {
        inGameController = InGameController.Instance;
    }

    public void OnClickJump() 
    {
        //Notify to ingameController that player got jump attck message
    }
    public void OnClickAttack() 
    {
        //Notify to ingameController that player got attack message
    }
    public void OnClickUlt() 
    {
        //Notify to ingameController that player got ult message

        //Notify()
    }
}
