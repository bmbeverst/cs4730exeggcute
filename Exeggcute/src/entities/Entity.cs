using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.entities
{
    abstract class Entity
    {
        private static int totalInstances = 0;
        private int id;
        public int ID { get { return id; } }
        public Entity()
        {
            id = totalInstances++;
        }

        public override int GetHashCode()
        {
            return id;
        }

        public override bool Equals(object obj)
        {
            return this == obj || (obj is Entity && ((Entity)obj).ID == id);
        }
    }
}
