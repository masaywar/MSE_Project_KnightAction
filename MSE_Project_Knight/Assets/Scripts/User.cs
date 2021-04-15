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

    public User()
    {
        // sample
        userName = "Sample name";
        userSkill = "Very nice!";
        userScore = 250;
    }
}
