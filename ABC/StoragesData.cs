using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC
{
    class StoragesData
    {
        const int population = 4;
        public List<Storage> storages;
        private List<int> ids;
        public List<List<int>> costs = new List<List<int>>() {  new List<int> { 0, 90, 50, -1, -1, -1, -1, -1, -1 }, 
                                                                new List<int> { -1, 0, 50, 50, 10, -1, -1, 1, 2 }, 
                                                                new List<int> { -1, -1, 0, 99, -1, -1, -1, -1, -1 }, 
                                                                new List<int> { -1, -1, -1, 0, -1, -1, -1, -1, -1 }, 
                                                                new List<int> { -1, -1, -1, 25, 0, -1, -1, -1, 2 },
                                                                new List<int> { -1, -1, -1, -1, -1, 0, -1, 3, -1},
                                                                new List<int> { 2, 2, -1, -1, -1, -1, 0, -1, -1},
                                                                new List<int> { -1, -1, -1, -1, -1, -1, -1, 0, 2},
                                                                new List<int> { -1, -1, -1, -1, -1, -1, -1, -1, 0}};

        // each row represents a storage containing it's resources [index, cost] (get the resource by using this index in the resources array)
        private List<List<Tuple<int, int>>> storageResources = new List<List<Tuple<int, int>>>() {
                                                        new List<Tuple<int,int>> {  },
                                                        new List<Tuple<int,int>> {  },
                                                        new List<Tuple<int,int>> { new Tuple<int, int>(1, 5) },
                                                        new List<Tuple<int,int>> { new Tuple<int, int>(2, 5) },
                                                        new List<Tuple<int,int>> { },
                                                        new List<Tuple<int,int>> { },
                                                        new List<Tuple<int,int>> { },
                                                        new List<Tuple<int,int>> { new Tuple<int, int>(0, 5) },
                                                        new List<Tuple<int,int>> { }};

        public List<List<bool>> hasStorageResource = new List<List<bool>> {
                                                new List<bool> {false, false},
                                                new List<bool> {false, false},
                                                new List<bool> {false, true},
                                                new List<bool> {false, true},
                                                new List<bool> {false, false},
                                                new List<bool> {false, false},
                                                new List<bool> {false, false},
                                                new List<bool> {true, false},
                                                new List<bool> {false, false},
        };


        private const int nResources = 3;
        private List<Resource> resources = new List<Resource>(); // stores all the resources in the map
        private List<int> rtypes = new List<int>() { 1, 2, 2 }; // stores resources types : 1- farm , 2- mine
        private List<int> rlevels = new List<int>() { 1, 2, 3, 1, 2, 3, 1, 2, 3, 1 }; // stores resources level
        private List<int> rvalues = new List<int>() { 1, 2, 3, 1, 2, 3, 1, 2, 3, 1 }; // stores resources value
        //private int[] rcosts = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 }; // stores resources cost from each storage


        public StoragesData(int numberStorages)
        {
            // init all storages ids
            this.ids = new List<int>();

            for (int i = 0; i < numberStorages; i++)
            {
                ids.Add(i);
            }

            // init all resources
            for (int i = 0; i < nResources; i++)
            {
                resources.Add(new Resource(i, rtypes[i], rlevels[i], rvalues[i]));//, rcosts[i]);
            }
            
            this.storages = new List<Storage>();

            /*
             * 
             * for each storage :
             *      1- set the costs to other storages.
             *      2- add storage resources from the all resources array (storageResources[]).
             *      3- create and store the new storage in (storages[]) array.
             *      
             * */
            for (int i = 0; i < numberStorages; i++)
            {
                // setting costs between storages
                int[] cost = new int[numberStorages];
                for (int j = 0; j < numberStorages; j++)
                {
                    cost[j] = costs[i][j];
                }
                // adding storage resources
                List<Tuple<Resource, int>> rtemp = new List<Tuple<Resource, int>>();
                int[] rcosts = new int[storageResources[i].Count];
                for (int k = 0; k < storageResources[i].Count; k++)
                {
                    Resource r = getResourceById(storageResources[i][k].Item1);
                    rcosts[k] = storageResources[i][k].Item2;
                    rtemp.Add(new Tuple<Resource, int>(r, rcosts[k]));
                }
                // creating storage
                this.storages.Add(new Storage(ids[i], cost, rtemp, population));
                //Console.WriteLine(rtemp[0].Item1);
            }
        }

        public Resource getResourceById(int id)
        {
            //Console.WriteLine(id);
            foreach (Resource r in resources)
            {
                if (r.getId() == id)
                {
                    //Console.WriteLine("Found : " + id);
                    return r;
                }
            }
            return null;
        }

        public double cost(int firstStorage, int secondStorage)
        {
            //check if they are connected
            if (costs[firstStorage][secondStorage] != -1)
            {
                return costs[firstStorage][secondStorage];
            }
            else
            {
                return int.MaxValue;
            }
            //return 1.0 * 1; // cost between the two storages + cost from the secondStorage to the resource
        }

        public double minPathCost()
        {
            return 1.0 * 1;// the cost from a resource to it's storage (this is the best case)
        }

        public long maxCost()
        {
            return int.MaxValue;// to be calculated later
        }

        public void addResource(int id, List<Tuple<int, int>> storages, int type, int value)
        {
            resources.Add(new Resource(id, type, 1, value));
            foreach (Tuple<int, int> i in storages) {
                storageResources[i.Item1].Add(i);
                hasStorageResource[i.Item1][type - 1] = true;
            }
        }

        public void addStorage(int id, int[] costs, List<Tuple<Resource, int>> sResources, int people)
        {
            storages.Add(new Storage(id, costs, sResources, people));
            List<bool> r = new List<bool>();
            for (int i = 0; i < this.resources.Count; i++)
            {
                r.Add(false);
            }
            for (int i = 0; i < sResources.Count; i++)
            {
                r[sResources[i].Item1.getId()] = true;
            }
            hasStorageResource.Add(r);
        }

        public int getTotalPopulation(int id)
        {
            int total = 0;
            foreach (Storage s in storages)
            {
                total += s.getPopulation();
            }
            return total;
        }

        public int getTotalResources(int type)
        {
            int total = 0;
            foreach (Resource r in resources)
            {
                if (r.getType() == type)
                {
                    total += r.getValue();
                }
            }
            return total;
        }

        public void setResourceValue(int sid, int nValue)
        {
            foreach (Resource r in resources) 
            {
                if (r.getId() == sid)
                {
                    r.setValue(nValue);
                    break;
                }
            }
        } 
    }
}
