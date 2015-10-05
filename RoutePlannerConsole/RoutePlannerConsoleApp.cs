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

            //Lab2 Aufgabe 2b - FunktionsTest - Funktioniert
            Cities c = new Cities();
            c.ReadCities("citiesTestDataLab2.txt");
            Console.WriteLine("Test: {0}", c[5].Location.Latitude);

            var target = new WayPoint("Windisch", 0.564, 0.646);
            Console.WriteLine(target.ToString() + " vs. " + "WayPoint: Windisch 0.56/0.65");

            //Lab3 Aufgabe 2
            var reqWatch = new RouteRequestWatcher();

            var routeCities = new Cities();
            routeCities.ReadCities("citiesTestDataLab2.txt");

            var routes = new Routes(routeCities);

            routes.RouteRequested += reqWatch.LogRouteRequests;

            routes.FindShortestRouteBetween("mumbai", "buenos aires", TransportMode.Rail);
            routes.FindShortestRouteBetween("dilli", "mumbai", TransportMode.Rail);
            routes.FindShortestRouteBetween("mumbai", "buenos aires", TransportMode.Rail);

            Console.ReadLine();
        }
    }
}
