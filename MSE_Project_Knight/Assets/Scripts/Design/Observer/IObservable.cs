using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservable
{
    public void Subscribe(IObserver o);
    public void Unsubscribe(IObserver o);

}
