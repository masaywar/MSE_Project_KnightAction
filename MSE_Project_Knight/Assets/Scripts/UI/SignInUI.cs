using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class SignInUI : UIWindow
{
    public Button[] buttons;
    public InputField[] inputFields;

    private Dictionary<Button, string> buttonsActionDict = new Dictionary<Button, string>();

    private void Start()
    {
        buttons.ForEach(button=> 
            buttonsActionDict.Add(button, "OnClick"+button.name)
        );

        buttonsActionDict.ForEach(keyValue => keyValue.Key.onClick.AddListener(()=> SendMessage(keyValue.Value)));
    }

    private string findFieldText(string fieldName)
    {
        foreach (var field in inputFields)
        {
            if (field.name == fieldName)
            {
                return field.text;
            }
        }

        return null;
    }

    public void OnClickSignIn()
    {
#if TEST
        GameManager.Instance.gameState = GameManager.GameState.main;
#else


        string emailText = findFieldText("EmailField");
        string pwText = findFieldText("PasswordField");

        LoginData data = LoginDataManager.SignInUser(emailText, pwText);

        print(data.email);
        print(data.id);
        print(data.userName);

        if (data.email != null)
        {
            UserData userData = UserDataManager.GetUserDataByName(data.userName);

            ClientUserData.name = userData.userName;
            ClientUserData.coin = userData.coin;
            ClientUserData.knight = userData.knight;
            ClientUserData.email = data.email;
            ClientUserData.score = userData.score;

            print(ClientUserData.name);
            print(ClientUserData.coin);
            print(ClientUserData.knight);
            print(ClientUserData.email);
            print(ClientUserData.score);

            GameManager.Instance.gameState = GameManager.GameState.main;
        }

        else 
        {
            //Notify "Sign up!"
        }
#endif
    }

    public void OnClickSignUp()
    {
        string emailText = findFieldText("EmailField");
        string pwText = findFieldText("PasswirdFuekd");
        string userName = findFieldText("UserNameField");

        LoginDataManager.SignUpUser(emailText, userName, pwText);
    }


}
