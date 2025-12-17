namespace DependencyInjection
{
    public class GeneralContainer : BaseContainer
    {
        ItemInteract itemInteract;
        public ItemInteract ItemInteract => itemInteract ??= FindAndValidate<ItemInteract>();

        Parallax parallax;
        public Parallax Parallax => parallax ??= FindAndValidate<Parallax>();

        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<IGameStateController>(() => FindAndValidate<GameStateController>());
            injector.Register<IClock>(() => FindAndValidate<Clock>());
            injector.Register<IPolaraidItem>(() => FindAndValidate<PolaroidItem>());
            injector.Register<IGearRotator>(() => FindAndValidate<GearRotator>());
            injector.Register<ISceneWeightController>(() => FindAndValidate<SceneWeightController>());
        }
    }
}