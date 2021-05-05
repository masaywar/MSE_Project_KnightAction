using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDespawnPlace : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("Trigger!");
        if (other.tag == "Destoryable" || other.tag == "Undestroyable") 
            other.gameObject.SetActive(false);
    }
}
