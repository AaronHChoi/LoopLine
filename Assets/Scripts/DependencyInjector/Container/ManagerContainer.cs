namespace DependencyInjection
{
    public class ManagerContainer : BaseContainer
    {
        GameManager gameManager;
        public GameManager GameManager => gameManager ??= FindAndValidate<GameManager>();

        DevelopmentManager developmentManager;
        public DevelopmentManager DevelopmentManager => developmentManager ??= FindAndValidate<DevelopmentManager>();

        SoundManager soundManager;
        public SoundManager SoundManager => soundManager ??= FindAndValidate<SoundManager>();

        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<IDialogueManager>(() => FindAndValidate<DialogueManager>());
            injector.Register<IScreenManager>(() => FindAndValidate<ScreenManager>());
            injector.Register<IGameSceneManager>(() => FindAndValidate<GameSceneManager>());
            injector.Register<ITimeProvider>(() => FindAndValidate<TimeManager>());
            injector.Register<IEventManager>(() => FindAndValidate<EventManager>());
            injector.Register<IItemManager>(() => FindAndValidate<ItemManager>());
            injector.Register<IBlackRoomManager>(() => FindAndValidate<BlackRoomManager>());
            injector.Register<ISceneTransitionController>(() => FindAndValidate<SceneTransitionController>());
            injector.Register<IPauseMenuManager>(() => FindAndValidate<PauseMenuManager>());
            injector.Register<IPhotoQuestManager>(() => FindAndValidate<PhotoQuestManager>());
            injector.Register<IClockPuzzleManager>(() => FindAndValidate<ClockPuzzleManager>());
        }
    }
}