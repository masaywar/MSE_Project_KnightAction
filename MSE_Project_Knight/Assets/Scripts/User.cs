using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class User
{
    public string userName;
    public string userSkill;
    public int userScore;
    public string localId;

    public User()
    {
        // sample
        userSkill = "Default skill";
        userScore = 250;
        localId = JsonExample.localId;
        userName = "Default Anonymous";
    }
}
