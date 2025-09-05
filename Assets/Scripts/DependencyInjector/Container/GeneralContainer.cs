namespace DependencyInjection
{
    public class GeneralContainer : BaseContainer
    {
        public ItemInteract ItemInteract { get; private set; }
        public Parallax Parallax { get; private set; }
        public void Initialize()
        {
            Parallax = FindAndValidate<Parallax>();
            ItemInteract = FindAndValidate<ItemInteract>();
        }
    }
}