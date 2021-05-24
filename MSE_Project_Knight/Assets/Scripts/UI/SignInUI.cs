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

    private Dictionary<string, string> buttonsActionDict = new Dictionary<string, string>();

    private void Start()
    {
        buttons.ForEach(button=> 
            buttonsActionDict.Add(button.name, "OnClick"+button.name)
        );

        buttonsActionDict.ForEach(keyValue => SendMessage(keyValue.Value));
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
        string emailText = findFieldText("EmailField");
        string pwText = findFieldText("PasswordField");

        LoginData data = LoginDataManager.SignInUser(emailText, pwText);

        if (data != null)
        {
            UserData userData = UserDataManager.GetUserDataByName(data.userName);

            ClientUserData.name = userData.userName;
            ClientUserData.coin = userData.coin;
            ClientUserData.knight = userData.knight;
            ClientUserData.email = data.email;
            ClientUserData.score = userData.score;
        }
    }

    public void OnClickSignUp()
    {
        string emailText = findFieldText("EmialField");
        string pwText = findFieldText("OasswirdFuekd");
        string userName = findFieldText("userName");

        LoginDataManager.SignUpUser(emailText, userName, pwText);
    }


}
