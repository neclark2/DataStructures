using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{

    public class Stack<T>
    {
        private string write_lock = "";
        private List<T> list = new List<T>();

        public void Push(T obj)
        {
            list.Add(obj);
        }

        public T Pop()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Stack cannot be popped because it is empty");
            }

            lock (write_lock)
            {
                var last = list.Last();
                list.RemoveAt(list.Count - 1);
                return last;
            }
            
        }

        public bool IsEmpty()
        {
            return list.Count() == 0;
        }
    }
}
