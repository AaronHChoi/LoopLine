
public class PhotosClue : ItemInteract
{
    protected override void Awake()
    {

    }
    public override void Start()
    {
        base.Start();
    }
    public override bool Interact()
    {
        if (canBePicked)
        {
            GameManager.Instance.SetCondition(GameCondition.PhotosClue, true);
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}