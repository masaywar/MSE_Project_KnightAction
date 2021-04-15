using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontBeDestroyed : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
