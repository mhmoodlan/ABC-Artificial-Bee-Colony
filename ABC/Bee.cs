using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC
{
    class Bee
    {
        public int status; /* 0: inactive, 1: active, 2: scout */
        public List<int> memoryMatrix; /* the solution */
        public double measureOfQuality; /* the fitness of the solution */
        public int numberOfVisits; /* simulates when the food source is used up */

        public Bee(int status, List<int> memoryMatrix, double measureOfQuality, int numberOfVisits)
        {
            this.status = status;
            this.memoryMatrix = new List<int>(memoryMatrix);
            this.measureOfQuality = measureOfQuality;
            this.numberOfVisits = numberOfVisits;
        }
    }
}
