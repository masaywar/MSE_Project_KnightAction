using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using TMPro;

public class SignInUI : UIWindow
{
    public Button[] buttons;
    public TMP_InputField[] inputFields;

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

        if (data.email != null)
        {
            UserData userData = UserDataManager.GetUserDataByName(data.userName);

            ClientUserData.name = userData.userName;
            ClientUserData.coin = userData.coin;
            ClientUserData.knight = userData.knight;
            ClientUserData.email = data.email;
            ClientUserData.score = userData.score;

            GameManager.Instance.gameState = GameManager.GameState.main;
        }

        else 
        {
            ToastMessenger.ShowToast("Cannot find email!, if you don't have account, please sign up.");
        }
#endif
    }

    public void OnClickSignUp()
    {
        UIManager.Instance.GetWindow<SignUpUI>("SignUpUI").Open();
    }

    public void OnClickQuit()
    {
        var window = UIManager.Instance.GetWindow<QuitUI>("QuitUI");
        window.Open();
    }

}
