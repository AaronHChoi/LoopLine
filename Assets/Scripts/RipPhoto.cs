using UnityEngine;

public class RipPhoto : MonoBehaviour
{
    [SerializeField] GameObject phto;
    public void DeactivatePhoto()
    {
        phto.SetActive(false);
        GameManager.Instance.SetCondition(GameCondition.PhotosClue1, true);
    }
}