using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RouteRequestWatcher
    {
        Dictionary<City, int> count;

        public RouteRequestWatcher()
        {
            count = new Dictionary<City, int>();
        }

        public void LogRouteRequests(object sender, RouteRequestEventArgs e)
        {
            if (count.ContainsKey(e.ToCity))
            {
                count[e.ToCity]++;
            }
            else
            {
                count[e.ToCity] = 1;
            }

            Console.WriteLine("Current Request State");
            Console.WriteLine("---------------------");

            foreach (var pair in count)
            {
                Console.WriteLine("ToCity: " + pair.Key.Name + " has been requested " + pair.Value + " times");
            }

            Console.WriteLine();

        }

        public int GetCityRequests(City _city)
        {
            return count[_city];
        }
    }
}
