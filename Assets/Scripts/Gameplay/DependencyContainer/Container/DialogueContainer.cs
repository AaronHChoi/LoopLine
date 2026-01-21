using Core.DependencyInjection;

namespace Gameplay.DependencyContainer
{
	public class DialogueContainer : BaseContainer
	{
		public void RegisterServices(InterfaceDependencyInjector injector)
		{
			injector.Register<IMonologueSpeaker>(() => FindAndValidate<MonologueSpeaker>());
			injector.Register<IDialogueManager>(() => FindAndValidate<DialogueManager>());
			injector.Register<INPCDialogueManager>(() => FindAndValidate<NPCDialogueManager>());
		}
	} 
}