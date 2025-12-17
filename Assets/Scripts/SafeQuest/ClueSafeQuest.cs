using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueSafeQuest : MonoBehaviour
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

    [ContextMenu("Test")]
    public void PlaySequence()
    {
        StartCoroutine(SequenceRoutine());
    }
    private IEnumerator SequenceRoutine()
    {
        SoundManager.Instance.CreateSound()
                 .WithSoundData(_do)
                 .WithSoundPosition(transform.position)
                 .Play();
        yield return StartCoroutine(FlashGroupMaterial(blueNotes, blueOnMaterial));
        yield return new WaitForSeconds(delayBetweenColors);

        SoundManager.Instance.CreateSound()
                 .WithSoundData(_re)
                 .WithSoundPosition(transform.position)
                 .Play();
        yield return StartCoroutine(FlashGroupMaterial(yellowNotes, yellowOnMaterial));
        yield return new WaitForSeconds(delayBetweenColors);

        SoundManager.Instance.CreateSound()
                 .WithSoundData(_mi)
                 .WithSoundPosition(transform.position)
                 .Play();
        yield return StartCoroutine(FlashGroupMaterial(greenNotes, greenOnMaterial));
        yield return new WaitForSeconds(delayBetweenColors);

        SoundManager.Instance.CreateSound()
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