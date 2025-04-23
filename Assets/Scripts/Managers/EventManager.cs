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

    private bool isWindowBroken = false;
    bool trainEvent1 = true;

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
            if (!audioSource.isPlaying)
            {
                audioSource.clip = trainStopSound_1;
                audioSource.Play();
            }
        }
        else
        {
            //audioSource.Stop();
        }
        if (timeManager.LoopTime <= 180 && timeManager.LoopTime >= 175)
        {
            NotifyObservers(Events.ResumeTrain);
            if (!audioSource.isPlaying)
            {
                audioSource.clip = trainStopSound_2;
                audioSource.Play();
            }
        }
        else
        {
            //audioSource.Stop();
        }
    }

    private void TrainEvent2()
    {
        if (!isWindowBroken && timeManager.LoopTime <= 60 && timeManager.LoopTime >= 55)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = crystalBreakSound;
                audioSource.Play();
            }
            NotifyObservers(Events.BreakCrystal);
            isWindowBroken = true;
        }
        else
        {
            //audioSource.Stop();
        }
    }
}
