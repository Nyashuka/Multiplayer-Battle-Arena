using System;
using System.Collections.Generic;
using Services.ServiceLocatorModule.Abstract;

namespace Services.ServiceLocatorModule
{
    public class ServiceLocator : IServiceLocator
    {
        public static ServiceLocator Instance { get; } = new ServiceLocator();

        private readonly Dictionary<Type, IService> _services = new();
        
        public T GetService<T>() where T : IService
        {
            Type type = typeof(T);

            if (!_services.TryGetValue(type, out var value))
                throw new ArgumentException();

            return (T)value; 
        }

        public void Register<T>(T service) where T : IService
        {
            _services.Add(typeof(T), service);
        }

        public void UnRegister<T>() where T : IService
        {
            Type type = typeof(T);

            if (!_services.Remove(type))
                throw new ArgumentException();
        }
    }
}