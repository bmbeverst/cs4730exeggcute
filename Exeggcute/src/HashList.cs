using System.Collections.Generic;
using System.Collections;

namespace Exeggcute.src
{
    class HashList<T> : IEnumerable<T>
    {
        private Dictionary<T, bool> list;

        public int Count { get { return list.Count; } }

        public string Name;

        public HashList(string name)
        {
            list = new Dictionary<T, bool>();
            Name = name;
        }

        public HashList(int capacity)
        {
            list = new Dictionary<T, bool>(capacity);
        }


        public HashList()
        {
            list = new Dictionary<T, bool>();
        }



        public HashList(IEnumerable<T> collection)
        {
            foreach (T thing in collection)
            {
                list[thing] = true;
            }
        }

        

        public void Add(T entity)
        {
            list[entity] = true;
        }

        public bool Contains(T entity)
        {
            return list.ContainsKey(entity);
        }

        public void Remove(T entity)
        {
            list.Remove(entity);
        }

        public void Clear()
        {
            list.Clear();
        }

        /*public Dictionary<T, bool>.KeyCollection GetKeys()
        {
            return list.Keys;
        }*/

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.Keys.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.Keys.GetEnumerator();
        }
    }
}
