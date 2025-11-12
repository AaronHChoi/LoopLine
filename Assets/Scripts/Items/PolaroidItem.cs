
public class PolaroidItem : ItemInteract
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
            GameManager.Instance.HasCamera = true;
            GameManager.Instance.SetCondition(GameCondition.PolaroidTaken, true);
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}