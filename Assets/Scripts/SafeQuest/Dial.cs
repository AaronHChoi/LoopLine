using System.Collections;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Dial : MonoBehaviour, IInteract
{
    [SerializeField] GameCondition conditionToRotate;
    private bool coroutineAllowed = false;
    private int indexShown = 0;
    public static event System.Action<string, int> OnDialRotated = delegate { };

    [SerializeField] SoundData _do;
    [SerializeField] SoundData _re;
    [SerializeField] SoundData _mi;
    [SerializeField] SoundData _sol;


    private void Start()
    {
        coroutineAllowed = true;
        indexShown = 0;
    }
    public void Interact()
    {
        if (coroutineAllowed && GameManager.Instance.GetCondition(conditionToRotate))
        {
            StartCoroutine(Rotate());
        }
    }
    private IEnumerator Rotate()
    {
        coroutineAllowed = false;

        indexShown++;

        if (indexShown > 3)
        {
            indexShown = 0;
        }

        switch (indexShown)
        {
            case 0:
                SoundManager.Instance.CreateSound()
                  .WithSoundData(_do)
                  .WithSoundPosition(transform.position)
                  .Play();
                break;
            case 1:
                SoundManager.Instance.CreateSound()
                  .WithSoundData(_re)
                  .WithSoundPosition(transform.position)
                  .Play();
                break;
            case 2:
                SoundManager.Instance.CreateSound()
                  .WithSoundData(_mi)
                  .WithSoundPosition(transform.position)
                  .Play();
                break;
            case 3:
                SoundManager.Instance.CreateSound()
                  .WithSoundData(_sol)
                  .WithSoundPosition(transform.position)
                  .Play();
                break;
            default:
                break;
        }

        for (int i = 0; i < 90; i++)
        {
            transform.Rotate(0, 0, 1);
            yield return new WaitForSeconds(0.01f);
        }

        coroutineAllowed = true;

        OnDialRotated(name, indexShown);
    }
    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }
}