using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Proyecto26;

public static class UserDataManager
{
#if TEST
    public static string serverURL = "http://localhost:9090/sak/";
#else
     public static string serverURL = "http://localhost:9090/sak/";
#endif


    /************************************************************
    GetUserDataByName Method

    Failure case:
    No user found -> return null UserData
    
    Succeed case:
    return user's UserData
    *************************************************************/
    public static UserData GetUserDataByName(string name)
    {
        string jsonForm = "{\"userName\":\"" + name + "\",\"rank\":" + 0 + ",\"score\":" + 0 + "}";

        UserData returnValue = new UserData();

        RestClient.Post<UserData>(serverURL + "get/user", jsonForm).Then(
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


    /************************************************************
    GetAllUserData Method
    *************************************************************/
    public static List<UserData> GetAllUserData()
    {
        List<UserData> returnValue = new List<UserData>();

        RestClient.GetArray<UserData>(serverURL + "getall/user").Then(
            response =>
            {
                returnValue = response.ToList();
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });

        return returnValue;
    }


    /************************************************************
    UpdataUserData Method

    name으로 DB에서 해당하는 데이터를 찾은 뒤 새로 주어진 score, coin, knight로 update

    Failure case:
    return 0
    
    Succeed case:
    return 1
    *************************************************************/
    public static int UpdatUserData(string name, int score, int coin, string knight)
    {
        string jsonForm = "{\"userName\":\"" + name + "\",\"score\":" + score + ",\"coin\":" + coin +
         ",\"knight\":\"" + knight + "\"}";

        int returnValue = 0;

        RestClient.Post<Flag>(serverURL + "update/user", jsonForm).Then(
            response =>
            {
                returnValue = response.flag;
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });

        return returnValue;          

    }

    /************************************************************
    GetSortedRank Method

    Note:
    1등부터 차례로 Name, Rank, Score가 담긴 List 반환
    *************************************************************/
    public static List<NameRank> GetSortedRank()
    {
        List<NameRank> returnValue = new List<NameRank>();

        RestClient.GetArray<NameRank>(serverURL + "sorted").Then(
            response =>
            {
                returnValue = response.ToList();
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });
        return returnValue;
    }

    /************************************************************
    GetUserRank Method

    Failure case:
    User not found - return null NameRank
    
    Succeed case:
    return NameRank obj
    *************************************************************/
    public static NameRank GetUserRank(string name)
    {
        string jsonForm = "{\"userName\":\"" + name + "\",\"score\":" + 0 + ",\"coin\":" + 0 +
         ",\"knight\":\"" + "dummy" + "\"}";

        NameRank returnValue = new NameRank();

        RestClient.Post<NameRank>(serverURL + "getrankscore", jsonForm).Then(
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
}
