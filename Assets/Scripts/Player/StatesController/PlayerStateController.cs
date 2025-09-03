using UnityEngine;
using System;
using InWorldUI;
using UI;

namespace Player
{
    public class PlayerStateController : MonoBehaviour, IDependencyInjectable
    {
        public StateMachine stateMachine { get; private set; }

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

        PlayerInputHandler playerInputHandler;
        PlayerMovement playerMovement;
        PhotoCapture photoCapture;
        PlayerInteraction interaction;
        PhotoMarker photoMarker;
        
        public NormalState NormalState { get; private set; }
        public DialogueState DialogueState { get; private set; }
        public CameraState CameraState { get; private set; }
        public DevelopmentState DevelopmentState { get; private set; }
        public FocusModeState FocusModeState { get; private set; }
        public InventoryState InventoryState { get; private set; }
        public MindPlaceState MindPlaceState { get; private set; }
        private void Awake()
        {
            InjectDependencies(DependencyContainer.Instance);

            stateMachine = new StateMachine();

            NormalState = new NormalState(this, playerInputHandler, playerMovement);
            DialogueState = new DialogueState(this, playerInputHandler, playerMovement);
            CameraState = new CameraState(this, playerInputHandler, playerMovement, photoCapture, interaction, photoMarker);
            InventoryState = new InventoryState(this, playerInputHandler, playerMovement);
            DevelopmentState = new DevelopmentState(this, playerInputHandler, playerMovement);
            FocusModeState = new FocusModeState(this, playerInputHandler, playerMovement);
            MindPlaceState = new MindPlaceState(this, playerInputHandler, playerMovement);

            stateMachine.Initialize(NormalState);
        }
        public void InjectDependencies(DependencyContainer provider)
        {
            playerMovement = provider.PlayerMovement;
            playerInputHandler = provider.PlayerInputHandler;
            photoCapture = provider.PhotoCapture;
            interaction = provider.PlayerInteraction;
            photoMarker = provider.PhotoMarker;
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
}