using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    /*
    UserClient user = new UserClient();

    public Text allRank;

    public InputField emailText;
    public InputField usernameText;
    public InputField passwordText;
    public InputField newVersion;
    */

    public Text item1;
    public Text item2;
    public Text item3;
    public Text userCompanion;
    public Text coin;

    //public UserClient user;
    //public PlayerClient player;
    public ClientUserAccess ca = new ClientUserAccess();

    public void GameStartButton()
    {
        GameStart();
    }

    public void GameStart() {
        // Turning to ingame Scene.
        Debug.Log("In game start!");
        SceneManager.LoadScene("InGame");
    }

    // Start is called before the first frame update
    void Start()
    {
        //user = ClientUserAccess.menu.user;
        //player = access.getPlayer(user.userName);
        //UserClient user = gameObject.GetComponent<UserClient>();
        //
        //PlayerClient player = ca.getPlayer(user.userName);
        //
        //ClientUserAccess CUAinstance = new ClientUserAccess();
        //userCompanion.text = "Companion : " + CUAinstance.player.companion;
        //coin.text = "coin : " + CUAinstance.player.coin;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
