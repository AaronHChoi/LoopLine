using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private ScreenManager screenManager;
    public string nextScene;

    public int TrainLoop = 0;   // CONTADOR DE LOOPS DEL TREN
    public int MindPlaceLoop = 0;
    public ScreenManager ScreenManager => screenManager;
    public bool CorrectWord101 = false;

    [Header("DeveloperTools")]
    public bool isMuted = false;

    [Header("NPC Interacted")]
    public bool workingMan = false;
    public bool cameraGirl = false;
    public bool bassistGirl = false;

    public Dictionary<string, System.Action<bool>> boolSetters;

    IDialogueManager dialogueManager;
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

        boolSetters = new Dictionary<string, System.Action<bool>>
        {
            {"WorkingMan", value => workingMan = value },
            {"CameraGirl", value => cameraGirl = value },
            {"BassistGirl", value => bassistGirl = value }
        };

        dialogueManager = InterfaceDependencyInjector.Instance.Resolve<IDialogueManager>();
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
            { "CameraGirl", cameraGirl },
            { "BassistGirl", bassistGirl }
        };
    }
}
