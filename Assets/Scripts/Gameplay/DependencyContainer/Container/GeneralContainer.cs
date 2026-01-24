using Core.DependencyInjection;

namespace Gameplay.DependencyInjection
{
    public class GeneralContainer : BaseContainer
    {
        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<IGameStateController>(() => FindAndValidate<GameStateController>());
            injector.Register<IClock>(() => FindAndValidate<Clock>());
            injector.Register<IPolaraidItem>(() => FindAndValidate<PolaroidItem>());
            injector.Register<IGearRotator>(() => FindAndValidate<GearRotator>());
            injector.Register<ISceneWeightController>(() => FindAndValidate<SceneWeightController>());
            injector.Register<ICinematicManager>(() => FindAndValidate<CinematicManager>());
            injector.Register<IClueSafeQuest>(() => FindAndValidate<ClueSafeQuest>());
        }
    }
}