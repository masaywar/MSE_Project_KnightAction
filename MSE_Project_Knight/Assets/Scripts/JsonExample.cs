// class PlayerScores

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FullSerializer;

using Proyecto26;

public class JsonExample : MonoBehaviour
{ 
    User user = new User();

    public InputField emailText;
    public InputField usernameText;
    public InputField passwordText;

    public static fsSerializer serializer = new fsSerializer();
    private string databaseURL = "https://mseprojectknight-default-rtdb.firebaseio.com/users";
    public static string localId;

    private string getlocalid;

    private string idToken;
    private string AuthKey = "AIzaSyAU_vVWSRJVB5ggIMReX65-TyIulK5mBc0";
    // Start is called before the first frame update
    void Start()
    {
        //print("wow");
        //PostToDatabase();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PostToDatabase(bool emptyScore = false)
    {
        if (emptyScore)
        {
            //user.userScore = 50; // 예시
        }
        RestClient.Put(databaseURL + "/" + localId +".json", user);
    }

    
    private void RetrieveFromDatabase()
    {
        RestClient.Get<User>(databaseURL + "/" + getlocalid  + ".json").Then(response =>
          {
              user = response;
          });
    }
    

    public void SignUpUserButton()
    {
        SignUpUser(emailText.text, usernameText.text, passwordText.text);
    }
    public void SignInUserButton()
    {
        SignInUser(emailText.text, passwordText.text);
    }

    private void SignUpUser(string email, string username, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        //string userData = "{\"email\":\"" + "kdh098@ajou.ac.kr" + "\",\"password\":\"" + "somepassword" + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + AuthKey, userData).Then(
            response =>
            {
                idToken = response.idToken;
                localId = response.localId;

                user.localId = response.localId;
                user.userName = username;


                PostToDatabase(true);
                
            }).Catch(error =>
        {
            // If the request fails
            Debug.Log(error);
        });
    }

    private void SignInUser(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + AuthKey, userData).Then(
            response =>
            {
                idToken = response.idToken;
                localId = response.localId;
                GetUsername();
                Debug.Log(user.userName + " sign in succeed");
            }).Catch(error =>
        {
            // If the request fails
            Debug.Log(error);
        });
    }
    
    private void GetUsername() 
    {
        RestClient.Get<User>(databaseURL + "/" + localId + ".json" + idToken).Then(response =>
            {
                user.userName = response.userName;
                //UpdateScore();
            });
    }
    
    private void getLocalId()
    {
        RestClient.Get(databaseURL + ".json").Then(response =>
            {
                //var username = getScoreText.text; 이건 강의에서 input field로 받아온 유저의 이름
                var username = user.userName; // 수정 요망
                fsData userData = fsJsonParser.Parse(response.Text);

                Dictionary<string, User> users = null;
                serializer.TryDeserialize(userData, ref users);  

                foreach (var u in users.Values)
                {
                    if(u.userName == username)
                    {
                        getlocalid = u.localId;
                        RetrieveFromDatabase();
                        break;
                    }
                }
                
            });
    }
    
}