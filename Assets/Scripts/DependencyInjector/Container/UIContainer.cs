namespace DependencyInjection
{
    public class UIContainer : BaseContainer
    {
        UIManager uiManager;
        public UIManager UIManager => uiManager ??= FindAndValidate<UIManager>();

        DialogueUI dialogueUI;
        public DialogueUI DialogueUI => dialogueUI ??= FindAndValidate<DialogueUI>();

        //InventoryUI inventoryUI;
        //public InventoryUI InventoryUI => inventoryUI ??= FindAndValidate<InventoryUI>();

        CrosshairFadeController crosshairFade;
        public CrosshairFadeController CrosshairFade => crosshairFade ??= FindAndValidate<CrosshairFadeController>();

        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<IInventoryUI>(() => FindAndValidate<InventoryUI>());
            injector.Register<IUIManager>(() => UIManager);
            injector.Register<ICrosshairFade>(() => CrosshairFade);
            //injector.Register<IInventoryUI>(() => InventoryUI);
        }
    }
}