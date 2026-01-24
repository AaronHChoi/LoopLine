
namespace Gameplay.DependencyInjection
{
    public interface IDependencyInjectable
    {
        void InjectDependencies(DependencyContainer provider);
    }
}