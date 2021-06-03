using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClientUserData
{
    private static string _email;
    public static string email
    {
        get => _email;
        set => _email = value;
    }

    private static string _name;
    public static string name
    {
        get => _name;
        set => _name = value;
    }

    private static int _coin;
    public static int coin
    {
        get => _coin;
        set => _coin = value;
    }

    private static string _knight;
    public static string knight
    {
        get => _knight;
        set => _knight = value;
    }

    private static int _score;
    public static int score
    {
        get => _score;
        set => _score = value;
    }

    private static List<string> _companions = new List<string>();
    public static List<string> companions
    {
        get => _companions;
    }
}
