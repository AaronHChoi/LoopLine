using DependencyInjection;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NPCDialogueSpeaker : DialogueSpeakerBase, IInteract
{ 
    [SerializeField] string interactText;
    IPlayerCamera playerCamera;
    private Coroutine lookAtCoroutine;
    private Coroutine returnRotationCoroutine;
    private Quaternion originalRotation;
    private float maxRotationAngle = 70f;
    [SerializeField] Animator animator;
    [SerializeField] private GameObject ikTarget;


    protected override void Awake()
    {
        base.Awake();
        playerCamera = InterfaceDependencyInjector.Instance.Resolve<IPlayerCamera>();
    }
    protected override void Start()
    {
        base.Start();
        if (npcType != NPCType.None)
        {
            if (NPCDialogueManager.Instance != null)
            {
                NPCDialogueManager.Instance.RegisterNPC(npcType, this);
                Debug.Log($"Registered NPC: {npcType}");
            }
            else
            {
                Debug.LogError($"NPCDialogueManager.Instance is null for NPC: {npcType}");
            }
        }
        else
        {
            Debug.LogWarning($"NPC {gameObject.name} has no NPCType assigned", gameObject);
        }
    }
    protected void OnDestroy()
    {
        if (npcType != NPCType.None)
        {
            NPCDialogueManager.Instance?.UnregisterNPC(npcType);
        }
    }
    public string GetInteractText()
    {
        if (interactText == null) return interactText = "";

        return interactText;
    }
    public void Interact()
    {
        if (DialogueManager.Instance.IsOnCooldown)
        {
            return;
        }

        if (!isShowingDialogue && playerStateController.IsInState(playerStateController.NormalState))
        {
            StartDialogueSequence();
            if (animator!= null)
            {
                animator.enabled = false;
                originalRotation = ikTarget.transform.rotation;
                lookAtCoroutine = StartCoroutine(LookAtPlayer());
            }
        }
    }

    protected override void EndDialogueSequence()
    {
        base.EndDialogueSequence();
        //if (animator != null)
        //{
        //    if (lookAtCoroutine != null)
        //    {
        //        StopCoroutine(lookAtCoroutine);
        //        lookAtCoroutine = null;
        //    }
        //        returnRotationCoroutine = StartCoroutine(ReturnToOriginalRotation());
            
        //}
       

    }


    private IEnumerator LookAtPlayer()
    {
        while (isShowingDialogue)
        {
            if (ikTarget != null && playerCamera != null)
            {
                Vector3 direction = playerCamera.GetCameraTransform().position - ikTarget.transform.position;
                direction.y = 0f; 
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                float angle = Quaternion.Angle(originalRotation, targetRotation);

                if (angle <= maxRotationAngle)
                {
                    ikTarget.transform.rotation = Quaternion.Slerp(ikTarget.transform.rotation, targetRotation, Time.deltaTime * 5f);
                }
                else
                {
                    Quaternion clampedRotation = Quaternion.RotateTowards( originalRotation, targetRotation, maxRotationAngle);

                    ikTarget.transform.rotation = Quaternion.Slerp(ikTarget.transform.rotation, clampedRotation, Time.deltaTime * 5f);
                }
            }

            yield return null;
        }
        lookAtCoroutine = null;

        if (returnRotationCoroutine == null)
            returnRotationCoroutine = StartCoroutine(ReturnToOriginalRotation());
    }

    private IEnumerator ReturnToOriginalRotation()
    {
        if (ikTarget == null)
        {
            if (animator != null) animator.enabled = true;
            returnRotationCoroutine = null;
            yield break;
        }

        float duration = 1.0f; 
        float t = 0f;
        Quaternion startRot = ikTarget.transform.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            ikTarget.transform.rotation = Quaternion.Slerp(startRot, originalRotation, t);
            yield return null; 
        }

        ikTarget.transform.rotation = originalRotation;

        if (animator != null) animator.enabled = true;

        returnRotationCoroutine = null;
    }

    //private void OnAnimatorIK()
    //{
    //    if (ikActive)
    //    {
    //        Transform camTransform = playerCamera.GetCameraTransform();
    //        if (camTransform != null)
    //        {
    //            animator.SetLookAtWeight(lookAtWeight);
    //            animator.SetLookAtPosition(camTransform.position);
    //        }
    //        else
    //        {
    //            animator.SetLookAtWeight(0f);
    //        }
    //    }
    //}
}