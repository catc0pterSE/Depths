using System;
using System.Collections.Generic;
using System.Linq;
using Factory;
using UnityEngine;

namespace Pool
{
    public class PoolMono<T> where T : MonoBehaviour
    {
        private readonly bool _autoExpand;
        private readonly Transform _container;
        private readonly IFactory<T> _factory;

        private List<T> _pool;

        public PoolMono(int startCount, Transform container, IFactory<T> factory, bool autoExpand = true)
        {
            _container = container;
            _factory = factory; 
            _autoExpand = autoExpand;

            CreatePool(startCount);
        }

        public T GetFreeElement()
        {
            if (HasFreeElement(out var element))
                return element;

            if (_autoExpand)
                return CreateObject(true);

            throw new Exception($"There is no free element in pool of type {typeof(T)}");
        }

        private T CreateObject(bool isActiveByDefault = false)
        {
            var createdObject = _factory.Create(_container);
            createdObject.gameObject.SetActive(isActiveByDefault);
            _pool.Add(createdObject);
            return createdObject;
        }

        private bool HasFreeElement(out T element)
        {
            foreach (var mono in _pool.Where(mono => mono.gameObject.activeInHierarchy == false))
            {
                element = mono;
                mono.gameObject.SetActive(true);
                return true;
            }

            element = null;
            return false;
        }

        private void CreatePool(int count)
        {
            _pool = new List<T>();

            for (var i = 0; i < count; i++)
                CreateObject();
        }
    }
}