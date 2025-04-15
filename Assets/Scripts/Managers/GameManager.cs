using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] public float LoopTime { get; private set; } = 360f;

    [SerializeField] private string nextScene;

    public bool changeLoopTime = false; 

    //private DevelopmentManager developmentManager;
    private float iniatialLoopTime;
    int timeMultiplier = 8;
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
            AdjustGameSpeed(Input.GetKey(KeyCode.F) ? timeMultiplier : TIME_DEFAULT);

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
}
