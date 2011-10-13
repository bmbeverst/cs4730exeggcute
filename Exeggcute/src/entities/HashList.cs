using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Exeggcute.src.entities
{
    class HashList<T> : IEnumerable<KeyValuePair<T, bool>>
    {
        private Dictionary<T, bool> container;

        public HashList()
        {
            container = new Dictionary<T, bool>();
        }

        public HashList(int capacity)
        {
            container = new Dictionary<T, bool>(capacity);
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

        public Dictionary<T, bool>.KeyCollection GetKeys()
        {
            return container.Keys;
        }

        public IEnumerator<KeyValuePair<T, bool>> GetEnumerator()
        {
            return container.GetEnumerator();
        }





        #region IEnumerable<KeyValuePair<T,bool>> Members

        IEnumerator<KeyValuePair<T, bool>> IEnumerable<KeyValuePair<T, bool>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
