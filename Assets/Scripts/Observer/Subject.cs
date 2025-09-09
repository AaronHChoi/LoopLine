using System.Collections.Generic;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{
    protected private HashSet<IObserver> observers = new HashSet<IObserver>();

    protected private Dictionary<string, IObserver> observersByID = 
        new Dictionary<string, IObserver>();

    public void AddObserver(IObserver _observer)
    {
        string observerId = _observer.GetObserverID();

        if (string.IsNullOrEmpty(observerId))
        {
            observers.Add(_observer);
        } 
        else
        {
            observersByID[observerId] = _observer;
        }
        
    }
    public void RemoveObserver(IObserver _observer)
    {
        string observerId = _observer.GetObserverID();

        if (string.IsNullOrEmpty(observerId))
        {
            observers.Remove(_observer);
        }
        else if (observersByID.ContainsKey(observerId))
        {
            if(observersByID[observerId] == _observer)
            {
                observersByID.Remove(observerId);
            }
        }
    }
    protected void NotifyAllObservers(Events _event, string _targetId = null, bool stopAfterFirstMatch = true)
    {
       foreach(var observer in new HashSet<IObserver>(observers))
       { 
            observer.OnNotify(_event, null);
       }
    }
    protected void NotifyObserverById(Events _event, string targetId)
    {
        if(!string.IsNullOrEmpty(targetId) && observersByID.TryGetValue(targetId, out var observer))
        {
            observer.OnNotify(_event, targetId);
        }
    }
    protected void NotifyObservers(Events _event, string targetId = null)
    {
        if(string.IsNullOrEmpty(targetId))
        {
            NotifyAllObservers(_event);
        }
        else
        {
            NotifyObserverById(_event, targetId);
        }
    }
}