
public class RockClue : ItemInteract
{
    protected override void Awake()
    {

    }
    public override void Start()
    {
        base.Start();

        if (GameManager.Instance.GetCondition(GameCondition.RockClue4))
        {
            gameObject.SetActive(false);
        }
    }
    public override bool Interact()
    {
        if (canBePicked)
        {
            GameManager.Instance.SetCondition(GameCondition.RockClue4, true);
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}