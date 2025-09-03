using UnityEngine;

public interface IObserver
{
    public void OnNotify(Events _event, string _id = null);
}
