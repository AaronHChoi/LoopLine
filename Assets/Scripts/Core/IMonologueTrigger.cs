
public interface IMonologueTrigger
{
    Events monologueToTrigger { get; }
	int monologueDelay { get; }
    bool HasTriggered { get; set; }
} 