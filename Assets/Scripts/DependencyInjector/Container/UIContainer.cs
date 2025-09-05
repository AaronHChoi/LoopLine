namespace DependencyInjection
{
    public class UIContainer : BaseContainer
    {
        public UIManager UIManager { get; private set; }
        public DialogueUI DialogueUI { get; private set; }
        public InventoryUI InventoryUI { get; private set; }
        public void Initialize()
        {
            UIManager = FindAndValidate<UIManager>();
            DialogueUI = FindAndValidate<DialogueUI>();
            InventoryUI = FindAndValidate<InventoryUI>();
        }
        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<IUIManager>(UIManager);
            //injector.Register<IInventoryUI>(InventoryUI);
        }
    }
}