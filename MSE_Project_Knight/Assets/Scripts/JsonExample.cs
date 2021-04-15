using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class JsonExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DataSave()
    {
        Dictionary<string, int> UserData = new Dictionary<string, int>();
        UserData["Happy"] = 15;
        UserData["Sopia"] = 20;

        string json = JsonConvert.SerializeObject(UserData);

        UnityWebRequest.Put("https://mseprojectknight-default-rtdb.firebaseio.com/.json", json).SendWebRequest();
        yield return null;
    }
}
