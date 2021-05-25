//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class SignUpUI : MonoBehaviour
//{
//    public Text example;
//    public InputField emailText;
//    public InputField usernameText;
//    public InputField passwordText;
//    public InputField confirmPasswordText;
//    public string password1;
//    public string password2;

//    public LoginData user = new LoginData();
//    private LoginDataManager ld_mgr = new LoginDataManager();

//    public void SignUpUserButton()
//    {
//        example.text = "";

//        user = ld_mgr.SignUpUser(emailText.text, usernameText.text, passwordText.text);

//        if (user.email.Equals("error"))
//        {
//            example.text = "Invalid Email!";
//        }
//        else if (user.userName.Equals("error"))
//        {
//            example.text = "Duplicated User Name!";
//        }
//        else if (passwordText.text!=confirmPasswordText.text)
//        {
//            example.text = "Passwords are different!";
//        }
//        else
//        {
//            example.text = user.userName + "sign up succeed!";
//            SceneManager.LoadScene("Login");
//        }
//    }

//    public void BackButton()
//    {
//        SceneManager.LoadScene("Login");
//    }

//}
