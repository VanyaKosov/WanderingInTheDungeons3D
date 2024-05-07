
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core
{
    public class Pool<T>
    {
        private readonly List<T> pool = new List<T>();
        private readonly Func<T> objectCreator;

        public Pool(Func<T> objectCreator)
        {
            this.objectCreator = objectCreator;
        }

        public void Return(T obj)
        {
            pool.Add(obj);
        }

        public T Take()
        {
            if (pool.Count == 0)
            {
                return objectCreator();
            }

            T obj = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
            return obj;
        }
    }
}
