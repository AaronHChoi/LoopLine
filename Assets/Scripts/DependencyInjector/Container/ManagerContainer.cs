namespace DependencyInjection
{
    public class ManagerContainer : BaseContainer
    {
        public GameManager GameManager { get; private set; }
        public DialogueManager DialogueManager { get; private set; }
        public TimeManager TimeManager { get; private set; }
        public GameSceneManager GameSceneManager { get; private set; }
        public DevelopmentManager DevelopmentManager { get; private set; }
        public QuestionManager QuestionManager { get; private set; }
        public EventManager EventManager { get; private set; }
        public SoundManager SoundManager { get; private set; }
        public NoteBookManager NoteBookManager { get; private set; }
        public ItemManager ItemManager { get; private set; }
        public FocusModeManager FocusModeManager { get; private set; }
        public EventDialogueManager EventDialogueManager { get; private set; }
        public CrosshairFade CrosshairFade { get; private set; }
        public void Initialize()
        {
            GameManager = FindAndValidate<GameManager>();
            DialogueManager = FindAndValidate<DialogueManager>();
            TimeManager = FindAndValidate<TimeManager>();
            GameSceneManager = FindAndValidate<GameSceneManager>();
            DevelopmentManager = FindAndValidate<DevelopmentManager>();
            QuestionManager = FindAndValidate<QuestionManager>();
            EventManager = FindAndValidate<EventManager>();
            SoundManager = FindAndValidate<SoundManager>();
            NoteBookManager = FindAndValidate<NoteBookManager>();
            ItemManager = FindAndValidate<ItemManager>();
            FocusModeManager = FindAndValidate<FocusModeManager>();
            EventDialogueManager = FindAndValidate<EventDialogueManager>();
            CrosshairFade = FindAndValidate<CrosshairFade>();
        }
        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<IColliderToggle>(FocusModeManager);
            injector.Register<INoteBookColliderToggle>(NoteBookManager);
            injector.Register<IDialogueManager>(DialogueManager);
            injector.Register<ITimeProvider>(TimeManager);
            injector.Register<ICrosshairFade>(CrosshairFade);
            //injector.Register<IEventDialogueManager>(EventDialogueManager); //DIALOGUE EVENTS
            //injector.Register<IEventManager>(EventManager); //EVENTS
            //injector.Register<IGameSceneManager>(GameSceneManager);
            //injector.Register<IItemManager>(ItemManager);
            //injector.Register<IEventStopTrain>(EventManager);
        }
    }
}