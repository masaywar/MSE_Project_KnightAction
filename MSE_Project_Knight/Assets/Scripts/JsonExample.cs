using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Proyecto26;

public class JsonExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print("wow");
        PostToDatabase();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PostToDatabase()
    {
        User user = new User();
        RestClient.Put("https://mseprojectknight-default-rtdb.firebaseio.com/.json", user);
    }

    
}
