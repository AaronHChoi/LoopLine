using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Audio;
using Core.DependencyInjection;

public class ClueSafeQuest : MonoBehaviour, IClueSafeQuest
{
    [SerializeField] List<GameObject> blueNotes = new List<GameObject>();
    [SerializeField] List<GameObject> yellowNotes = new List<GameObject>();
    [SerializeField] List<GameObject> greenNotes = new List<GameObject>();
    [SerializeField] List<GameObject> redNotes = new List<GameObject>();

    [SerializeField] Material blueOnMaterial;
    [SerializeField] Material yellowOnMaterial;
    [SerializeField] Material greenOnMaterial;
    [SerializeField] Material redOnMaterial;

    [SerializeField] float lightDuration = 0.5f;
    [SerializeField] float delayBetweenColors = 0.25f;

    [SerializeField] SoundData _do;
    [SerializeField] SoundData _re;
    [SerializeField] SoundData _mi;
    [SerializeField] SoundData _sol;

    ISoundManager soundManager;

    private void Awake()
    {
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
    }
    [ContextMenu("Test")]
    public void PlaySequence()
    {
        StartCoroutine(SequenceRoutine());
    }
    private IEnumerator SequenceRoutine()
    {
        soundManager.CreateSound()
                 .WithSoundData(_do)
                 .WithSoundPosition(transform.position)
                 .Play();
        yield return StartCoroutine(FlashGroupMaterial(blueNotes, blueOnMaterial));
        yield return new WaitForSeconds(delayBetweenColors);

        soundManager.CreateSound()
                 .WithSoundData(_re)
                 .WithSoundPosition(transform.position)
                 .Play();
        yield return StartCoroutine(FlashGroupMaterial(yellowNotes, yellowOnMaterial));
        yield return new WaitForSeconds(delayBetweenColors);

        soundManager.CreateSound()
                 .WithSoundData(_mi)
                 .WithSoundPosition(transform.position)
                 .Play();
        yield return StartCoroutine(FlashGroupMaterial(greenNotes, greenOnMaterial));
        yield return new WaitForSeconds(delayBetweenColors);

        soundManager.CreateSound()
                .WithSoundData(_sol)
                .WithSoundPosition(transform.position)
                .Play();
        yield return StartCoroutine(FlashGroupMaterial(redNotes, redOnMaterial));
       
    }
    private IEnumerator FlashGroupMaterial(List<GameObject> notes, Material matOn)
    {
        Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>();

        foreach (var note in notes)
        {
            Renderer rend = note.GetComponent<Renderer>();
            if (rend != null)
            {
                originalMaterials[note] = rend.material;

                rend.material = matOn;
            }
        }

        yield return new WaitForSeconds(lightDuration);

        foreach (var note in notes)
        {
            Renderer rend = note.GetComponent<Renderer>();
            if (rend != null && originalMaterials.ContainsKey(note))
            {
                rend.material = originalMaterials[note];
            }
        }
    }
}
public interface IClueSafeQuest
{
    void PlaySequence();
}