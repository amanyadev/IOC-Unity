using System;
using UnityEngine;

namespace InversionOfControl
{
    public class IOC 
    {
        public static bool IsRegistered<T>(string name)
        {
            return s_defaultContainer.IsRegistered<T>(name);
        }

        public static void Register<T>(string name, Func<T> func)
        {
            s_defaultContainer.Register(name, func);
        }

        public static void Register<T>(string name, T instance)
        {
            s_defaultContainer.Register(name, instance);
        }

        public static void Unregister<T>(string name, Func<T> func)
        {
            s_defaultContainer.Unregister(name, func);
        }

        public static void Unregister<T>(string name, T instance)
        {
            s_defaultContainer.Unregister(name, instance);
        }

        public static bool IsRegistered<T>()
        {
            return s_defaultContainer.IsRegistered<T>();
        }

        public static void Register<T>(Func<T> func)
        {
            s_defaultContainer.Register(func);
        }

        public static void Register<T>(T instance)
        {
            s_defaultContainer.Register(instance);
        }

        public static void Unregister<T>(Func<T> func)
        {
            s_defaultContainer.Unregister(func);
        }

        public static void Unregister<T>(T instance)
        {
            s_defaultContainer.Unregister(instance);
        }

        public static void Unregister<T>()
        {
            s_defaultContainer.Unregister<T>();
        }

        public static bool IsFallbackRegistered<T>()
        {
            return s_defaultContainer.IsFallbackRegistered<T>();
        }

        public static void RegisterFallback<T>(Func<T> func)
        {
            s_defaultContainer.RegisterFallback(func);
        }

        public static void RegisterFallback<T>(T instance)
        {
            s_defaultContainer.RegisterFallback(instance);
        }

        public static void UnregisterFallback<T>(Func<T> func)
        {
            s_defaultContainer.UnregisterFallback(func);
        }

        public static void UnregisterFallback<T>(T instance)
        {
            s_defaultContainer.UnregisterFallback(instance);
        }

        public static void UnregisterFallback<T>()
        {
            s_defaultContainer.UnregisterFallback<T>();
        }

        public static T Resolve<T>(string name)
        {
            return s_defaultContainer.Resolve<T>(name);
        }

        public static T Resolve<T>()
        {
            return s_defaultContainer.Resolve<T>();
        }
        public static void ClearAll()
        {
            s_defaultContainer.Clear();
        }

        private static IOCContainer s_defaultContainer = new IOCContainer();
                
        public static IOCContainer GetContainerFor(MonoBehaviour monoBehaviour = null)
        {
            if (monoBehaviour == null)
            {
                return s_defaultContainer;
            }

            IOCGroup group = monoBehaviour.GetComponentInParent<IOCGroup>();
            return group != null ? group.Container : s_defaultContainer;            
        }  
    }
}

