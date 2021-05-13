// class PlayerScores

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Proyecto26;

public class ClientUserAccess : MonoBehaviour
{ 
    UserClient user = new UserClient();

    public Text allRank;

    public InputField emailText;
    public InputField usernameText;
    public InputField passwordText;
    public InputField newVersion;

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
        UpdateUser(emailText.text, passwordText.text, newVersion.text);
    }
    public void DeleteUserButton()
    {
        DeleteUser(emailText.text, usernameText.text, passwordText.text);
    }
    public void GetAllRankButton()
    {
        getAllRank();
    }

    public void getPlayerRankButton()
    {
        getPlayerRank(usernameText.text);
    }

    private UserClient SignUpUser(string email, string username, string password)
    {
        string userData = "{\"userName\":\"" + username + "\",\"email\":\"" + email + "\",\"password\":\"" + password +
         "\",\"userVersion\":\"" + "1.0" + "\"}";

        UserClient returnValue = new UserClient();

        RestClient.Post<UserClient>("http://localhost:9090/sak/signupuser", userData).Then(
            response =>
            {
                if (response.email.Equals("error")){
                    // When Email is already in DB or user doesn't give any email
                    Debug.Log("Invalid email!");
                    returnValue = null;
                }
                else if (response.userName.Equals("error")){
                    // When userName is already in DB
                    Debug.Log("Please use another Nickname!");
                    returnValue = response;
                }
                else{
                    // Sign up succeed
                    Debug.Log("Hi " + response.userName + ". Sign up completed!");
                    returnValue = null;
                }
            }).Catch(error =>
        {
            // If the request fails
            Debug.Log(error);
        });
        // If signup fails, then return null. If signup succeed, then return the new signup userclient object.
        return returnValue;
    }



    private UserClient SignInUser(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";

        UserClient returnValue = new UserClient();

        RestClient.Post<UserClient>("http://localhost:9090/sak/signinuser", userData).Then(
            response =>
            {
                if (response == null){
                    Debug.Log("Please check Email again..");
                    returnValue = null;
                }
                else if (response.password.Equals("error")){
                    Debug.Log("Please check password again..");
                    returnValue = null;
                }
                else{
                    Debug.Log("Hi " + response.userName);
                    returnValue = response;
                }
            }).Catch(error =>
        {
            // If the request fails
            Debug.Log(error);
        });
        // If signin fails, then return null. If signin succeed, then return userclient object.
        return returnValue;
    }

    //change password
    private UserClient UpdateUser(string email, string password, string userVersion)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"userVersion\":\"" + userVersion + "\"}";

        UserClient returnValue = new UserClient();

        RestClient.Post<UserClient>("http://localhost:9090/sak/updateuser", userData).Then(
            response =>
            {
                if (!response.userVersion.Equals("")){
                    returnValue = response;
                    Debug.Log("Update user"+ response.userName +"'s data succeed");
                }
                else {
                    Debug.Log("Update fails");
                }
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });
        // If succeed, return updated user, If failed, return null user
        return returnValue;
    }

    //delete ID
    /*
    * Important Task : Password check needed.
    */
    private UserClient DeleteUser(string email, string username, string password)
    {
        // JSON body only needs email & username. The rest of the data don't matter.
        string userData = "{\"userName\":\"" + username + "\",\"email\":\"" + email + "\",\"password\":\"" + password +
         "\",\"userVersion\":\"" + "1.0" + "\"}";

        UserClient returnValue = new UserClient();

        RestClient.Post<UserClient>("http://localhost:9090/sak/deleteuser", userData).Then(
            response =>
            {
                if (!response.userVersion.Equals("")){
                    Debug.Log("Account " + response.email + "successfully deleted..Good Bye!");
                    returnValue = response;
                }
                else{
                    Debug.Log("Deletion failed");
                }
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });
        return returnValue;
    }


    // Json array를 받아오지 못하는 문제점이 있음
    private List<Rank> getAllRank()
    {
        List<Rank> returnValue = new List<Rank>();

        RestClient.Get<List<Rank>>("http://localhost:9090/sak/sorted").Then(
            response =>
            {
                Debug.Log("Checking");
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });

        return returnValue;
    }

    /*
    private Rank getPlayerRank(PlayerClient p){

        string userData = "{\"userName\":\"" + p.userName + "\",\"score\":" + p.score + ",\"coin\":" + p.coin +
         ",\"companion\":\"" + p.companion + "\"}";

        Rank returnValue = new Rank();

        RestClient.Post<Rank>("http://localhost:9090/sak/getrankscore", userData).Then(
            response =>
            {
                returnValue = response;
                Debug.Log(response);
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });

        return returnValue;

    }
    */
    private Rank getPlayerRank(string name){

        string userData = "{\"userName\":\"" + name + "\",\"score\":" + 123 + ",\"coin\":" + 123 +
         ",\"companion\":\"" + "Dummy" + "\"}";

        Rank returnValue = new Rank();

        RestClient.Post<Rank>("http://localhost:9090/sak/getrankscore", userData).Then(
            response =>
            {
                returnValue = response;
                Debug.Log(response.rank);
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });

        return returnValue;

    }



}