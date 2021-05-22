using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Proyecto26;



public class LoginDataManager : MonoBehaviour
{

    public string localURL = "http://localhost:9090/sak/";
    public string alterURL = "Comming soon";

    
    /************************************************************
    Sign up Process Method

    Failure case:
    email이 이미 DB에 존재할 때 -> Return UserData with UserData.email = "error"
    username이 이미 DB에 존재할 때 -> Return UserData with UserData.username = "error"
    
    Succeed case:
    return signed up LoginData

    Note:
    * Default userVersion = 1.0
    * Sign up을 하면 username을 참조하여 default userData도 DB에 생성됨
    *************************************************************/
    public LoginData SignUpUser(string email, string username, string password)
    {
        string jsonForm = "{\"userName\":\"" + username + "\",\"email\":\"" + email + "\",\"password\":\"" + password +
         "\",\"userVersion\":\"" + "1.0" + "\"}";

        LoginData returnValue = new LoginData();

        RestClient.Post<LoginData>(localURL + "signupuser", jsonForm).Then(
            response =>
            {
                Debug.Log("From LoginDataManager: Received sign up request");
                returnValue = response;
            }).Catch(error =>
        {
            Debug.Log(error);
        });
        return returnValue;
    }
    

    /************************************************************
    Sign in Process Method

    Failure case:
    email이 DB에 없을 때 ( email이 틀렸을 때 ) -> Return null UserData
    email은 맞으나 password가 틀렸을 때 -> Return UserData with UserData.password = "error"
    
    Succeed case:
    return signed in user's LoginData
    *************************************************************/
    public LoginData SignInUser(string email, string password)
    {
        string jsonForm = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";

        LoginData returnValue = new LoginData();

        RestClient.Post<LoginData>(localURL + "signinuser", jsonForm).Then(
            response =>
            {
                Debug.Log("From LoginDataManager: Received sign in request");
                returnValue = response;
            }).Catch(error =>
        {
            Debug.Log(error);
        });

        return returnValue;
    }


    /************************************************************
    GetAllLoginData Method
    *************************************************************/
    public List<LoginData> GetAllLoginData()
    {   
        List<LoginData> returnValue = new List<LoginData>();

        RestClient.GetArray<LoginData>(localURL + "getall/logindata").Then(
            response =>
            {
                Debug.Log("From LoginDataManager: Received GetAllLoginData request");
                returnValue = response.ToList();

            }).Catch(error =>
        {
            Debug.Log(error);
        });

        return returnValue;
    }


    /************************************************************
    Get Login Data by email Method

    Failure case:
    email이 DB에 없을 때: return null
    
    Succeed case:
    return user's LoginData
    *************************************************************/
    public LoginData GetLoginDataByEmail(string email)
    {
        string jsonForm = "{\"email\":\"" + email + "\",\"password\":\"" + "dummyPassword" + "\"}";

        LoginData returnValue = new LoginData();

        RestClient.Post<LoginData>(localURL + "get/logindata", jsonForm).Then(
            response =>
            {
                Debug.Log("From LoginDataManager: Received GetLoginDataByEmail request");
                returnValue = response;
            }).Catch(error =>
        {
            Debug.Log(error);
        });

        return returnValue;

    }


    /************************************************************
    UpdateLoginData Method

    Failure case:
    Update failed -> return 0
    
    Succeed case:
    return 1
    *************************************************************/
    public int UpdateLoginData(string email, string password, string userVersion)
    {
        string jsonForm = "{\"userName\":\"" + "dummy" + "\",\"email\":\"" + email + "\",\"password\":\"" + password +
         "\",\"userVersion\":\"" + userVersion + "\"}";

        int returnValue = 0;

        RestClient.Post<Flag>(localURL + "update/logindata", jsonForm).Then(
            response =>
            {
                Debug.Log("From LoginDataManager: Received updateLoginData request");
                returnValue = response.flag;
            }).Catch(error =>
        {
            Debug.Log(error);
        });

        return returnValue;
    }


    /************************************************************
    DeleteUser Method

    Failure case:
    Password가 맞지 않을 때: return 2
    email과 username 중 하나가 맞지 않을 때: return 0
    
    Succeed case:
    return 1
    *************************************************************/
    public int DeleteUser(string email, string username, string password)
    {
        string jsonForm = "{\"userName\":\"" + username + "\",\"email\":\"" + email + "\",\"password\":\"" + password +
         "\",\"userVersion\":\"" + "dummy" + "\"}";

        int returnValue = 0;

        RestClient.Post<Flag>(localURL + "delete", jsonForm).Then(
            response =>
            {
                Debug.Log("From LoginDataManager: Received user deletion request");
                returnValue = response.flag;
            }).Catch(error =>
        {
            Debug.Log(error);
        });

        return returnValue;
    }
}
