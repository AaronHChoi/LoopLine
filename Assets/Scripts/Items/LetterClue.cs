using UnityEngine;

public class LetterClue : ItemInteract
{
    [SerializeField] ItemDissolve item;

    protected override void Awake()
    {

    }
    public override void Start()
    {
        //base.Start();

        if (GameManager.Instance.GetCondition(GameCondition.LetterClue6))
        {
            gameObject.SetActive(false);
        }
    }
    public override bool Interact()
    {
        if (canBePicked)
        {
            item.TakePhoto();
            GameManager.Instance.SetCondition(GameCondition.LetterClue6, true);
            return true;
        }
        return false;
    }
}