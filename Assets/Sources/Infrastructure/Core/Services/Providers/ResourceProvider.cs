using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Infrastructure.Core.Services.Providers
{
    public class ResourceProvider
    {
        private static readonly Dictionary<Type, string> ResourcePathByType = new Dictionary<Type, string>()
        {
            [typeof(ConfigurationContainer)] = "Infrastructure/ConfigurationContainer",
        };

        public T Load<T>() where T : Object => 
            Object.Instantiate(Resources.Load(ResourcePathByType[typeof(T)]) as T);
    }
}