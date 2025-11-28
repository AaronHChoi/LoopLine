using UnityEngine;
using AK.Wwise;
public class Music : Singleton<Music>
{

    [Header("Main Music Event (Wwise)")]
    public AK.Wwise.Event musicEvent;     

    [Header("Optional: Default State on Start")]
    public State defaultState;       
    public State MindPlace1State;
    public State MindPlace2State;

    private uint playingID;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        if (musicEvent != null)
        {
            playingID = musicEvent.Post(gameObject);
        }

        if (defaultState != null)
        {
            defaultState.SetValue();
        }
    }


    public void ChangeState(State newState)
    {
        if (newState == null)
        {
            Debug.LogWarning("MusicManager_Wwise: El State es null.");
            return;
        }

        newState.SetValue();
    }

    public void OnSceneChange(State sceneState)
    {
        ChangeState(sceneState);
    }

    public void RestartMusic()
    {
        if (musicEvent != null)
        {
            AkSoundEngine.StopPlayingID(playingID);
            playingID = musicEvent.Post(gameObject);
        }
    }
}
