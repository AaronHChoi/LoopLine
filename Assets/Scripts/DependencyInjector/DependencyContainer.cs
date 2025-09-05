using UnityEngine;

namespace DependencyInjection
{
    public class DependencyContainer : MonoBehaviour
    {
        public static DependencyContainer Instance { get; private set; }

        public PlayerContainer PlayerContainer { get; private set; }
        public UIContainer UIContainer { get; private set; }
        public GeneralContainer GeneralContainer { get; private set; }
        public CinemachineContainer CinemachineContainer { get; private set; }
        public PhotoContainer PhotoContainer { get; private set; }
        public ManagerContainer ManagerContainer { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            InitializeDependencies();
        }
        private void InitializeDependencies()
        {
            PlayerContainer = new PlayerContainer();
            UIContainer = new UIContainer();
            GeneralContainer = new GeneralContainer();
            CinemachineContainer = new CinemachineContainer();
            PhotoContainer = new PhotoContainer();
            ManagerContainer = new ManagerContainer();

            PlayerContainer.Initialize();
            UIContainer.Initialize();
            GeneralContainer.Initialize();
            CinemachineContainer.Initialize();
            PhotoContainer.Initialize();
            ManagerContainer.Initialize();
        }
    }
}