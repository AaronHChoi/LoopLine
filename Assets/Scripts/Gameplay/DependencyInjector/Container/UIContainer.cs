using Core.DependencyInjection;

namespace DependencyInjection
{
    public class UIContainer : BaseContainer
    {
        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            //injector.Register<IDialogueUI>(() => FindAndValidate<DialogueUI>());
            injector.Register<IInventoryUI>(() => FindAndValidate<InventoryUI>());
            injector.Register<IUIManager>(() => FindAndValidate<UIManager>());
            injector.Register<ICrosshairFade>(() => FindAndValidate<CrosshairFadeController>());
        }
    }
}