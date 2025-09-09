using UI;

namespace DependencyInjection
{
    public class PhotoContainer : BaseContainer
    {
        PhotoCapture photoCapture;
        public PhotoCapture PhotoCapture => photoCapture ??= FindAndValidate<PhotoCapture>();

        PhotoMarker photoMarker;
        public PhotoMarker PhotoMarker => photoMarker ??= FindAndValidate<PhotoMarker>();

        PhotoDetectionZone photoDetectionZone;
        public PhotoDetectionZone PhotoDetectionZone => photoDetectionZone ??= FindAndValidate<PhotoDetectionZone>();

        PhotoMarkerManager photoMarkerManager;
        public PhotoMarkerManager PhotoMarkerManager => photoMarkerManager ??= FindAndValidate<PhotoMarkerManager>();

        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<ITogglePhotoDetection>(() => PhotoDetectionZone);
        }
    }
}