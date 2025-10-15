using UnityEngine;
using DependencyInjection;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public string nextScene;

    public int TrainLoop = 0;   // CONTADOR DE LOOPS DEL TREN
    public int MindPlaceLoop = 0;

    public bool CameraGirlPhoto = false;
    public List<BlackRoomComponentSETTING> CameraGirlPhotoMision = new List<BlackRoomComponentSETTING>();

    [Header("DeveloperTools")]
    public bool isMuted = false;
    bool firstTime = false;

    IDialogueManager dialogueManager;
    IGameSceneManager sceneManager;
    IMonologueSpeaker monologueSpeaker;
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
        sceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
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
            NPCDialogueManager.Instance.HandleEventChange("cameraGirl", Events.Test);
            monologueSpeaker.StartMonologue(Events.MonologueTest1);
        }
        if (Input.GetKeyDown(KeyCode.O)) //TEST
        {
            monologueSpeaker.StartMonologue(Events.MonologueTest2);
        }
        if (sceneManager.IsCurrentScene("04. Train") && TrainLoop > 0)
        {
            CameraGirlMarkerBoard.Instance.DeactivateMakerBoard();
        }
        else if (TrainLoop > 0) 
        {
            CameraGirlMarkerBoard.Instance.ActivateMakerBoard();
        }
    }
}
