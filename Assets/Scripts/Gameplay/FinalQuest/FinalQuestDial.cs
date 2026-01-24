using UnityEngine;
using System.Collections;

public class FinalQuestDial : MonoBehaviour, IInteract
{
    [SerializeField] GameCondition conditionToRotate;
    private bool coroutineAllowed = false;
    private int indexShown = 0;
    public static event System.Action<string, int> OnDialRotated = delegate { };


    private void Start()
    {
        coroutineAllowed = true;
        indexShown = 0;
        SetFinalChild(indexShown);
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

        int currentIndex = indexShown;
        int nextIndex = indexShown + 1;

        if (nextIndex >= transform.childCount)
            nextIndex = 0;

        SetTransitionChildren(currentIndex, nextIndex);

        for (int i = 0; i < 90; i++)
        {
            transform.Rotate(1, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }

        indexShown = nextIndex;
        SetFinalChild(indexShown);

        OnDialRotated(name, indexShown);

        coroutineAllowed = true;
    }
    private void SetTransitionChildren(int currentIndex, int nextIndex)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bool shouldBeActive = (i == currentIndex || i == nextIndex);
            transform.GetChild(i).gameObject.SetActive(shouldBeActive);
        }
    }
    private void SetFinalChild(int index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == index);
        }
    }
    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }
}
