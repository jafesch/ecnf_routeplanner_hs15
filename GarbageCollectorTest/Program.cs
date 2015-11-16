using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarbageCollectorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            * Default
            * Ellapsed=750
            *
            * Forced
            * Ellapsed=768
            *
            * Optimized
            * Ellapsed=784
            */
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < 100; i++)
                {
                    string[] stringArray = new string[10000000];
                }
            }

            GC.Collect();
            //GC.Collect(2, GCCollectionMode.Forced);
            //GC.Collect(2, GCCollectionMode.Optimized);
            
            stopWatch.Stop();
            Console.WriteLine("Elapsed={0}", stopWatch.ElapsedMilliseconds / 10);
            Console.ReadLine();
        }
    }
}
