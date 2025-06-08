using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils.ObjectPoolUtil
{
    public class LocalObjectPool<T> : IDisposable where T : PoolableObject
    {
        private readonly Transform _parent;
        private readonly T _prefab;
        private readonly Queue<T> _objects;
        
        public LocalObjectPool(int count, T prefab)
        {
            _parent = new GameObject("DummyProjectilesPool").transform;
            _prefab = prefab;
            _objects = new Queue<T>(count);
            Initialize(count);
        }

        private void Initialize(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var spawnedObject = InstantiateObject();
                
                _objects.Enqueue(spawnedObject);
            }
        }

        public T InstantiateObject()
        {
            var spawnedObject = Object.Instantiate(_prefab, _parent.position, Quaternion.identity, _parent);
            spawnedObject.gameObject.SetActive(false);
            
            return spawnedObject;
        }

        public T Get(Vector3 position, Quaternion rotation)
        {
            var objectToSend = _objects.Count == 0 ? InstantiateObject() : _objects.Dequeue();
            
            objectToSend.transform.position = position;
            objectToSend.transform.rotation = rotation;
            
            objectToSend.gameObject.SetActive(true);
            
            return objectToSend;
        }

        public void Return(T objectToReturn)
        {
            objectToReturn.gameObject.SetActive(false);
            
            _objects.Enqueue(objectToReturn);
        }

        public void Dispose()
        {
            foreach (var poolableObject in _objects)
            {
                Object.Destroy(poolableObject);
            }
        }
    }
}