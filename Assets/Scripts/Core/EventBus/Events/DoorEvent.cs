
namespace Core.EventBus
{
	public struct DoorEvent : IGameEvent
	{
		public EventsID SoundID;
		public bool ShouldPlay;
	} 
}