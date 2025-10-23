using System.Collections.Generic;
using UnityEngine;

public class GamePlayScreen : MonoBehaviour, IScreen 
{
    private Dictionary<Behaviour, bool> _dictionaryData = new Dictionary<Behaviour, bool>();
    public void Activate()
    {
        gameObject.SetActive(true);

        //foreach (var behaviour in _dictionaryData) behaviour.Key.enabled = behaviour.Value;
    }

    public void Deactivate()
    {
        //foreach (var behaviour in GetComponentsInChildren<Behaviour>())
        //{
        //    _dictionaryData[behaviour] = behaviour.enabled;
        //    behaviour.enabled = false;
        //}
    }

    public void Free()
    {
        gameObject.SetActive(false);
    }
}

