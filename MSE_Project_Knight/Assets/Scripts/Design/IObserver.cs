using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    public abstract void GetUpdate(string name, string diff);
}

