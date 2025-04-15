using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> clipList = new List<AudioClip>();
    [SerializeField] private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.LoopTime <= 240 && GameManager.Instance.LoopTime >= 235)
        {
            audioSource.PlayOneShot(clipList[0]);
        }
        else
        {
            audioSource.Stop();
        }
        if (GameManager.Instance.LoopTime <= 180 && GameManager.Instance.LoopTime >= 175)
        {
            audioSource.PlayOneShot(clipList[1]);
        }
        else
        {
            audioSource.Stop();
        }
    }
}
