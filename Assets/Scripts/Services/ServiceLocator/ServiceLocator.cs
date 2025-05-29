using System;
using System.Collections.Generic;
using Services.ServiceLocator.Abstract;

namespace Services.ServiceLocator
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
            Type type = service.GetType();

            _services.Add(type, service);
        }

        public void UnRegister<T>(T service) where T : IService
        {
            Type type = service.GetType();

            if (!_services.Remove(type))
                throw new ArgumentException();
        }
    }
}