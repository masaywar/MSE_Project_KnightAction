using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Net;
using System.IO;

using Newtonsoft.Json;

public class UserDataManager : MonoBehaviour
{
    private string localURL = "http://localhost:9090/sak/";
    private string alterURL = "Comming soon";

    /************************************************************
    GetUserDataByName Method

    Failure case:
    No user found -> return null UserData
    
    Succeed case:
    return user's UserData
    *************************************************************/
    public UserData GetUserDataByName(string name)
    {
        string jsonForm = "{\"userName\":\"" + name + "\",\"rank\":" + 0 + ",\"score\":" + 0 + "}";

        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonForm);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(localURL + "get/user");
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
        
        return JsonUtility.FromJson<UserData>(json);       
    }


    /************************************************************
    GetAllUserData Method
    *************************************************************/
    public List<UserData> GetAllUserData()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(localURL + "getall/user");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        return JsonConvert.DeserializeObject<List<UserData>>(json);
    }


    /************************************************************
    UpdataUserData Method

    name으로 DB에서 해당하는 데이터를 찾은 뒤 새로 주어진 score, coin, knight로 update

    Failure case:
    return 0
    
    Succeed case:
    return 1
    *************************************************************/
    public int UpdatUserData(string name, int score, int coin, string knight)
    {
        string jsonForm = "{\"userName\":\"" + name + "\",\"score\":" + score + ",\"coin\":" + coin +
         ",\"knight\":\"" + knight + "\"}";

        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonForm);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(localURL + "update/user");
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
    GetSortedRank Method

    Note:
    1등부터 차례로 Name, Rank, Score가 담긴 List 반환
    *************************************************************/
    public List<NameRank> GetSortedRank()
    {

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(localURL + "sorted");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        return JsonConvert.DeserializeObject<List<NameRank>>(json);
    }

    /************************************************************
    GetUserRank Method

    Failure case:
    User not found - return null NameRank
    
    Succeed case:
    return NameRank obj
    *************************************************************/
    public NameRank GetUserRank(string name)
    {
        string jsonForm = "{\"userName\":\"" + name + "\",\"score\":" + 0 + ",\"coin\":" + 0 +
        ",\"knight\":\"" + "dummy" + "\"}";
        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonForm);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(localURL + "getrankscore");
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
        
        return JsonUtility.FromJson<NameRank>(json);
    }
}
