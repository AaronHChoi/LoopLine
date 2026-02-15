using Core.DependencyInjection;
using Core.Utilities;
using UnityEngine;

namespace DependencyInjection
{
    public class DependencyContainer : Singleton<DependencyContainer>
    {
        public PlayerContainer PlayerContainer { get; private set; } = new PlayerContainer();
        public UIContainer UIContainer { get; private set; } = new UIContainer();
        public GeneralContainer GeneralContainer { get; private set; } = new GeneralContainer();
        public CinemachineContainer CinemachineContainer { get; private set; } = new CinemachineContainer();
        public WalkmanContainer WalkmanContainer { get; private set; } = new WalkmanContainer();
        public PhotoContainer PhotoContainer { get; private set; } = new PhotoContainer();
        public ManagerContainer ManagerContainer { get; private set; } = new ManagerContainer();
        public DialogueContainer DialogueContainer { get; private set; } = new DialogueContainer();

        protected override void Awake()
        {
            base.Awake();

            RegisterAll();
        }
        private void RegisterAll()
        {
            var injector = InterfaceDependencyInjector.Instance;

            if (injector == null)
            {
                Debug.LogError("[DependencyContainer] No se encontró el Inyector en la escena.");
                return;
            }

            injector.ClearInstances();

            PlayerContainer.RegisterServices(injector);
            UIContainer.RegisterServices(injector);
            GeneralContainer.RegisterServices(injector);
            CinemachineContainer.RegisterServices(injector);
            PhotoContainer.RegisterServices(injector);
            WalkmanContainer.RegisterServices(injector);
            ManagerContainer.RegisterServices(injector);
            DialogueContainer.RegisterServices(injector);
        }
    }
}