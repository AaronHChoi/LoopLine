using System.Collections.Generic;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{
    private List<IObserver> observers = new List<IObserver>();

    public void AddObserver(IObserver _observer)
    {
        observers.Add(_observer);
    }
    public void RemoveObserver(IObserver _observer)
    {
        observers.Remove(_observer);
    }
    protected void NotifyObservers(Events _event)
    {
        //observers.ForEach((observers) => { observers.OnNotify(_event); });
        var observersCopy = new List<IObserver>(observers);
        foreach (var observer in observersCopy)
        {
            observer.OnNotify(_event);
        }
    }
}