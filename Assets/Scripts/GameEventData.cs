
public class GameEventData
{
    public Events EventType { get; }
    public string TargetId { get; }

    public GameEventData(Events eventType, string targetId = null)
    {
        EventType = eventType;
        TargetId = targetId;
    }
}