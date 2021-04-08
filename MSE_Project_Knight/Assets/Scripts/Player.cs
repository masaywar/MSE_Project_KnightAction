using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Object
{
    public bool state = true; // true is Not jumping

    public void Move()
    {
        if (state)
        {
            //Go up
        }
        else 
        {
            //Go Down
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        
    }

    public void Attack()
    { 
        //Maybe use raycast.
    }


}
