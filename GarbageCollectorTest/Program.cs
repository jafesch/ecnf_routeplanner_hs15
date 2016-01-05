using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GarbageCollector
{
    public class Program
    {
        private static Stopwatch stopWatch = new Stopwatch();

        public static void Main(string[] args)
        {
            Console.WriteLine("0 Def blocking: " + MeasureMilisElapsed(0, GCCollectionMode.Default, true));
            Console.WriteLine("1 Def blocking: " + MeasureMilisElapsed(1, GCCollectionMode.Default, true));
            Console.WriteLine("2 Def blocking: " + MeasureMilisElapsed(2, GCCollectionMode.Default, true));

            Console.WriteLine("0 Def not blocking: " + MeasureMilisElapsed(0, GCCollectionMode.Default, false));
            Console.WriteLine("1 Def not blocking: " + MeasureMilisElapsed(1, GCCollectionMode.Default, false));
            Console.WriteLine("2 Def not blocking: " + MeasureMilisElapsed(2, GCCollectionMode.Default, false));

            Console.WriteLine("------------------");

            Console.WriteLine("0 For blocking: " + MeasureMilisElapsed(0, GCCollectionMode.Forced, true));
            Console.WriteLine("1 For blocking: " + MeasureMilisElapsed(1, GCCollectionMode.Forced, true));
            Console.WriteLine("2 For blocking: " + MeasureMilisElapsed(2, GCCollectionMode.Forced, true));

            Console.WriteLine("0 For not blocking: " + MeasureMilisElapsed(0, GCCollectionMode.Forced, false));
            Console.WriteLine("1 For not blocking: " + MeasureMilisElapsed(1, GCCollectionMode.Forced, false));
            Console.WriteLine("2 For not blocking: " + MeasureMilisElapsed(2, GCCollectionMode.Forced, false));

            Console.WriteLine("------------------");

            Console.WriteLine("0 Opt blocking: " + MeasureMilisElapsed(0, GCCollectionMode.Optimized, true));
            Console.WriteLine("1 Opt blocking: " + MeasureMilisElapsed(1, GCCollectionMode.Optimized, true));
            Console.WriteLine("2 Opt blocking: " + MeasureMilisElapsed(2, GCCollectionMode.Optimized, true));

            Console.WriteLine("0 Opt not blocking: " + MeasureMilisElapsed(0, GCCollectionMode.Optimized, false));
            Console.WriteLine("1 Opt not blocking: " + MeasureMilisElapsed(1, GCCollectionMode.Optimized, false));
            Console.WriteLine("2 Opt not blocking: " + MeasureMilisElapsed(2, GCCollectionMode.Optimized, false));

            Console.WriteLine("------------------");

            //Console.WriteLine("0 Def blocking " + MeasureMilisElapsed(0, GCCollectionMode.Default, true));

            /*

            big objects:

            0 Def blocking: 0
            1 Def blocking: 8
            2 Def blocking: 48
            0 Def not blocking: 15
            1 Def not blocking: 0
            2 Def not blocking: 0
            ------------------
            0 For blocking: 1
            1 For blocking: 46
            2 For blocking: 444
            0 For not blocking: 0
            1 For not blocking: 0
            2 For not blocking: 0
            ------------------
            0 Opt blocking: 0
            1 Opt blocking: 0
            2 Opt blocking: 0
            0 Opt not blocking: 1
            1 Opt not blocking: 14
            2 Opt not blocking: 0
            ------------------

            small objects:

            0 Def blocking: 2
            1 Def blocking: 10
            2 Def blocking: 8
            0 Def not blocking: 4
            1 Def not blocking: 10
            2 Def not blocking: 8
            ------------------
            0 For blocking: 2
            1 For blocking: 9
            2 For blocking: 8
            0 For not blocking: 2
            1 For not blocking: 8
            2 For not blocking: 8
            ------------------
            0 Opt blocking: 0
            1 Opt blocking: 7
            2 Opt blocking: 8
            0 Opt not blocking: 0
            1 Opt not blocking: 7
            2 Opt not blocking: 8
            ------------------

            */

            Console.ReadLine();
        }

        public static long MeasureMilisElapsed(int generation, GCCollectionMode mode, bool blocking)
        {
            var stringArray = new Object[10000];
            for (int i = 0; i < 10000; i++)
            {
                //stringArray[i] = new string[100]; //small objects
                stringArray[i] = new string[10000]; // big objects
            }
            stringArray = null;

            stopWatch.Start();
            GC.Collect(generation, mode, blocking);
            stopWatch.Stop();

            var elapsed = stopWatch.ElapsedMilliseconds;
            stopWatch.Reset();
            return elapsed;
        }
    }
}