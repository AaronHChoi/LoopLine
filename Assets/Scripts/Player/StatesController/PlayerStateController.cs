using UnityEngine;
using System;
using DependencyInjection;

namespace Player
{
    public class PlayerStateController : MonoBehaviour, IPlayerStateController
    {
        public StateMachine StateMachine => stateMachine;
        public bool CanUseNormalStateExecute { get; set; } = true;

        public event Action<IState> OnStateChanged;
        public event Action OnTakePhoto;
        public event Action OnInteract;
        public event Action OnDialogueNext;
        public event Action OnOpenInventory;
        public event Action OnOpenDevelopment;
        public event Action OnScrollInventory;
        public event Action OnGrab;
        public event Action OnTeleport;
        public event Action OnSwitchCameraMode;
        
        StateMachine stateMachine { get; set; }

        ITimeProvider timeManager;
        ICameraOrientation cinemachinePOVExtension;
        IPhotoCapture photoCapture;
        IPlayerInteractMarkerPrompt interaction;
        IPlayerMovement playerMovement;
        ITogglePhotoDetection togglePhotoDetection;
        IPlayerInputHandler inputHandler;
        IGameSceneManager gameSceneManager;
        public NormalState NormalState { get; set; }
        public DialogueState DialogueState { get;  set; }
        public CameraState CameraState { get;  set; }
        public DevelopmentState DevelopmentState { get;  set; }
        public PauseMenuState PauseMenuState { get; set; }
        public MindPlaceState MindPlaceState { get;  set; }
        public ObjectInHandState ObjectInHandState { get;  set; }
        private void Awake()
        {
            togglePhotoDetection = InterfaceDependencyInjector.Instance.Resolve<ITogglePhotoDetection>();           
            inputHandler = InterfaceDependencyInjector.Instance.Resolve<IPlayerInputHandler>();
            playerMovement = InterfaceDependencyInjector.Instance.Resolve<IPlayerMovement>();
            interaction = InterfaceDependencyInjector.Instance.Resolve<IPlayerInteractMarkerPrompt>();
            photoCapture = InterfaceDependencyInjector.Instance.Resolve<IPhotoCapture>();
            cinemachinePOVExtension = InterfaceDependencyInjector.Instance.Resolve<ICameraOrientation>();
            timeManager = InterfaceDependencyInjector.Instance.Resolve<ITimeProvider>();
            gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();

            stateMachine = new StateMachine();

            NormalState = new NormalState(this, inputHandler, playerMovement, cinemachinePOVExtension);
            DialogueState = new DialogueState(this, inputHandler, playerMovement, cinemachinePOVExtension);
            CameraState = new CameraState(this, inputHandler, playerMovement, photoCapture, cinemachinePOVExtension, interaction, togglePhotoDetection);
            DevelopmentState = new DevelopmentState(this, inputHandler, playerMovement, cinemachinePOVExtension, timeManager);
            MindPlaceState = new MindPlaceState(this, inputHandler, playerMovement);
            ObjectInHandState = new ObjectInHandState(this, inputHandler, playerMovement, cinemachinePOVExtension);

            stateMachine.Initialize(NormalState);
        }
        private void Update()
        {
            stateMachine.Execute();
        }
        public void ChangeState(IState newState)
        {
            stateMachine.TransitionTo(newState);
            OnStateChanged?.Invoke(newState);
        }
        public bool IsInState(IState state)
        {
            return stateMachine.CurrentState == state;
        }
        #region ENCAPSULATE_EVENTS
        public void UseEventInteract()
        {
            OnInteract?.Invoke();
        }
        public void UseEventTakePhoto()
        {
            OnTakePhoto?.Invoke();
        }
        public void UseEventDialogueNext()
        {
            OnDialogueNext?.Invoke();
        }
        public void UseEventOpenInventory()
        {
            OnOpenInventory?.Invoke();
        }
        public void UseEventDevelopment()
        {
            OnOpenDevelopment?.Invoke();
        }
        public void UseEventGrab()
        {
            OnGrab?.Invoke();
        }
        public void UseEventScrollInventory()
        {
            OnScrollInventory?.Invoke();
        }
        public void UseEventTeleport()
        {
            OnTeleport?.Invoke();
        }
        #endregion
    }
    public interface IPlayerStateController
    {
        public event Action<IState> OnStateChanged;
        public event Action OnTakePhoto;
        public event Action OnInteract;
        public event Action OnDialogueNext;
        public event Action OnOpenInventory;
        public event Action OnOpenDevelopment;
        public event Action OnScrollInventory;
        public event Action OnGrab;
        public event Action OnTeleport;
        public StateMachine StateMachine { get; }
        bool IsInState(IState state);
        void ChangeState(IState newState);
        void UseEventInteract();
        void UseEventTakePhoto();
        void UseEventOpenInventory();
        void UseEventDevelopment();
        void UseEventDialogueNext();
        void UseEventGrab();
        void UseEventTeleport();
        bool CanUseNormalStateExecute { get; set; }
        NormalState NormalState { get;  set; }
        DialogueState DialogueState { get; set; }
        CameraState CameraState { get; set; }
        DevelopmentState DevelopmentState { get; set; }
        PauseMenuState PauseMenuState { get; set; }
        MindPlaceState MindPlaceState { get; set; }
        ObjectInHandState ObjectInHandState { get; set; }
    }
}