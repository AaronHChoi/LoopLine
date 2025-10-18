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
            injector.Register<IGameSceneManager>(() => FindAndValidate<GameSceneManager>());
            injector.Register<ITimeProvider>(() => FindAndValidate<TimeManager>());
            injector.Register<IQuestionManager>(() => FindAndValidate<QuestionManager>());
            injector.Register<IEventManager>(() => FindAndValidate<EventManager>());
            injector.Register<IItemManager>(() => FindAndValidate<ItemManager>());
            injector.Register<IBlackRoomManager>(() => FindAndValidate<BlackRoomManager>());
        }
    }
}