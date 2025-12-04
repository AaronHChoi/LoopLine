using System;
using DependencyInjection;
using UnityEngine;

public class TeleportLoop : MonoBehaviour, ITeleportLoop
{
    public Transform TeleportZoneObject;

    [SerializeField] private DoorInteract doorInteract;
    public event Action OnTeleportTrain;

    IGameSceneManager gameSceneManager;
    IPhotoCapture polaroid;
    IUIManager uiManager;

    [SerializeField] GameObject player;

    private void Awake()
    {
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
        polaroid = InterfaceDependencyInjector.Instance.Resolve<IPhotoCapture>();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
    }
    public void Teleport()
    {
        Vector3 localOffset = transform.InverseTransformPoint(player.transform.position);
        Quaternion relativeRotation = TeleportZoneObject.rotation * Quaternion.Inverse(transform.rotation);
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null)
        {
            //bool isInitialLoop = gameSceneManager.GetIsInInitialLoop();

            //if (!isInitialLoop && GameManager.Instance.GetCondition(GameCondition.IsFirstLoopsCompleted))
            //{
                
            //}
            gameSceneManager.UnloadLastScene();
            gameSceneManager.LoadRandomScene();
            //else if (!isInitialLoop && !GameManager.Instance.GetCondition(GameCondition.IsFirstLoopsCompleted))
            //{

            //    switch (GameManager.Instance.TrainLoop)
            //    {
            //        case 0:
            //        case 1:
            //            {
            //                gameSceneManager.UnloadLastScene();
            //                gameSceneManager.LoadSceneAsync2("AS_NPC");
            //            }
            //            break;
            //        case 2:
            //            {
            //                gameSceneManager.UnloadLastScene();
            //                gameSceneManager.LoadSceneAsync2("AS_NoNPC-NoItems");
            //            }
            //            break;
            //        case 3:
            //            {
            //                gameSceneManager.UnloadLastScene();
            //                gameSceneManager.LoadSceneAsync2("AS_Clocks");
            //            }
            //            break;
            //        case 4:
            //            {
            //                uiManager.ShowPanel(UIPanelID.TeleportTutorial);
            //                GameManager.Instance.SetCondition(GameCondition.TeleportAvailable, true);
            //                GameManager.Instance.SetCondition(GameCondition.IsFirstLoopsCompleted, true);
            //                gameSceneManager.UnloadLastScene();
            //                gameSceneManager.LoadRandomScene();
            //            }
            //            break;
            //        default:
            //            {
            //                gameSceneManager.UnloadLastScene();
            //                gameSceneManager.LoadRandomScene();
            //            }
            //            break;
            //    }
            //}

            cc.enabled = false;

            player.transform.position = TeleportZoneObject.TransformPoint(localOffset);
            player.transform.rotation = relativeRotation * player.transform.rotation;

            cc.enabled = true;

            GameManager.Instance.TrainLoop += 1;
            polaroid.ResetPhotoCounter();
        }

        OnTeleportTrain?.Invoke();
    }
}
public interface ITeleportLoop
{
    event Action OnTeleportTrain;
}