using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataModUI : UIWindow
{
    public TMP_InputField username;
    public TMP_InputField pw;
    public TMP_InputField validPw;

    private void Start()
    {
        Close();
    }

    public void OnClickConfirm()
    {
        if (username.text == "")
        {
#if !UNITY_EDITOR
            ToastMessenger.ShowToast("No Empty username");
#endif
            return;
        }

        else if (pw.text == "")
        {
#if !UNITY_EDITOR
            ToastMessenger.ShowToast("No Empty password");
#endif
            return;
        }

        else if (pw.text != validPw.text)
        {
#if !UNITY_EDITOR
            ToastMessenger.ShowToast("Passwords are diffrent!");
#endif
            return;
        }

        //if (UserDataManager.UpdatUserData(username.text, ClientUserData.score, ClientUserData.coin, ClientUserData.knight) == 0)
        //{
        //    print("userdata error");
        //}

        //if (LoginDataManager.UpdateLoginData(ClientUserData.email, pw.text, "1.0") == 0)
        //{
        //    print("loginData error");
        //}
        //ClientUserData.name = username.text;

        Close();
    }
}
