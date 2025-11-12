
public class LetterClue : ItemInteract
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
            GameManager.Instance.SetCondition(GameCondition.LetterClue, true);
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}