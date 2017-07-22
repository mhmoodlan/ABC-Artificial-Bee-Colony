using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC
{
    class Hive
    {
        static Random random = null;

        public int storageId;
        public int foodType;
        public int foodValue;

        public StoragesData storagesData;

        public int totalNumberBees;
        public int numberInactive;
        public int numberActive;
        public int numberScout;

        public int maxNumberCycles;
        public int maxNumberVisits;

        public double probPersuasion = 0.90;
        public double probMistake = 0.01;

        public Bee[] bees;
        public List<int> bestMemoryMatrix;
        public double bestMeasureOfQuality;
        public int bestResourceCost;
        public List<int> indexesOfInactiveBees;


        public Hive(int totalNumberBees, int numberInactive,
            int numberActive, int numberScout, int maxNumberVisits,
            int maxNumberCycles, StoragesData storagesData, int storageId, int foodType, int foodValue)
        {
            random = new Random(0);

            this.totalNumberBees = totalNumberBees;
            this.numberInactive = numberInactive;
            this.numberActive = numberActive;
            this.numberScout = numberScout;
            this.maxNumberVisits = maxNumberVisits;
            this.maxNumberCycles = maxNumberCycles;
            this.storageId = storageId;
            this.foodType = foodType;
            this.foodValue = foodValue;

            this.storagesData = storagesData;

            this.bees = new Bee[totalNumberBees];
            this.bestMemoryMatrix = GenerateRandomMemoryMatrix();
            //Console.WriteLine(getStorageById(bestMemoryMatrix[bestMemoryMatrix.Count - 1]).getResources());
            foreach (Tuple<Resource, int> r in getStorageById(bestMemoryMatrix[bestMemoryMatrix.Count -1 ]).getResources())
            {
                //Console.WriteLine(r.Item1);
                if (r.Item1.getType() == foodType)
                {
                    this.bestResourceCost = r.Item2;
                }
            }
            this.bestMeasureOfQuality = MeasureOfQuality(this.bestMemoryMatrix) + this.bestResourceCost;

            this.indexesOfInactiveBees = new List<int>();
            foreach (int i in this.bestMemoryMatrix)
            {
                Console.Write(i);
            }
            Console.WriteLine();
            Console.WriteLine(this.bestMeasureOfQuality);
            Console.WriteLine("After .......................");
            for (int i = 0; i < totalNumberBees; ++i)
            {
                int currStatus;
                if (i < numberInactive)
                {
                    currStatus = 0; // inactive
                    indexesOfInactiveBees.Add(i);
                }
                else if (i < numberInactive + numberScout)
                    currStatus = 2; // scout
                else
                    currStatus = 1; // active

                List<int> randomMemoryMatrix = GenerateRandomMemoryMatrix();
                int randomResourceCost = 0;
                foreach (Tuple<Resource, int> r in getStorageById(randomMemoryMatrix[randomMemoryMatrix.Count - 1]).getResources())
                {
                    if (r.Item1.getType() == foodType)
                    {
                        randomResourceCost = r.Item2;
                    }
                }
                double mq = MeasureOfQuality(randomMemoryMatrix) + randomResourceCost;
                int numberOfVisits = 0;

                bees[i] = new Bee(currStatus, randomMemoryMatrix, mq, numberOfVisits);

                if (bees[i].measureOfQuality < bestMeasureOfQuality)
                {
                    this.bestMemoryMatrix = bees[i].memoryMatrix;
                    this.bestMeasureOfQuality = bees[i].measureOfQuality;
                }
            }
            foreach(int i in this.bestMemoryMatrix) {
                Console.Write(i);
            }
            Console.WriteLine();
            Console.WriteLine(this.bestMeasureOfQuality);
            
        }

        public Storage getStorageById(int id)
        {
            foreach (Storage s in storagesData.storages)
            {
                if (s.getId() == id)
                {
                    return s;
                }
            }
            return null;
        }

        public List<int> GenerateRandomMemoryMatrix()
        {
            List<int> result = new List<int>();
            List<Tuple<int, int>> hasPath = new List<Tuple<int, int>>();

            int tu = 0;
            foreach (Storage i in this.storagesData.storages)
            {
                if (storagesData.costs[storageId][i.getId()] != -1 && i.getId() != this.storageId)
                {
                    hasPath.Add(new Tuple<int, int>(i.getId(), tu));
                }
                tu++;
            }
 
                List<Storage> storagestemp = new List<Storage>(this.storagesData.storages);
                tu = -1;
                int m = random.Next(1, this.storagesData.storages.Count);
                result.Add(this.storageId);
                List<Tuple<int, int>> hasResourceIndex = new List<Tuple<int, int>>();
                for (int i = 0; i < m; i++)
                {
                    if (hasPath.Count == 0) break;
                    int r = random.Next(0, hasPath.Count);
                    //Console.WriteLine("DEBUG:   " + hasPath.Count);
                    result.Add(hasPath[r].Item1);
                    if (tu != -1)
                    {
                        storagestemp.RemoveAt(hasPath[r].Item2);
                    }
                    if (storagesData.hasStorageResource[hasPath[r].Item1][this.foodType - 1]) /* TODO : Check for the value */
                    {
                        hasResourceIndex.Add(new Tuple<int, int>(hasPath[r].Item1, i + 1));
                    }
                    hasPath.Clear();
                    tu = 0;
                    foreach (Storage f in storagestemp)
                    {
                        if (storagesData.costs[result[i + 1]][f.getId()] != -1 && f.getId() != this.storageId && f.getId() != result[i + 1])
                        {
                            hasPath.Add(new Tuple<int, int>(f.getId(), tu));
                        }
                        tu++;
                    }
                }



                if (storagesData.hasStorageResource[result[result.Count - 1]][this.foodType - 1])
                {
                    /*foreach (int i in result)
                    {
                        Console.Write(i);
                    }
                    Console.WriteLine();*/
                    return result;
                }
                else
                {
                    if (hasResourceIndex.Count > 0)
                    {
                        /*Console.Write("has array :   ");
                        foreach (Tuple<int, int> i in hasResourceIndex)
                        {
                            Console.Write(i.Item1+ " ");
                        }
                        Console.WriteLine();*/
                        int r = random.Next(0, hasResourceIndex.Count);
                        int lastIndx = hasResourceIndex[r].Item2;
                        //result.RemoveRange(lastIndx + 1, result.Count-lastIndx+1);
                        for (int t = result.Count - 1; t >= lastIndx + 1; t--)
                        {
                            result.RemoveAt(t);
                        }
                        if (result.Count > 1)
                        {
                            /*Console.Write("else :   ");
                            foreach (int i in result)
                            {
                                Console.Write(i);
                            }
                            Console.WriteLine();*/
                            return result;
                        }
                        else
                        {
                            return GenerateRandomMemoryMatrix();
                        }
                    }
                    else
                    {
                        return GenerateRandomMemoryMatrix();
                    }

                }
            
        }

        public List<int> GenerateNeighborMemoryMatrix(List<int> memoryMatrix)
        {
            if (memoryMatrix.Count > 2)
            {
                List<int> result = new List<int>(memoryMatrix);
                HashSet<int> ex = new HashSet<int>() { 0 };
                for (int i = 0; i < result.Count; i++)
                {
                    if (ex.Count == result.Count - 1)
                    {
                        return memoryMatrix;
                    }
                    int ranIndex = GiveMeANumber(ex, result.Count - 1);
                    ex.Add(ranIndex);
                    int adjIndex;
                    if (ranIndex == result.Count - 1)
                        adjIndex = 0;
                    else
                        adjIndex = ranIndex + 1;

                    int nextStorage = -1, preStorage = -1;

                    if (ranIndex != 0)
                        preStorage = ranIndex - 1;
                    if (adjIndex != result.Count - 1)
                        nextStorage = adjIndex + 1;
                    if ((nextStorage == -1 || storagesData.costs[result[ranIndex]][result[nextStorage]] != -1))
                    {

                        if ((preStorage == -1 || storagesData.costs[result[preStorage]][result[adjIndex]] != -1))
                        {
                            int tmp = result[ranIndex];
                            result[ranIndex] = result[adjIndex];
                            result[adjIndex] = tmp;
                            break;
                        }

                    }

                }

                return result;
            }
            return memoryMatrix;
        }

        private static int GiveMeANumber(HashSet<int> ex, int upperBound)
        {
            //Console.WriteLine("DEBUG:   " + ex.Count + "     "  + upperBound);
            
            var exclude = new HashSet<int>(ex);
            var range = Enumerable.Range(0, upperBound).Where(i => !exclude.Contains(i));
            //for (int i = 0; i < range.Count(); i++ )
                //Console.Write("DEBUG:   " + range.ElementAt(i));
            //Console.WriteLine();
            var rand = new System.Random();
            int index = rand.Next(0, upperBound - exclude.Count);
            //Console.WriteLine("DEBUG:  " + range.Count());
            return range.ElementAt(index);
        }

        public double MeasureOfQuality(List<int> memoryMatrix)
        {
            /***
                Most time consuming method
                Real-world simulation would be done in StoragesData.cost method 
            ***/
            double answer = 0.0;
            for (int i = 0; i < memoryMatrix.Count - 1; ++i)
            {
                int c1 = memoryMatrix[i];
                int c2 = memoryMatrix[i + 1];
                double d = this.storagesData.cost(c1, c2);
                answer += d;
            }
            
            return answer;
        }

        public void Solve()
        {
            int cycle = 0;

            while (cycle < this.maxNumberCycles)
            {
                for (int i = 0; i < totalNumberBees; ++i)
                {
                    if (this.bees[i].status == 1)
                        ProcessActiveBee(i);
                    else if (this.bees[i].status == 2)
                        ProcessScoutBee(i);
                    else if (this.bees[i].status == 0)
                        ProcessInactiveBee(i);
                }
                ++cycle;
            }
        }

        private void ProcessActiveBee(int i)
        {
            List<int> neighbor = GenerateNeighborMemoryMatrix(bees[i].memoryMatrix);
            double neighborQuality = MeasureOfQuality(neighbor);
            double prob = random.NextDouble();
            bool memoryWasUpdated = false;
            bool numberOfVisitsOverLimit = false;

            if (neighborQuality < bees[i].measureOfQuality)
            { // better
                if (prob < probMistake)
                { // mistake
                    ++bees[i].numberOfVisits;
                    if (bees[i].numberOfVisits > maxNumberVisits)
                        numberOfVisitsOverLimit = true;
                }
                else
                { // No mistake
                    bees[i].memoryMatrix = neighbor;
                    bees[i].measureOfQuality = neighborQuality;
                    bees[i].numberOfVisits = 0;
                    memoryWasUpdated = true;
                }
            }
            else
            { // Did not find better neighbor
                if (prob < probMistake)
                { // Mistake
                    bees[i].memoryMatrix = neighbor;
                    bees[i].measureOfQuality = neighborQuality;
                    bees[i].numberOfVisits = 0;
                    memoryWasUpdated = true;
                }
                else
                { // No mistake
                    ++bees[i].numberOfVisits;
                    if (bees[i].numberOfVisits > maxNumberVisits)
                        numberOfVisitsOverLimit = true;
                }
            }

            if (numberOfVisitsOverLimit == true)
            {
                bees[i].status = 0;
                bees[i].numberOfVisits = 0;
                int x = random.Next(numberInactive);
                bees[indexesOfInactiveBees[x]].status = 1;
                indexesOfInactiveBees[x] = i;
            }
            else if (memoryWasUpdated == true)
            {
                if (bees[i].measureOfQuality < this.bestMeasureOfQuality)
                {
                    this.bestMemoryMatrix = bees[i].memoryMatrix;
                    this.bestMeasureOfQuality = bees[i].measureOfQuality;
                }
                DoWaggleDance(i);
            }
            else
            {
                return;
            }
        }

        private void ProcessScoutBee(int i)
        {
            List<int> randomFoodSource = GenerateRandomMemoryMatrix();
            int randomResourceCost = 0;
            foreach (Tuple<Resource, int> r in getStorageById(randomFoodSource[randomFoodSource.Count - 1]).getResources())
            {
                if (r.Item1.getType() == foodType)
                {
                    randomResourceCost = r.Item2;
                }
            }
            double randomFoodSourceQuality = MeasureOfQuality(randomFoodSource) + randomResourceCost;
            if (randomFoodSourceQuality < bees[i].measureOfQuality)
            { // NO Mistake
                bees[i].memoryMatrix = randomFoodSource;
                bees[i].measureOfQuality = randomFoodSourceQuality;
                if (bees[i].measureOfQuality < bestMeasureOfQuality)
                {
                    this.bestMemoryMatrix = bees[i].memoryMatrix;
                    this.bestMeasureOfQuality = bees[i].measureOfQuality;
                }
                DoWaggleDance(i);
            }
        }

        private void ProcessInactiveBee(int i)
        {
            // problem dependent logic
            return;
        }

        public void printSolution()
        {
            foreach(int i in bestMemoryMatrix) {
                Console.Write(i);
            }
            Console.WriteLine();
        }

        private void DoWaggleDance(int i)
        {
            for (int ii = 0; ii < numberInactive; ++ii)
            {
                int b = indexesOfInactiveBees[ii];
                if (bees[i].measureOfQuality < bees[b].measureOfQuality)
                {
                    double p = random.NextDouble();
                    if (this.probPersuasion > p)
                    {
                        bees[b].memoryMatrix = bees[i].memoryMatrix;
                        bees[b].measureOfQuality = bees[i].measureOfQuality;
                    }
                }
            }
        }
    }
}
