using UnityEngine;
using DependencyInjection;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public string nextScene;

    public int TrainLoop = 0;   // CONTADOR DE LOOPS DEL TREN
    public int MindPlaceLoop = 0;

    public bool CameraGirlPhoto = false;

    [Header("DeveloperTools")]
    public bool isMuted = false;

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

        dialogueManager = InterfaceDependencyInjector.Instance.Resolve<IDialogueManager>();
    }
    private void Start()
    {
        dialogueManager.ResetAllDialogues();
        dialogueManager.UnlockFirstDialogues();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) //TEST
        {
            CameraGirlPhoto = true;
        }
    }
}
