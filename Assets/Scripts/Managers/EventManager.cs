using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Subject
{
    [SerializeField] private AudioSource audioSource;

    [Header("Train Event 1")]
    [SerializeField] private AudioClip trainStopSound_1;
    [SerializeField] private AudioClip trainStopSound_2;

    [Header("Train Event 2")]
    [SerializeField] private AudioClip crystalBreakSound;
    [SerializeField] private GameObject crystal;
    [SerializeField] private GameObject crystalBreakEffect;

    [SerializeField] TimeManager timeManager;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        timeManager = FindFirstObjectByType<TimeManager>();
    }
    void Update()
    {
        TrainEvent1();
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
            crystal.SetActive(false);
            crystalBreakEffect.SetActive(true);
        }
        else
        {
            audioSource.Stop();
        }
    }
}
