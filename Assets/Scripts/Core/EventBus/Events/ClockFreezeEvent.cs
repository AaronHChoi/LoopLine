
namespace Core.EventBus
{
	public struct ClockFreezeEvent : IGameEvent
	{
		public int hour;
		public int minute;
		public int second;

		public ClockFreezeEvent(int _hour, int _minute, int _second)
		{
			hour = _hour;
			minute = _minute;
			second = _second;
		}
	} 
}