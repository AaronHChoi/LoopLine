
namespace DependencyInjection
{
    public interface IDependencyInjectable
    {
        void InjectDependencies(DependencyContainer provider);
    }
}