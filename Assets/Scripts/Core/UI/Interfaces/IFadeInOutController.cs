using UnityEngine;

namespace Core.UI
{
    public interface IFadeInOutController
    {
        FadeID Identifier { get; }
        bool IsVisible { get; }
        void ForceFade(bool isFadeIn);
        GameObject gameObject { get; }
    } 
}