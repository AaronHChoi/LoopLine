using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerBoard : MonoBehaviour, IInteract
{
    [SerializeField] GameObject Photo;

    [SerializeField] List<GameObject> clues;
    [SerializeField] float delayBetween = 1f;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            CheckTest();
        }
    }
    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        CheckTest();
    }
    private void CheckTest()
    {
        if (GameManager.Instance.CameraGirlPhoto)
        {
            Photo.SetActive(true);
            StartCoroutine(ActiveInSequence(clues, delayBetween));
        }
    }
    IEnumerator ActiveInSequence(List<GameObject> objects, float delay)
    {
        foreach (GameObject obj in objects)
        {
            yield return new WaitForSeconds(delay);
            if (obj != null)
                obj.SetActive(true);
        }
    }
}
