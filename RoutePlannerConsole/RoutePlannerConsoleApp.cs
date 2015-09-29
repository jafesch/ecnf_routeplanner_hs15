using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerConsole
{
    class RoutePlannerConsoleApp
    {
        static void Main(string[] args)
        {
            //Lab1 Aufgabe 1
            Console.WriteLine("Welcome to RoutePlanner (Version " + Assembly.GetExecutingAssembly().GetName().Version + ")");

            //Lab1 Aufgabe 2d
            var wayPoint = new WayPoint("Windisch", 47.479319847061966, 8.212966918945312);
            Console.WriteLine("{0}: {1}/{2}", wayPoint.Name, wayPoint.Latitude, wayPoint.Longitude);
            Console.WriteLine(wayPoint);

            //Lab2 Aufgabe 1a
            Console.WriteLine(wayPoint);
            var wayPoint1 = new WayPoint("", 47.479319847061966, 8.212966918945312);
            var wayPoint2 = new WayPoint(null, 47.479319847061966, 8.212966918945312);
            Console.WriteLine(wayPoint1);
            Console.WriteLine(wayPoint2);

            //Lab2 Aufgabe 1b
            var bern = new WayPoint("Bern", 46.948342, 7.442935);
            var tripolis = new WayPoint("Tripolis", 32.808858, 13.098922);
            Console.WriteLine(bern.Distance(tripolis));
            Console.WriteLine(tripolis.Distance(bern));



            Console.ReadLine();
        }
    }
}
