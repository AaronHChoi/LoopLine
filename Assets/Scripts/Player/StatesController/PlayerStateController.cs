using UnityEngine;
using System;
using Unity.Cinemachine.Samples;
using InWorldUI;
using UI;
using DependencyInjection;

namespace Player
{
    public class PlayerStateController : MonoBehaviour, IDependencyInjectable, IPlayerStateController
    {
        public StateMachine StateMachine => stateMachine;
        public bool CanUseNormalStateExecute { get; set; } = true;

        public event Action<IState> OnStateChanged;
        public event Action OnTakePhoto;
        public event Action OnInteract;
        public event Action OnDialogueNext;
        //public event Action OnDialogueSkip;
        public event Action OnOpenInventory;
        public event Action OnOpenDevelopment;
        public event Action OnFocusMode;
        public event Action OnScrollInventory;
        public event Action OnGrab;

        StateMachine stateMachine { get; set; }

        TimeManager timeManager;
       
        IPhotoMarker photoMarker;
        ICameraOrientation cinemachinePOVExtension;
        IPhotoCapture photoCapture;
        IPlayerInteractMarkerPrompt interaction;
        IPlayerMovement playerMovement;
        ITogglePhotoDetection togglePhotoDetection;
        IPlayerInputHandler inputHandler;
        public NormalState NormalState { get; set; }
        public DialogueState DialogueState { get;  set; }
        public CameraState CameraState { get;  set; }
        public DevelopmentState DevelopmentState { get;  set; }
        public FocusModeState FocusModeState { get;  set; }
        public MindPlaceState MindPlaceState { get;  set; }
        public ObjectInHandState ObjectInHandState { get;  set; }
        private void Awake()
        {
            InjectDependencies(DependencyContainer.Instance);
            togglePhotoDetection = InterfaceDependencyInjector.Instance.Resolve<ITogglePhotoDetection>();           
            inputHandler = InterfaceDependencyInjector.Instance.Resolve<IPlayerInputHandler>();
            playerMovement = InterfaceDependencyInjector.Instance.Resolve<IPlayerMovement>();
            interaction = InterfaceDependencyInjector.Instance.Resolve<IPlayerInteractMarkerPrompt>();
            photoCapture = InterfaceDependencyInjector.Instance.Resolve<IPhotoCapture>();
            photoMarker = InterfaceDependencyInjector.Instance.Resolve<IPhotoMarker>();
            cinemachinePOVExtension = InterfaceDependencyInjector.Instance.Resolve<ICameraOrientation>();

            stateMachine = new StateMachine();

            NormalState = new NormalState(this, inputHandler, playerMovement, cinemachinePOVExtension);
            DialogueState = new DialogueState(this, inputHandler, playerMovement, cinemachinePOVExtension);
            CameraState = new CameraState(this, inputHandler, playerMovement, photoCapture, cinemachinePOVExtension, interaction, togglePhotoDetection, photoMarker);
            DevelopmentState = new DevelopmentState(this, inputHandler, playerMovement, cinemachinePOVExtension, timeManager);
            FocusModeState = new FocusModeState(this, inputHandler, playerMovement, cinemachinePOVExtension);
            MindPlaceState = new MindPlaceState(this, inputHandler, playerMovement);
            ObjectInHandState = new ObjectInHandState(this, inputHandler, playerMovement, cinemachinePOVExtension);

            stateMachine.Initialize(NormalState);
        }
        public void InjectDependencies(DependencyContainer provider)
        {
            //playerMovement = provider.PlayerContainer.PlayerMovement;
            //photoCapture = provider.PhotoContainer.PhotoCapture;
            //cinemachinePOVExtension = provider.CinemachineContainer.CinemachinePOVExtension;
            timeManager = provider.ManagerContainer.TimeManager;
            //interaction = provider.PlayerContainer.PlayerInteraction;
            //photoMarker = provider.PhotoContainer.PhotoMarker;
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
        public void UseEventFocusMode()
        {
            OnFocusMode?.Invoke();
        }
        public void UseEventGrab()
        {
            OnGrab?.Invoke();
        }
        public void UseEventScrollInventory()
        {
            OnScrollInventory?.Invoke();
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
        public event Action OnFocusMode;
        public event Action OnScrollInventory;
        public event Action OnGrab;
        public StateMachine StateMachine { get; }
        bool IsInState(IState state);
        void ChangeState(IState newState);
        void UseEventInteract();
        void UseEventTakePhoto();
        void UseEventOpenInventory();
        void UseEventDevelopment();
        void UseEventDialogueNext();
        void UseEventFocusMode();
        void UseEventGrab();
        bool CanUseNormalStateExecute { get; set; }
        NormalState NormalState { get;  set; }
        DialogueState DialogueState { get; set; }
        CameraState CameraState { get; set; }
        DevelopmentState DevelopmentState { get; set; }
        FocusModeState FocusModeState { get; set; }
        MindPlaceState MindPlaceState { get; set; }
        ObjectInHandState ObjectInHandState { get; set; }
    }
}