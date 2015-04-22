using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace DataStructures
{
    public class ConcurrentDictionary<TKey, TValue>
    {
        private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private  Dictionary<TKey, TValue>  _dictionary = new Dictionary<TKey, TValue>();

        public void AddItem(TKey key, TValue value)
        {
            _lock.EnterWriteLock();
            _dictionary.Add(key, value);
            _lock.ExitWriteLock();
        }

        public TValue GetItem(TKey key)
        {
            _lock.EnterReadLock();
            var val = _dictionary[key];
            _lock.ExitReadLock();
            return val;
        }
    }

    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var size = 100000;
            var numWriters = 3;
            var dictionary = new ConcurrentDictionary<string, string>();
            var threads = new List<Thread>();
            for (var i = 0; i < numWriters - 1; i++)
            {
                var t = new Thread(() => AddItemsToDictionary(dictionary, i * size / numWriters, (i * size / numWriters) + size / numWriters));
                t.Start();
                threads.Add(t);
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

        }

        private void AddItemsToDictionary(ConcurrentDictionary<string, string> dictionary, int min, int max)
        {
            for (var i = min; i < max; i++)
            {
                dictionary.AddItem(i.ToString(), i.ToString());
            }
        }

    }
}
