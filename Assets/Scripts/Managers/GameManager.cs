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
            for (int i = 0; i < CameraGirlPhotoMision.Count; i++)
            {
                CameraGirlPhotoMision[i].CanSpawn = true;
            }
        }
        if(sceneManager.IsCurrentScene("04. Train") && TrainLoop > 0)
        {
            MarkerBoard.Instance.DeactivateMakerBoard();
        }
        else if (TrainLoop > 0) 
        {
            MarkerBoard.Instance.ActivateMakerBoard();
        }
    }
}
