using UnityEngine;

public class CameraClue : ItemInteract
{
    protected override void Awake()
    {

    }
    public override void Start()
    {
        base.Start();

        if (GameManager.Instance.GetCondition(GameCondition.CameraGirlClue))
        {
            gameObject.SetActive(false);
        }
    }
    public override bool Interact()
    {
        if (canBePicked)
        {
            GameManager.Instance.SetCondition(GameCondition.CameraGirlClue, true);
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}
