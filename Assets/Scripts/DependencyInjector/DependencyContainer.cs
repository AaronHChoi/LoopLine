using UnityEngine;

namespace DependencyInjection
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
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            RegisterAll();
        }
        private void RegisterAll()
        {
            var injector = InterfaceDependencyInjector.Instance;

            new PlayerContainer().RegisterServices(injector);

        }
    }
}