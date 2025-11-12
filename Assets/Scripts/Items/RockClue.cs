
public class RockClue : ItemInteract
{
    protected override void Awake()
    {

    }
    public override void Start()
    {
        base.Start();

        if (GameManager.Instance.GetCondition(GameCondition.RockClue))
        {
            gameObject.SetActive(false);
        }
    }
    public override bool Interact()
    {
        if (canBePicked)
        {
            GameManager.Instance.SetCondition(GameCondition.RockClue, true);
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}