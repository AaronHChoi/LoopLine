using System.Collections;
using UnityEngine;

public class Dial : MonoBehaviour, IInteract
{
    [SerializeField] GameCondition conditionToRotate;
    private bool coroutineAllowed = false;
    private int indexShown = 0;
    public static event System.Action<string, int> OnDialRotated = delegate { };


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

        for (int i = 0; i < 90; i++)
        {
            transform.Rotate(0, 1, 0);
            yield return new WaitForSeconds(0.01f);
        }

        coroutineAllowed = true;

        indexShown++;

        if (indexShown > 3)
        {
            indexShown = 0;
        }

        OnDialRotated(name, indexShown);
    }


    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }
}
