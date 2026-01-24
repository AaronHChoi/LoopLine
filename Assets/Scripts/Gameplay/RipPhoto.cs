using UnityEngine;

public class RipPhoto : MonoBehaviour
{
    [SerializeField] GameObject phto;
    [SerializeField] DissolveControllerScript photo;
    public void DeactivatePhoto()
    {
        //phto.SetActive(false);
        photo.ActivateDissolve();
        GameManager.Instance.SetCondition(GameCondition.PhotosClue1, true);
    }
}