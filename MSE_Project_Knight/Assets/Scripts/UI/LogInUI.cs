using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogInUI : MonoBehaviour
{
    public Text example;
    public Text signInEmailError;
    public Text signInPasswordError;
    public InputField emailText;
    public InputField passwordText;

    public LoginData user = new LoginData();
    private LoginDataManager ld_mgr = new LoginDataManager();


    public void SignInUserButton()
    {
        example.text = "";
        //SignInUser(emailText.text, passwordText.text);
        user = ld_mgr.SignInUser(emailText.text, passwordText.text);
        if (user == null)
        {
            example.text = "Please check email again!";
        }
        else if (user.password.Equals("error"))
        {
            example.text = "Please check password again!";
        }
        else
        {
            example.text = user.userName + " Sign in succeed!";
            SceneManager.LoadScene("Menu");
        }
    }

    public void NewAccountButton()
    {
        SceneManager.LoadScene("SignUp");
    }

    public void QuitButton()
    {
        Application.Quit(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitButton();
        }
    }
}
