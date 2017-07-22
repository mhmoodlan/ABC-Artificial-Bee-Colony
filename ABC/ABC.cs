using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC
{
    class ABC
    {
        static int totalNumberBees = 200;//old 500
        static int numberInactive = 20;
        static int numberActive = 50;
        static int numberScout = 30;
        static int maxNumberVisits = 50;//old 100
        static int maxNumberCycles = 500;//old 3460
        static int storageId = 6;
        static int foodType = 2;
        static int foodValue = 10;

        static void Main(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("ABC Started...");
            // Storages
            StoragesData storagesData = new StoragesData(9);

            // Hive
            Hive hive = new Hive(totalNumberBees, numberInactive, numberActive, numberScout, maxNumberVisits, maxNumberCycles, storagesData, storageId, foodType, foodValue);
            hive.Solve();
            Console.WriteLine("ABC Ended...");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("ABC Solution :");
            hive.printSolution();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Time elapsed: " + elapsedMs);
            string s = Console.ReadLine();
        }
    }
}
