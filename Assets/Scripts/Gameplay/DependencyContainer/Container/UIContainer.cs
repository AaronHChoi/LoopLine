using Core.DependencyInjection;
using Gameplay.Inventory;
using UnityEngine;

namespace Gameplay.DependencyContainer
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