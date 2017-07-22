using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC
{
    class Storage
    {
        private int id;
        private int[] costs;
        private List<Tuple<Resource, int>> resources;
        private int population;

        public Storage(int id, int[] costs, List<Tuple<Resource, int>> resources, int population)
        {
            this.id = id;
            this.costs = costs;
            this.resources = resources;
            this.population = population;
        }

        public int getId()
        {
            return this.id;
        }

        public int getPopulation()
        {
            return this.population;
        }

        public List<Tuple<Resource, int>> getResources() {
            return this.resources;
        }
    }
}
