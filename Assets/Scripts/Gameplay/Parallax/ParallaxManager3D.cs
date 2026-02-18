using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.EventBus;
using Core.Data;

[Serializable]
public class ParallaxLayer
{
    public string name;
    public float moveSpeed = 5f;
    public List<Transform> objects = new List<Transform>();
    public float spacing;
}
public class ParallaxManager3D : MonoBehaviour
{
    [SerializeField] private float limitZ = -706f;
    [SerializeField] private List<ParallaxLayer> layers = new List<ParallaxLayer>();

    private float globalSpeedMultiplier = 1f;
    private bool isTransitiong = false;

    private void Start()
    {
        foreach (var layer in layers)
        {
            if (layer.objects.Count < 2) continue;

            layer.objects.Sort((a, b) => b.position.z.CompareTo(a.position.z));

            float total = 0f;
            for (int i = 0; i < layer.objects.Count - 1; i++)
            {
                total += Mathf.Abs(layer.objects[i].position.z - layer.objects[i + 1].position.z);
            }

            layer.spacing = total / (layer.objects.Count - 1);
        }
    }
    void LateUpdate()
    {
        foreach (var layer in layers)
        {
            if (layer.objects.Count == 0) continue;

            foreach (var obj in layer.objects)
            {
                if (obj == null) continue;
                obj.Translate(Vector3.back * (layer.moveSpeed * globalSpeedMultiplier) * Time.deltaTime, Space.World);
            }

            foreach (var obj in layer.objects)
            {
                if (obj == null) continue;

                if (obj.position.z <= limitZ)
                {
                    Transform first = GetFrontMostObject(layer.objects);

                    Vector3 newPos = obj.position;
                    newPos.z = first.position.z + layer.spacing;
                    obj.position = newPos;
                }
            }
        }
    }
    private void OnEnable()
    {
        EventBus.Subscribe<ClockSyncEvent>(DebugStartStopSequence);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<ClockSyncEvent>(DebugStartStopSequence);
    }

    [ContextMenu("Debug: Trigger stop and resume")]
    public void DebugStartStopSequence(ClockSyncEvent ev)
    {
        StartStopSequence();
    }
    public void StartStopSequence(float stopDuration = 7.5f, float waitTime = 30f, float resumeDuration = 7.5f)
    {
        if (isTransitiong)
        {
            return;
        }
        StartCoroutine(StopAndResumeRoutine(stopDuration, waitTime, resumeDuration));
    }

    private IEnumerator StopAndResumeRoutine(float stopDuration, float waitTime, float resumeDuration)
    {
        isTransitiong = true;
        float timer = 0f;

        while (timer < stopDuration)
        {
            timer += Time.deltaTime;
            globalSpeedMultiplier = Mathf.Lerp(1f, 0f, timer / stopDuration);
            yield return null;
        }
        globalSpeedMultiplier = 0f;

        GameManager.Instance.SetCondition(GameCondition.IsClockFrozen, true);
        EventBus.Publish(new ClockFreezeEvent(11, 20, 0));
        
        yield return new WaitForSeconds(waitTime);

        GameManager.Instance.SetCondition(GameCondition.IsClockFrozen, false);
        EventBus.Publish(new ClockResumeEvent());

        timer = 0f;
        
        while (timer < resumeDuration)
        {
            timer += Time.deltaTime;
            globalSpeedMultiplier = Mathf.Lerp(0f, 1f, timer / resumeDuration);
            yield return null;
        }

        globalSpeedMultiplier = 1f;

        isTransitiong = false;
    }
    private Transform GetFrontMostObject(List<Transform> objs)
    {
        Transform front = objs[0];
        foreach (var o in objs)
        {
            if (o.position.z > front.position.z)
                front = o;
        }
        return front;
    }
}