using UnityEngine;
using Core.UI;
using Core.DependencyInjection;

public class CarretteController : MonoBehaviour, ICarretteController
{
    private Animator animator;

    [SerializeField] AnimatorEnum identifier;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        InterfaceDependencyInjector.Instance.Register<ICarretteController>(() => this, identifier);
    }
    public void SetRotation(bool active)
    {
        if (animator != null)
        {
            animator.SetBool("isRotating", active);
        }
    }
}