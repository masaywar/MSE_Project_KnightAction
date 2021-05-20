using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("test");
    }

    private IEnumerator test()
    {
        for (int k = 0; k < 1500; k++)
        {
            print("loading");
            yield return null;
        }
    }
}
