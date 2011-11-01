using System.Collections.Generic;

namespace Exeggcute.src
{
    class HashList<T>
    {
        private Dictionary<T, bool> container;
        public int Count
        {
            get { return container.Count; } 
        }
        public string Name;

        public HashList(string name)
        {
            container = new Dictionary<T, bool>();
            Name = name;
        }

        public HashList(int capacity)
        {
            container = new Dictionary<T, bool>(capacity);
        }


        public HashList()
        {
            container = new Dictionary<T, bool>();
        }



        public HashList(IEnumerable<T> collection)
        {
            foreach (T thing in collection)
            {
                container[thing] = true;
            }
        }

        

        public void Add(T entity)
        {
            container[entity] = true;
        }

        public bool Contains(T entity)
        {
            return container.ContainsKey(entity);
        }

        public void Remove(T entity)
        {
            container.Remove(entity);
        }

        public void Clear()
        {
            container.Clear();
        }

        public Dictionary<T, bool>.KeyCollection GetKeys()
        {
            return container.Keys;
        }

        public IEnumerator<KeyValuePair<T, bool>> GetEnumerator()
        {
            return container.GetEnumerator();
        }
    }
}
