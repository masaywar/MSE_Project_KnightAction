using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Net;
using System.IO;

using Newtonsoft.Json;

public static class LoginDataManager
{

    public static string serverURL = URL.url;
 
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
    public static LoginData SignUpUser(string email, string username, string password)
    {
        string jsonForm = "{\"userName\":\"" + username + "\",\"email\":\"" + email + "\",\"password\":\"" + password +
         "\",\"userVersion\":\"" + "1.0" + "\"}";
        
        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonForm);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverURL + "signupuser");
        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = bytes.Length;

        using(var stream = request.GetRequestStream())
        {
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            stream.Close();
        }
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        
        return JsonUtility.FromJson<LoginData>(json);       
    }
    

    /************************************************************
    Sign in Process Method

    Failure case:
    email이 DB에 없을 때 ( email이 틀렸을 때 ) -> Return null UserData
    email은 맞으나 password가 틀렸을 때 -> Return UserData with UserData.password = "error"
    
    Succeed case:
    return signed in user's LoginData
    *************************************************************/
    public static LoginData SignInUser(string email, string password)
    {
        string jsonForm = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";

        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonForm);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverURL + "signinuser");
        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = bytes.Length;

        using(var stream = request.GetRequestStream())
        {
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            stream.Close();
        }

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        
        return JsonUtility.FromJson<LoginData>(json);
    }


    /************************************************************
    GetAllLoginData Method  
    *************************************************************/
    public static List<LoginData> GetAllLoginData()
    {   
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverURL + "getall/logindata");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        return JsonConvert.DeserializeObject<List<LoginData>>(json);
    }


    /************************************************************
    Get Login Data by email Method

    Failure case:
    email이 DB에 없을 때: return null
    
    Succeed case:
    return user's LoginData
    *************************************************************/
    public static LoginData GetLoginDataByEmail(string email)
    {
        string jsonForm = "{\"email\":\"" + email + "\",\"password\":\"" + "dummyPassword" + "\"}";

        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonForm);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverURL + "get/logindata");
        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = bytes.Length;

        using(var stream = request.GetRequestStream())
        {
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            stream.Close();
        }
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        
        return JsonUtility.FromJson<LoginData>(json);

    }


    /************************************************************
    UpdateLoginData Method

    email 값으로 DB에서 데이터를 찾음 / LoginData에서 email에 해당하는 password와 userVersion을 update할 때 사용

    Failure case:
    Update failed -> return 0
    
    Succeed case:
    return 1
    *************************************************************/
    public static int UpdateLoginData(string email, string password, string userVersion)
    {
        string jsonForm = "{\"userName\":\"" + "dummy" + "\",\"email\":\"" + email + "\",\"password\":\"" + password +
         "\",\"userVersion\":\"" + userVersion + "\"}";

        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonForm);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverURL + "update/logindata");
        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = bytes.Length;

        using(var stream = request.GetRequestStream())
        {
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            stream.Close();
        }
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        
        Flag receivedFlag = JsonUtility.FromJson<Flag>(json);

        return receivedFlag.flag;
    }


    /************************************************************
    DeleteUser Method

    Failure case:
    Password가 맞지 않을 때: return 2
    email과 username 중 하나가 맞지 않을 때: return 0
    
    Succeed case:
    return 1
    *************************************************************/
    public static int DeleteUser(string email, string username, string password)
    {
        string jsonForm = "{\"userName\":\"" + username + "\",\"email\":\"" + email + "\",\"password\":\"" + password +
         "\",\"userVersion\":\"" + "dummy" + "\"}";

        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonForm);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverURL + "delete");
        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = bytes.Length;

        using(var stream = request.GetRequestStream())
        {
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            stream.Close();
        }
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        
        Flag receivedFlag = JsonUtility.FromJson<Flag>(json);
        return receivedFlag.flag;
    }
}
