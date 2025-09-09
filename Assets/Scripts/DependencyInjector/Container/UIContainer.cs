namespace DependencyInjection
{
    public class UIContainer : BaseContainer
    {
        UIManager uiManager;
        public UIManager UIManager => uiManager ??= FindAndValidate<UIManager>();

        DialogueUI dialogueUI;
        public DialogueUI DialogueUI => dialogueUI ??= FindAndValidate<DialogueUI>();

        InventoryUI inventoryUI;
        public InventoryUI InventoryUI => inventoryUI ??= FindAndValidate<InventoryUI>();

        CrosshairFade crosshairFade;
        public CrosshairFade CrosshairFade => crosshairFade ??= FindAndValidate<CrosshairFade>();

        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<IUIManager>(() => UIManager);
            injector.Register<ICrosshairFade>(() => CrosshairFade);
            //injector.Register<IInventoryUI>(() => InventoryUI);
        }
    }
}