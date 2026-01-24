using UI;

namespace DependencyInjection
{
    public class PhotoContainer : BaseContainer
    {
        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<ITogglePhotoDetection>(() => FindAndValidate<PhotoDetectionZone>());
            injector.Register<IPhotoCapture>(() => FindAndValidate<PhotoCapture>());
            injector.Register<IPhotoMarker>(() => FindAndValidate<PhotoMarker>());
            injector.Register<IBlackRoomComponent>(() => FindAndValidate<BlackRoomComponent>());
            injector.Register<IPolaroidUIAnimation>(() => FindAndValidate<PolaroidUIAnimation>());
        }
    }
}