
namespace DependencyInjection
{
	public class DialogueContainer : BaseContainer
	{
		public void RegisterServices(InterfaceDependencyInjector injector)
		{
			injector.Register<IMonologueSpeaker>(() => FindAndValidate<MonologueSpeaker>());
		}
	} 
}