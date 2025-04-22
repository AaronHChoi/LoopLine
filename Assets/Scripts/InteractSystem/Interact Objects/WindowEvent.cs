using UnityEngine;
using System.Collections.Generic;
using System;

public class WindowEvent : MonoBehaviour, IObserver
{
    public Subject EventManager;
    [SerializeField] private GameObject crystal;
    [SerializeField] private GameObject crystalBreakEffect;

    void Awake()
    {
        EventManager = FindFirstObjectByType<Subject>();
    }

    public void OnNotify(Events _event)
    {
        if (_event == Events.BreakCrystal)
            breakCrystal();
    }
    private void OnEnable()
    {
        EventManager.AddObserver(this);
    }
    private void OnDisable()
    {
        EventManager.RemoveObserver(this);
    }

    private void breakCrystal()
    {
        if (crystal != null)
        {
            crystal.SetActive(false);
        }
        crystalBreakEffect.SetActive(true);
    }
}
