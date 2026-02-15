using System.Collections;
using UnityEngine;
using Core.Audio;
using Core.DependencyInjection;
using Core.Data;

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

    IMonologueSpeaker monologueSpeaker;
    ISoundManager soundManager;

    private void Awake()
    {
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
    }
    private void Start()
    {
        coroutineAllowed = true;
        indexShown = 0;
    }
    public void Interact()
    {
        if (!coroutineAllowed)
        {
            return;
        }

       
        StartCoroutine(Rotate());
        

    }
    private IEnumerator Rotate()
    {
        coroutineAllowed = false;

        indexShown++;

        if (indexShown > 3)
        {
            indexShown = 0;
        }

        //switch (indexShown)
        //{
        //    case 0:
        //        soundManager.CreateSound()
        //          .WithSoundData(_do)
        //          .WithSoundPosition(transform.position)
        //          .Play();
        //        break;
        //    case 1:
        //        soundManager.CreateSound()
        //          .WithSoundData(_re)
        //          .WithSoundPosition(transform.position)
        //          .Play();
        //        break;
        //    case 2:
        //        soundManager.CreateSound()
        //          .WithSoundData(_mi)
        //          .WithSoundPosition(transform.position)
        //          .Play();
        //        break;
        //    case 3:
        //        soundManager.CreateSound()
        //          .WithSoundData(_sol)
        //          .WithSoundPosition(transform.position)
        //          .Play();
        //        break;
        //    default:
        //        break;
        //}

        for (int i = 0; i < 36; i++)
        {
            transform.Rotate(0, 1, 0);
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