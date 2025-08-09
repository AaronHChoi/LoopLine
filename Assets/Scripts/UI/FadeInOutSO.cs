using UnityEngine;

[CreateAssetMenu(fileName = "FadeInOut", menuName = "Scriptable Object/New FadeInOut")]

public class FadeInOutSO : ScriptableObject
{
    [SerializeField] private FadeState startsWithFadeState = FadeState.FadeIn;
    [Space]
    [SerializeField] private FadeTiming fadeIn;
    [Space]
    [SerializeField] private FadeTiming fadeOut;
    [Space]
    [SerializeField] private int ammountFadeLoops = 1;
    [SerializeField] private bool disableOnFinish = false;

    public FadeState StartsWithFadeState => startsWithFadeState;
    public FadeTiming FadeIn => fadeIn;
    public FadeTiming FadeOut => fadeOut;
    public int AmmountFadeLoops => ammountFadeLoops;
    public bool DisableOnFinish => disableOnFinish;
}

[System.Serializable]
public  struct FadeTiming
{
    public  float TimeBeforeFade;
    public  float FadeTime;
}

public enum FadeState
{
    FadeIn,
    FadeOut,
    Force
}