using System;
using System.Collections;
using UnityEngine;

public class DelayUtility : Singleton<DelayUtility>
{
    public void Delay(float seconds, Action action)
    {
        StartCoroutine(DelayCoroutine(seconds, action));
    }
    private IEnumerator DelayCoroutine(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
    public void WaitUntil(Func<bool> condition, Action onComplete)
    {
        StartCoroutine(WaitUntilCoroutine(condition, onComplete));
    }
    private IEnumerator WaitUntilCoroutine(Func<bool> condition, Action onComplete)
    {
        yield return new WaitUntil(condition);
        onComplete?.Invoke();
    }
}