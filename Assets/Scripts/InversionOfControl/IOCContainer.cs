using System;
using System.Collections.Generic;
using UnityEngine;

namespace InversionOfControl
{
    public class IOCContainer
    {
        private class Item
        {
            public object Instance;
            public MulticastDelegate Function;

            public Item(object instance)
            {
                Instance = instance;
            }

            public Item(MulticastDelegate function)
            {
                Function = function;
            }

            public T Resolve<T>()
            {
                if (Instance != null)
                {
                    return (T)Instance;
                }

                return ((Func<T>)Function)();
            }
        }

        private Dictionary<Type, Dictionary<string, Item>> m_named = new Dictionary<Type, Dictionary<string, Item>>();
        private Dictionary<Type, Item> m_registered = new Dictionary<Type, Item>();
        private Dictionary<Type, Item> m_fallbacks = new Dictionary<Type, Item>();

        public bool IsRegistered<T>(string name)
        {
            if (!m_named.TryGetValue(typeof(T), out Dictionary<string, Item> nameToItem))
            {
                return false;
            }

            return nameToItem.ContainsKey(name);
        }

        public void Register<T>(string name, Func<T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            Dictionary<string, Item> nameToItem;
            if (!m_named.TryGetValue(typeof(T), out nameToItem))
            {
                nameToItem = new Dictionary<string, Item>();
                m_named.Add(typeof(T), nameToItem);
            }

            if (nameToItem.ContainsKey(name))
            {
                Debug.LogWarningFormat("item with name {0} already registered", name);
            }

            nameToItem[name] = new Item(func);
        }

        public void Register<T>(string name, T instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("func");
            }

            Dictionary<string, Item> nameToItem;
            if (!m_named.TryGetValue(typeof(T), out nameToItem))
            {
                nameToItem = new Dictionary<string, Item>();
                m_named.Add(typeof(T), nameToItem);
            }

            if (nameToItem.ContainsKey(name))
            {
                Debug.LogWarningFormat("item with name {0} already registered", name);
            }

            nameToItem[name] = new Item(instance);
        }

        public bool IsRegistered<T>()
        {
            return m_registered.ContainsKey(typeof(T));
        }

        public void Register<T>(Func<T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            if (m_registered.ContainsKey(typeof(T)))
            {
                Debug.LogWarningFormat("type {0} already registered.", typeof(T).FullName);
            }

            m_registered[typeof(T)] = new Item(func);
        }

        public void Register<T>(T instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (m_registered.ContainsKey(typeof(T)))
            {
                Debug.LogWarningFormat("type {0} already registered.", typeof(T).FullName);
            }

            m_registered[typeof(T)] = new Item(instance);
        }

        public void Unregister<T>(string name, Func<T> func)
        {
            Dictionary<string, Item> nameToItem;
            if (m_named.TryGetValue(typeof(T), out nameToItem))
            {
                Item item;
                if (nameToItem.TryGetValue(name, out item))
                {
                    if (item.Function != null && item.Function.Equals(func))
                    {
                        nameToItem.Remove(name);
                        if (nameToItem.Count == 0)
                        {
                            m_named.Remove(typeof(T));
                        }
                    }
                }
            }
        }

        public void Unregister<T>(string name, T instance)
        {
            Dictionary<string, Item> nameToItem;
            if (m_named.TryGetValue(typeof(T), out nameToItem))
            {
                Item item;
                if (nameToItem.TryGetValue(name, out item))
                {
                    if (ReferenceEquals(item.Instance, instance))
                    {
                        nameToItem.Remove(name);
                        if (nameToItem.Count == 0)
                        {
                            m_named.Remove(typeof(T));
                        }
                    }
                }
            }
        }

        public void Unregister<T>(Func<T> func)
        {
            Item item;
            if (m_registered.TryGetValue(typeof(T), out item))
            {
                if (item.Function != null && item.Function.Equals(func))
                {
                    m_registered.Remove(typeof(T));
                }
            }
        }

        public void Unregister<T>(T instance)
        {
            Item item;
            if (m_registered.TryGetValue(typeof(T), out item))
            {
                if (ReferenceEquals(item.Instance, instance))
                {
                    m_registered.Remove(typeof(T));
                }
            }
        }

        public void Unregister<T>()
        {
            m_registered.Remove(typeof(T));
        }

        public bool IsFallbackRegistered<T>()
        {
            return m_fallbacks.ContainsKey(typeof(T));
        }

        public void RegisterFallback<T>(Func<T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            if (m_fallbacks.ContainsKey(typeof(T)))
            {
                Debug.LogWarningFormat("fallback for type {0} already registered.", typeof(T).FullName);
            }

            m_fallbacks[typeof(T)] = new Item(func);
        }

        public void RegisterFallback<T>(T instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (m_fallbacks.ContainsKey(typeof(T)))
            {
                Debug.LogWarningFormat("type {0} already registered.", typeof(T).FullName);
            }

            m_fallbacks[typeof(T)] = new Item(instance);
        }

        public void UnregisterFallback<T>(Func<T> func)
        {
            Item item;
            if (m_fallbacks.TryGetValue(typeof(T), out item))
            {
                if (item.Function != null && item.Function.Equals(func))
                {
                    m_fallbacks.Remove(typeof(T));
                }
            }
        }

        public void UnregisterFallback<T>(T instance)
        {
            Item item;
            if (m_fallbacks.TryGetValue(typeof(T), out item))
            {
                if (ReferenceEquals(item.Instance, instance))
                {
                    m_fallbacks.Remove(typeof(T));
                }
            }
        }

        public void UnregisterFallback<T>()
        {
            m_fallbacks.Remove(typeof(T));
        }

        public T Resolve<T>(string name)
        {
            Dictionary<string, Item> nameToItem;
            if (m_named.TryGetValue(typeof(T), out nameToItem))
            {
                Item item;
                if (nameToItem.TryGetValue(name, out item))
                {
                    return item.Resolve<T>();
                }
            }

            return default(T);
        }

        public T Resolve<T>()
        {
            Item item;
            if (m_registered.TryGetValue(typeof(T), out item))
            {
                return item.Resolve<T>();
            }
            else
            {
                if (m_fallbacks.TryGetValue(typeof(T), out item))
                {
                    return item.Resolve<T>();
                }
            }

            return default(T);
        }

        public void Clear()
        {
            m_registered.Clear();
            m_fallbacks.Clear();
            m_named.Clear();
        }
    }
}
