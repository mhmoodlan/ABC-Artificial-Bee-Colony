using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC
{
    class Resource
    {
        private enum rtype { farm=1, mine };
        private int id;
        private rtype type;
        private int level;
        private int value;
        //private int cost;

        public Resource(int id, int type, int level, int value) //, int cost)
        {
            this.id = id;
            this.type = (rtype)type;
            this.level = level;
            this.value = value;
            //this.cost = cost;
        }

        public int getId()
        {
            return this.id;
        }

        public int getType()
        {
            return (int)this.type;
        }

        public int getValue()
        {
            return this.value;
        }

        public void setValue(int value)
        {
            this.value = value;
        }
    }
}
