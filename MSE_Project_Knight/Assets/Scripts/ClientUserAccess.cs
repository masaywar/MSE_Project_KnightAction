// class PlayerScores

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FullSerializer;

using Proyecto26;

public class ClientUserAccess : MonoBehaviour
{ 
    UserClient user = new UserClient();

    public InputField emailText;
    public InputField usernameText;
    public InputField passwordText;

    public static fsSerializer serializer = new fsSerializer();
   

    // Start is called before the first frame update
    void Start()
    {
        //print("wow");
        //PostToDatabase();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(user.userName);
    }

    public void SignUpUserButton()
    {
        SignUpUser(emailText.text, usernameText.text, passwordText.text);
    }
    public void SignInUserButton()
    {
        SignInUser(emailText.text, passwordText.text);
    }
    public void UpdateUserButton()
    {
        UpdateUser(emailText.text, passwordText.text);
    }
    public void DeleteUserButton()
    {
        DeleteUser(emailText.text, passwordText.text);
    }
    public void RankingButton()
    {
        Ranking();
    }

    private void SignUpUser(string email, string username, string password)
    {
        string userData = "{\"userName\":\"" + username + "\",\"email\":\"" + email + "\",\"password\":\"" + password +
         "\",\"userVersion\":\"" + "1.0" + "\"}";
        RestClient.Post<UserClient>("http://localhost:9090/sak/signupuser", userData).Then(
            response =>
            {
                Debug.Log(response.userName + " sign up completed!");
            }).Catch(error =>
        {
            // If the request fails
            Debug.Log(error);
        });
    }



    private void SignInUser(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";
        RestClient.Post<UserClient>("http://localhost:9090/sak/signinuser", userData).Then(
            response =>
            {
                Debug.Log(response.email);
            }).Catch(error =>
        {
            // If the request fails
            Debug.Log(error);
        });
    }

    //change password
    private void UpdateUser(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"userVersion\":\"" + "1.0" + "\"}";
        RestClient.Post<UserClient>("http://localhost:9090/sak/updateuser", userData).Then(
            response =>
            {
                Debug.Log(response.email);
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });
    }

    //delete ID
    private void DeleteUser(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";
        RestClient.Post<UserClient>("http://localhost:9090/sak/deleteuser", userData).Then(
            response =>
            {
                Debug.Log(response.email);
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });
    }

    private void Ranking()
    {
        RestClient.Get<Rank>("http://localhost:9090/sak/sorted").Then(
            response =>
            {
                Debug.Log(response.score);
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });
    }
}