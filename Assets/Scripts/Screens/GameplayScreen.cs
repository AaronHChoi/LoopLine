using System.Collections.Generic;
using UnityEngine;

public class GameplayScreen : MonoBehaviour, IScreen
{
    private Dictionary<Behaviour, bool> dictionaryData = new Dictionary<Behaviour, bool>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
            GameManager.Instance.ScreenManager.Push(EnumScreenName.Pause);
    }
    public void Activate()
    {
        gameObject.SetActive(true);

        foreach (var behaviour in dictionaryData)
            behaviour.Key.enabled = behaviour.Value;
    }
    public void Deactivate()
    {
        foreach (var behaviour in GetComponentsInChildren<Behaviour>())
        {
            dictionaryData[behaviour] = behaviour.enabled;
            behaviour.enabled = false;
        }
    }
    public void Free()
    {
        gameObject.SetActive(false);
    }
}
