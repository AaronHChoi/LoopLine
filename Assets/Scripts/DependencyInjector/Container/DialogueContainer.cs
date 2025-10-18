
namespace DependencyInjection
{
	public class DialogueContainer : BaseContainer
	{
		public void RegisterServices(InterfaceDependencyInjector injector)
		{
			injector.Register<IMonologueSpeaker>(() => FindAndValidate<MonologueSpeaker>());
			injector.Register<IDialogueManager2>(() => FindAndValidate<DialogueManager2>());
			injector.Register<INPCDialogueManager>(() => FindAndValidate<NPCDialogueManager>());
		}
	} 
}