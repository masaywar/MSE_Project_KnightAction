using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SignUpUI : UIWindow
{
    public TMP_InputField emailText;
    public TMP_InputField usernameText;
    public TMP_InputField passwordText;
    public TMP_InputField confirmPasswordText;

    public Button SignUp;

    private void Start()
    {
        Close();
    }

    public void SignUpUserButton()
    {
        string message;
        if (passwordText.text != confirmPasswordText.text)
        {
            message = "Passwords are different!";
            ToastMessenger.ShowToast(message);
            return;
        }

        var user = LoginDataManager.SignUpUser(emailText.text, usernameText.text, passwordText.text);

        if (user.email.Equals("error"))
        {
            message = "Invalid Email!";
            ToastMessenger.ShowToast(message);
        }
        else if (user.userName.Equals("error"))
        {
            message = "Duplicated User Name!";
            ToastMessenger.ShowToast(message);
        }
      
        else
        {
            message = "sign up succeed! Please sign in again.";
            ToastMessenger.ShowToast(message);
            Close();
        }
    }

    public void OnClickBack()
    {
        Close();
    }
}
