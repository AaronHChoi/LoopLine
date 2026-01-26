using Core.DependencyInjection;
using Core.Utilities;

namespace DependencyInjection
{
    public class DependencyContainer : Singleton<DependencyContainer>
    {
        public PlayerContainer PlayerContainer { get; private set; } = new PlayerContainer();
        public UIContainer UIContainer { get; private set; } = new UIContainer();
        public GeneralContainer GeneralContainer { get; private set; } = new GeneralContainer();
        public CinemachineContainer CinemachineContainer { get; private set; } = new CinemachineContainer();
        public PhotoContainer PhotoContainer { get; private set; } = new PhotoContainer();
        public ManagerContainer ManagerContainer { get; private set; } = new ManagerContainer();
        public DialogueContainer DialogueContainer { get; private set; } = new DialogueContainer();
        protected override void Awake()
        {
            base.Awake();

            var injector = InterfaceDependencyInjector.Instance;

            PlayerContainer.RegisterServices(injector);
            UIContainer.RegisterServices(injector);
            GeneralContainer.RegisterServices(injector);
            CinemachineContainer.RegisterServices(injector);
            PhotoContainer.RegisterServices(injector);
            ManagerContainer.RegisterServices(injector);
            DialogueContainer.RegisterServices(injector);
        }
    }
}