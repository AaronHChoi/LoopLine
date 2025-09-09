
public interface IObserver
{
    string Id { get; }
    public void OnNotify(Events _event, string _id = null);
    string GetObserverID();
}
