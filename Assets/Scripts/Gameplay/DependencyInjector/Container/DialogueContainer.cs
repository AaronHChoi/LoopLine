using Core.DependencyInjection;

namespace DependencyInjection
{
	public class DialogueContainer : BaseContainer
	{
		public void RegisterServices(InterfaceDependencyInjector injector)
		{
			//injector.Register<IMonologueSpeaker>(() => FindAndValidate<MonologueSpeaker>());
			injector.Register<IDialogueManager>(() => FindAndValidate<DialogueManager>());
			injector.Register<INPCDialogueManager>(() => FindAndValidate<NPCDialogueManager>());
			injector.Register<IDialogueUI>(() => FindAndValidate<DialogueUI>());
		}
	} 
}