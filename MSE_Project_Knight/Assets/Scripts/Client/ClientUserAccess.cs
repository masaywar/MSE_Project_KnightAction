// class PlayerScores

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

using Proyecto26;

public class ClientUserAccess : MonoBehaviour
{
    public Text example;
    public Text userCompanion;
    public Text coin;

    public InputField emailText;
    public InputField usernameText;
    public InputField passwordText;
    public InputField newVersion;

    public string realURL = "122.35.41.80:9090/sak/";
    public string localURL = "http://localhost:9090/sak/";

    public UserClient user = new UserClient();
    public PlayerClient player = new PlayerClient();

    public static ClientUserAccess CUAinstance = null;  

    //Awake is always called before any Start functions

    void Awake()
    {
        //Check if instance already exists
        if (CUAinstance == null)
        {        
            //if not, set instance to this
            CUAinstance = this;
        }
        //If instance already exists and it's not this:
        else if (CUAinstance != this)
        {        
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);    
        }   
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
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
        GetAllRank();
    }

    public void GetPlayerRankButton()
    {
        getPlayerRank(usernameText.text);
    }


    public UserClient SignUpUser(string email, string username, string password)
    {
        string userData = "{\"userName\":\"" + username + "\",\"email\":\"" + email + "\",\"password\":\"" + password +
         "\",\"userVersion\":\"" + "1.0" + "\"}";

        UserClient returnValue = new UserClient();

        RestClient.Post<UserClient>(realURL + "signupuser", userData).Then(
            response =>
            {
                if (response.email.Equals("error")){
                    // When Email is already in DB or user doesn't give any email
                    Debug.Log("Invalid email!");
                    example.text = "Invalid email!"; 
                    returnValue = null;
                }
                else if (response.userName.Equals("error")){
                    // When userName is already in DB
                    Debug.Log("Please use another Nickname!");
                    example.text = "Please use another Nickname!"; 
                    returnValue = response;
                }
                else{
                    // Sign up succeed
                    Debug.Log("Hi " + response.userName + ". Sign up completed!");
                    example.text = "Hi " + response.userName + ". Sign up completed!";
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

    public UserClient SignInUser(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";

#if UNITY_EDITOR
        SceneManager.LoadScene("Menu");
        return null;
#endif


        UserClient returnValue = new UserClient();

        RestClient.Post<UserClient>(realURL + "signinuser", userData).Then(
            response =>
            {
                if (response == null){
                    Debug.Log("Please check Email again..");
                    example.text = "Please check Email again..";

                    returnValue = null;
                }
                else if (response.password.Equals("error")){
                    Debug.Log("Please check password again..");
                    example.text = "Please check password again..";
                    returnValue = null;
                }
                else{
                    Debug.Log("Hi " + response.userName);
                    example.text = "Hi " + response.userName;
                    
                    user.id = response.id;
                    user.userName = response.userName;
                    user.email = response.email;
                    user.password = response.password;
                    user.userVersion = response.userVersion;

                    PlayerClient p = GetPlayer(user.userName);
                    player.companion = p.companion;
                    player.coin = p.coin;

                    SceneManager.LoadScene("Menu");

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
    public UserClient UpdateUser(string email, string password, string userVersion)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"userVersion\":\"" + userVersion + "\"}";

        UserClient returnValue = new UserClient();

        RestClient.Post<UserClient>(realURL + "updateuser", userData).Then(
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
    public UserClient DeleteUser(string email, string username, string password)
    {
        // JSON body only needs email & username. The rest of the data don't matter.
        string userData = "{\"userName\":\"" + username + "\",\"email\":\"" + email + "\",\"password\":\"" + password +
         "\",\"userVersion\":\"" + "1.0" + "\"}";

        UserClient returnValue = new UserClient();

        RestClient.Post<UserClient>(realURL + "deleteuser", userData).Then(
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

    public PlayerClient GetPlayer (string name){

        string userData = "{\"userName\":\"" + name + "\",\"rank\":" + 123 + ",\"score\":" + 123 + "}";

        PlayerClient returnValue = new PlayerClient();

        RestClient.Post<PlayerClient>(realURL + "getplayer", userData).Then(
            response =>
            {
                returnValue = response;
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });

        return returnValue;

    }


    // Json array를 받아오지 못하는 문제점이 있음
    public List<Rank> GetAllRank()
    {
        List<Rank> returnValue = new List<Rank>();

        RestClient.Get<List<Rank>>(realURL + "sorted").Then(
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
    public Rank getPlayerRank(string name){

        string userData = "{\"userName\":\"" + name + "\",\"score\":" + 123 + ",\"coin\":" + 123 +
         ",\"companion\":\"" + "Dummy" + "\"}";

        Rank returnValue = new Rank();

        RestClient.Post<Rank>(realURL + "getrankscore", userData).Then(
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