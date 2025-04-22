using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Subject
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] TimeManager timeManager;

    [Header("Train Event 1")]
    [SerializeField] private AudioClip trainStopSound_1;
    [SerializeField] private AudioClip trainStopSound_2;

    [Header("Train Event 2")]
    [SerializeField] private AudioClip crystalBreakSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        timeManager = FindFirstObjectByType<TimeManager>();
    }
    void Update()
    {
        TrainEvent1();
        TrainEvent2();
    }
    private void TrainEvent1()
    {
        if (timeManager.LoopTime <= 240 && timeManager.LoopTime >= 235)
        {
            NotifyObservers(Events.StopTrain);
            audioSource.PlayOneShot(trainStopSound_1);
        }
        else
        {
            audioSource.Stop();
        }
        if (timeManager.LoopTime <= 180 && timeManager.LoopTime >= 175)
        {
            NotifyObservers(Events.ResumeTrain);
            audioSource.PlayOneShot(trainStopSound_2);
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void TrainEvent2()
    {
        if (timeManager.LoopTime <= 60 && timeManager.LoopTime >= 55)
        {
            audioSource.PlayOneShot(crystalBreakSound);
            NotifyObservers(Events.BreakCrystal);
        }
        else
        {
            audioSource.Stop();
        }
    }
}
