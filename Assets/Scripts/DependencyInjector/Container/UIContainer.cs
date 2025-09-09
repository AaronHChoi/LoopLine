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

        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<IUIManager>(UIManager);
            //injector.Register<IInventoryUI>(InventoryUI);
        }
    }
}