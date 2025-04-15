using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] public float LoopTime { get; private set; } = 360f;

    [SerializeField] private string nextScene;

    public bool changeLoopTime = false;
    public bool AllowFastForward = false;

    //private DevelopmentManager developmentManager;
    private float iniatialLoopTime;
    [SerializeField] private int timeMultiplier = 4;
    
    
    private const int TIME_DEFAULT = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //developmentManager = FindAnyObjectByType<DevelopmentManager>();
        iniatialLoopTime = LoopTime;
    }

    private void Update()
    {
        if (changeLoopTime && LoopTime >= 5f)
        {
            LoopTime = 5f;
        }

        TimeForward();
    }

    private void LoadNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    private void TimeForward()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            float speedMultiplier = AllowFastForward && Input.GetKey(KeyCode.F) ? timeMultiplier : TIME_DEFAULT;

            AdjustGameSpeed(speedMultiplier);

            LoopTime -= Time.deltaTime * Time.timeScale;

            if (LoopTime <= 0)
            {
                LoopTime = 360f;
                LoadNextScene(nextScene);
            }
        }
        else
        {
            AdjustGameSpeed(TIME_DEFAULT);
        }
    }
    public void AdjustGameSpeed(float speedMultiplier)
    {
        Time.timeScale = speedMultiplier;
    }
    public void AllowFastForwardMethod(bool enabled)
    {
        AllowFastForward = enabled;
    }
}
