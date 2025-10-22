using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public string nextScene;

    public int TrainLoop = 0;
    public int MindPlaceLoop = 0;

    [Header("DeveloperTools")]
    public bool isMuted = false;

    bool clockQuest;
    public bool ClockQuest
    {
        get { return clockQuest; }
        set { clockQuest = value; }
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}