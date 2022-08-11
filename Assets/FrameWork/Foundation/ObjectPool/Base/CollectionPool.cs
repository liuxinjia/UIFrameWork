// Base on Unity C# reference source


using System.Collections.Generic;

namespace Cr7Sund.Pool
{
    public class CollectionPool<TCollection, TItem> where TCollection : class, ICollection<TItem>, new()
    {
        internal static readonly ObjectPool<TCollection> s_Pool = new ObjectPool<TCollection>(() => new TCollection(), null, l => l.Clear());

        /// <summary>
        /// Get a new instance from the Pool.
        /// </summary>
        /// <returns></returns>
        public static TCollection Get() => s_Pool.Get();

        /// <summary>
        /// Get a new instance and a PooledObject. The PooledObject will automatically return the instance when it is Disposed.
        /// PLAN support different size adjugement
        /// </summary>
        /// <param name="value">Output new instance.</param>
        /// <returns>A new PooledObject.</returns>
        public static PooledObject<TCollection> Get(out TCollection value) => s_Pool.Get(out value);

        /// <summary>
        /// Release an object to the pool.
        /// </summary>
        /// <param name="toRelease">instance to release.</param>
        public static void Release(TCollection toRelease) => s_Pool.Release(toRelease);
    }

    public class ListPool<T> : CollectionPool<List<T>, T> { }
    public class HashSetPool<T> : CollectionPool<HashSet<T>, T> { }
    public class DictionaryPool<TKey, TValue> : CollectionPool<Dictionary<TKey, TValue>, KeyValuePair<TKey, TValue>> { }
}
