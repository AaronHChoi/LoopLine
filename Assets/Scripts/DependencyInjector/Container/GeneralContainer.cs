namespace DependencyInjection
{
    public class GeneralContainer : BaseContainer
    {
        ItemInteract itemInteract;
        public ItemInteract ItemInteract => itemInteract ??= FindAndValidate<ItemInteract>();

        Parallax parallax;
        public Parallax Parallax => parallax ??= FindAndValidate<Parallax>();

    }
}