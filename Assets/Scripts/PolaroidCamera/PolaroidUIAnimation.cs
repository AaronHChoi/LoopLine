using UnityEngine;

public class PolaroidUIAnimation : MonoBehaviour, IPolaroidUIAnimation
{
    [SerializeField] Animator top;
    [SerializeField] Animator bottom;

    public void PhotoUIAnimation()
    {
        top.SetTrigger("TakePhotoTop");
        bottom.SetTrigger("TakePhotoBottom");
    }
}
public interface IPolaroidUIAnimation
{
    void PhotoUIAnimation();
}