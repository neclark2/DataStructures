using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    public class Queue<T>
    {
        private List<T> list  = new List<T>();

        public void Enqueue(T obj)
        {
            list.Add(obj);
        }

        public T Dequeue()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Cannot dequeue because it is empty");
            }
            var first = list.Last();
            list.RemoveAt(list.Count - 1);
            return first;
        }

        public bool IsEmpty()
        {
            return !list.Any();
        }
    }
}