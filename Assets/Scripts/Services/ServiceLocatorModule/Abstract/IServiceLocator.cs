namespace Services.ServiceLocatorModule.Abstract
{
    public interface IServiceLocator
    {
        T GetService<T>() where T : IService;
        void Register<T>(T service) where T : IService;
        void UnRegister<T>() where T : IService; 
    }
}