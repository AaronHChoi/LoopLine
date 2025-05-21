using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private ScreenManager screenManager;
    [SerializeField] DialogueManager dialogueManager;
    public string nextScene;

    public int TrainLoop = 0;
    public int MindPlaceLoop = 0;
    public ScreenManager ScreenManager => screenManager;
    public bool CorrectWord101 = false;

    [Header("DeveloperTools")]
    public bool isMuted = false;

    [Header("Test")]
    public bool workingMan = false;
    public bool cameraGirl = false;

    public Dictionary<string, System.Action<bool>> boolSetters;
    //
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

        boolSetters = new Dictionary<string, System.Action<bool>>
        {
            {"WorkingMan", value => workingMan = value },
            {"CameraGirl", value => cameraGirl = value }
        };

        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }
    private void Start()
    {
        dialogueManager.ResetAllDialogues();
    }
    public void SetBool(string key, bool value)
    {
        if(boolSetters.TryGetValue(key, out var setter))
        {
            setter(value);
        }
        else
        {
            Debug.LogWarning($"No se encontro el bool para la clave: {key}");
        }
    }
    public Dictionary<string, bool> GetNPCBoolStates()
    {
        return new Dictionary<string, bool>
        {
            { "WorkingMan", workingMan },
            { "CameraGirl", cameraGirl }
        };
    }
}
