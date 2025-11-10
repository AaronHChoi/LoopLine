
public class PolaroidItem : ItemInteract
{
    public override void Start()
    {
        base.Start();
    }
    public override bool Interact()
    {
        if (canBePicked)
        {
            GameManager.Instance.HasCamera = true;
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}