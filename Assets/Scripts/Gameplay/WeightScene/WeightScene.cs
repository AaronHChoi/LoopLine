using UnityEngine;
[System.Serializable]
public class WeightScene 
{
    public string sceneName; 
    public float weight = 1f;
    public int TimesLoaded = 0;
    public Events SceneEvent;
    public GameCondition sceneCondition;
}
