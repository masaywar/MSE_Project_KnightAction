using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Net;
using System.IO;

using Newtonsoft.Json;

public static class CompDataManager
{
    private static string serverURL = URL.url;

    /************************************************************
    InsertComp Method // Insert companion data to DB table

    Failure case:
    return null CompData
    
    Succeed case:
    return inserted CompData
    *************************************************************/
    public static CompData InsertComp(CompData cd)
    {
        string jsonForm = "{\"userName\":\"" + cd.userName + "\",\"companion\":\"" + cd.companion + "\",\"level\":" + cd.level + "}";

        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonForm);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverURL + "insert/comp");
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
        
        return JsonUtility.FromJson<CompData>(json);   
    }

    /************************************************************
    GetOneComp Method // return corresponding CompData

    Failure case:
    return null CompData
    
    Succeed case:
    return CompData
    *************************************************************/
    public static CompData GetOneComp(string name, string comp)
    {
        string jsonForm = "{\"userName\":\"" + name + "\",\"companion\":\"" + comp +"\"}";

        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonForm);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverURL + "get/comp");
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
        
        return JsonUtility.FromJson<CompData>(json); 
    }


    /************************************************************
    GetCompByUserName Method

    Failure case:
    No name found -> return null
    
    Succeed case:
    return user's CompData list
    *************************************************************/
    public static List<CompData> GetCompByUserName(string name)
    {
        string jsonForm = "{\"userName\":\"" + name + "\"}";

        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonForm);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverURL + "get/comp/byname");
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
        
        return JsonConvert.DeserializeObject<List<CompData>>(json);
    }

    /************************************************************
    UpdateComp Method

    Failure case:
    No name found -> return 0
    
    Succeed case -> return 1
    *************************************************************/
    public static int UpdateComp(CompData cd)
    {
        string jsonForm = "{\"userName\":\"" + cd.userName + "\",\"companion\":\"" + cd.companion + "\",\"level\":" + cd.level + "}";

        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonForm);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverURL + "update/comp");
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
    DeleteComp Method

    Failure case:
    return 0
    
    Succeed case -> return 1
    *************************************************************/
    public static int DeleteComp(string name, string comp)
    {
        string jsonForm = "{\"userName\":\"" + name + "\",\"companion\":\"" + comp + "\"}";

        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonForm);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverURL + "delete/comp");
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
    DeleteCompByUserName Method

    return the number of deleted Comp data
    *************************************************************/
    public static int DeleteCompByUserName(string name)
    {
        string jsonForm = "{\"userName\":\"" + name + "\"}";

        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonForm);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverURL + "delete/comp/byname");
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
