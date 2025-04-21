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

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        TrainEvent1();
    }
    private void TrainEvent1()
    {
        if (GameManager.Instance.LoopTime <= 240 && GameManager.Instance.LoopTime >= 235)
        {
            NotifyObservers(Events.StopTrain);
            audioSource.PlayOneShot(trainStopSound_1);
        }
        else
        {
            audioSource.Stop();
        }
        if (GameManager.Instance.LoopTime <= 180 && GameManager.Instance.LoopTime >= 175)
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
        if (GameManager.Instance.LoopTime <= 60 && GameManager.Instance.LoopTime >= 55)
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
