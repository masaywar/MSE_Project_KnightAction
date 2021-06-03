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
    GetCompDataByName Method

    Failure case:
    No name found -> return null CompData
    
    Succeed case:
    return user's CompData
    *************************************************************/
    public static CompData GetCompDataByName(string name)
    {
        string jsonForm = name;

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
    UpdateCompData Method

    Failure case:
    No name found -> return 0
    
    Succeed case -> return 1
    *************************************************************/
    public static int UpdateCompData(string userName, bool knight, bool archer, bool mage)
    {
        string k_bool = "true";
        string a_bool = "false";
        string m_bool = "false";

        if (archer == true)
        {
            a_bool = "true";
        }
        if (mage == true)
        {
            m_bool = "true";
        }
        if (knight == false)
        {
            k_bool = "false";
        }

        string jsonForm = "{\"userName\":\"" + userName + "\",\"knight\":" + k_bool + ",\"archer\":" + a_bool +
         ",\"mage\":" + m_bool + "}";

        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonForm);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverURL + "update/comp");
        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = bytes.Length;

        using (var stream = request.GetRequestStream())
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
