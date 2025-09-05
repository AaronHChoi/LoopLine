using UI;

namespace DependencyInjection
{
    public class PhotoContainer : BaseContainer
    {
        public PhotoCapture PhotoCapture { get; private set; }
        public PhotoMarker PhotoMarker { get; private set; }
        public PhotoDetectionZone PhotoDetectionZone { get; private set; }
        public PhotoMarkerManager PhotoMarkerManager { get; private set; }

        public void Initialize()
        {
            PhotoCapture = FindAndValidate<PhotoCapture>();
            PhotoMarker = FindAndValidate<PhotoMarker>();
            PhotoDetectionZone = FindAndValidate<PhotoDetectionZone>();
            PhotoMarkerManager = FindAndValidate<PhotoMarkerManager>();
        }
        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<ITogglePhotoDetection>(PhotoDetectionZone);
            //injector.Register<IPhotoMarkerManager>(PhotoMarkerManager);
            //injector.Register<IPhotoDetectionZone>(PhotoDetectionZone);
        }
    }
}