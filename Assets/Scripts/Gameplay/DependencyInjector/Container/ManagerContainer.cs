using Core.DependencyInjection;

namespace DependencyInjection
{
    public class ManagerContainer : BaseContainer
    {
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
            injector.Register<ISafeQuestManager>(() => FindAndValidate<SafeQuestManager>());
            injector.Register<IFinalQuestManager>(() => FindAndValidate<FinalQuestManager>());
        }
    }
}