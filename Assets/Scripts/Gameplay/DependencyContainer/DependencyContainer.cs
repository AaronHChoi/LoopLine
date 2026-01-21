using Core.DependencyInjection;
using UnityEngine;

namespace Gameplay.DependencyContainer
{
    public class DependencyContainer : MonoBehaviour
    {
        public static DependencyContainer Instance { get; private set; }

        public PlayerContainer PlayerContainer { get; private set; } = new PlayerContainer();
        public UIContainer UIContainer { get; private set; } = new UIContainer();
        public GeneralContainer GeneralContainer { get; private set; } = new GeneralContainer();
        public CinemachineContainer CinemachineContainer { get; private set; } = new CinemachineContainer();
        public PhotoContainer PhotoContainer { get; private set; } = new PhotoContainer();
        public ManagerContainer ManagerContainer { get; private set; } = new ManagerContainer();
        public DialogueContainer DialogueContainer { get; private set; } = new DialogueContainer();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        private void Start()
        {
            var injector = InterfaceDependencyInjector.Instance;

            if (injector != null)
            {
                RegisterAllServices(injector);
            }
        }
        private void RegisterAllServices(InterfaceDependencyInjector injector)
        {
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