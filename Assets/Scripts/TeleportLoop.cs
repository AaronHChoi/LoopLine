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

    [SerializeField] GameObject player;

    private void Awake()
    {
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
        polaroid = InterfaceDependencyInjector.Instance.Resolve<IPhotoCapture>();
    }
    public void Teleport()
    {
        Vector3 localOffset = transform.InverseTransformPoint(player.transform.position);
        Quaternion relativeRotation = TeleportZoneObject.rotation * Quaternion.Inverse(transform.rotation);
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null)
        {
            bool isInitialLoop = gameSceneManager.GetIsInInitialLoop();

            if (!isInitialLoop && GameManager.Instance.GetCondition(GameCondition.IsFirstLoopsCompleted))
            {
                gameSceneManager.UnloadLastScene();
                gameSceneManager.LoadRandomScene();
            }
            else if (!GameManager.Instance.GetCondition(GameCondition.IsFirstLoopsCompleted))
            {
                
                switch (GameManager.Instance.TrainLoop)
                {
                    case 0:
                    case 1:
                        gameSceneManager.UnloadLastScene();
                        gameSceneManager.LoadSceneAsync("AS_NPC");
                        break;
                    case 2:
                        gameSceneManager.UnloadLastScene();
                        gameSceneManager.LoadSceneAsync("AS_NoNPC-NoItems");
                        break;
                    case 3:
                        gameSceneManager.UnloadLastScene();
                        gameSceneManager.LoadSceneAsync("AS_Clocks");
                        break;
                    default:
                        gameSceneManager.UnloadLastScene();
                        gameSceneManager.LoadRandomScene();
                        break;
                }
            }

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