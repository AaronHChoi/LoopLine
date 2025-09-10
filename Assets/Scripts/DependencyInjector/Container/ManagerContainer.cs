namespace DependencyInjection
{
    public class ManagerContainer : BaseContainer
    {
        GameManager gameManager;
        public GameManager GameManager => gameManager ??= FindAndValidate<GameManager>();

        DialogueManager dialogueManager;
        public DialogueManager DialogueManager => dialogueManager ??= FindAndValidate<DialogueManager>();

        TimeManager timeManager;
        public TimeManager TimeManager => timeManager ??= FindAndValidate<TimeManager>();

        GameSceneManager gameSceneManager;
        public GameSceneManager GameSceneManager => gameSceneManager ??= FindAndValidate<GameSceneManager>();

        DevelopmentManager developmentManager;
        public DevelopmentManager DevelopmentManager => developmentManager ??= FindAndValidate<DevelopmentManager>();

        QuestionManager questionManager;
        public QuestionManager QuestionManager => questionManager ??= FindAndValidate<QuestionManager>();

        EventManager eventManager;
        public EventManager EventManager => eventManager ??= FindAndValidate<EventManager>();

        SoundManager soundManager;
        public SoundManager SoundManager => soundManager ??= FindAndValidate<SoundManager>();

        ItemManager itemManager;
        public ItemManager ItemManager => itemManager ??= FindAndValidate<ItemManager>();

        EventDialogueManager eventDialogueManager;
        public EventDialogueManager EventDialogueManager => eventDialogueManager ??= FindAndValidate<EventDialogueManager>();

        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<IDialogueManager>(() => DialogueManager);
            injector.Register<ITimeProvider>(() => TimeManager);
            //injector.Register<IEventDialogueManager>(() => EventDialogueManager); //DIALOGUE EVENTS
            //injector.Register<IEventManager>(() => EventManager); //EVENTS
            //injector.Register<IGameSceneManager>(() => GameSceneManager);
            //injector.Register<IItemManager>(() => ItemManager);
            //injector.Register<IEventStopTrain>(() => EventManager);
        }
    }
}