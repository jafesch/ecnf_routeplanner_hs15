using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
	///	<summary>
	///	Manages	a routes from a	city to	another	city.
	///	</summary>
	public class Routes
	{
        public delegate void RouteRequestHandler(object _sender, RouteRequestEventArgs _e);
        public event RouteRequestHandler RouteRequested;

        private List<Link> routes = new List<Link>();
		private Cities cities;

		public int Count
		{
			get
			{
				return routes.Count;
			}
		}

		///	<summary>
		///	Initializes	the	Routes with	the	cities.
		///	</summary>
		///	<param name="cities"></param>
		public Routes(Cities _cities)
		{
			this.cities = _cities;
		}

		///	<summary>
		///	Reads a	list of	links from the given file.
		///	Reads only links where the cities exist.
		///	</summary>
		///	<param name="filename">name	of links file</param>
		///	<returns>number	of read	route</returns>
		public int ReadRoutes(string _filename)
		{
			using (var reader = new StreamReader(_filename))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					var tokens = line.Split('\t');

                    City city1;
                    City city2;
                    try
                    {
                        city1 = cities[tokens[0]];
                        city2 = cities[tokens[1]];
                    }
                    catch (Exception)
                    {
                        city1 = null;
                        city2 = null;
                    }
					
					
					// only add links, where both cities are found 
					if ((city1 != null)	&& (city2 != null))
						routes.Add(new Link(city1, city2, city1.Location.Distance(city2.Location), TransportMode.Rail));
				}
			}
			
			return Count;
		}

		//public List<Link> FindShortestRouteBetween(string _fromCity, string _toCity, TransportMode _mode)
		//{
  //          RouteRequested?.Invoke(this, new RouteRequestEventArgs(cities[_fromCity], cities[_toCity], _mode));
  //          return new List<Link>();
		//}

        #region Lab04: Dijkstra implementation
        public List<Link> FindShortestRouteBetween(string _fromCity, string _toCity, TransportMode _mode)
        {
            //inform listeners
            RouteRequested?.Invoke(this, new RouteRequestEventArgs(cities[_fromCity], cities[_toCity], _mode));

            //use dijkstra's algorithm to look for all single-source shortest paths
            var visited = new Dictionary<City, DijkstraNode>();
            var pending = new SortedSet<DijkstraNode>(new DijkstraNode[]
            {
                new DijkstraNode()
                {
                    VisitingCity = cities[_fromCity],
                    Distance = 0
                }
            });

            while (pending.Any())
            {
                var cur = pending.Last();
                pending.Remove(cur);

                if (!visited.ContainsKey(cur.VisitingCity))
                {
                    visited[cur.VisitingCity] = cur;

                    foreach (Link link in GetListOfAllOutgoingRoutes(cur.VisitingCity, _mode))
                        pending.Add(new DijkstraNode()
                        {
                            VisitingCity = (link.FromCity == cur.VisitingCity) ? link.ToCity : link.FromCity,
                            Distance = cur.Distance + link.Distance,
                            PreviousCity = cur.VisitingCity
                        });
                }
            }

            //did we find any route?
            if (!visited.ContainsKey(cities[_toCity]))
                return null;

            //create a list of cities that we passed along the way
            var citiesEnRoute = new List<City>();
            for (var c = cities[_toCity]; c != null; c = visited[c].PreviousCity)
                citiesEnRoute.Add(c);
            citiesEnRoute.Reverse();

            //convert that city-list into a list of links
            IEnumerable<Link> paths = ConvertListOfCitiesToListOfLinks(citiesEnRoute);
            return paths.ToList();
        }

        private IEnumerable<Link> ConvertListOfCitiesToListOfLinks(List<City> citiesEnRoute)
        {
            var links = new List<Link>();
            for (int i = 0; i < citiesEnRoute.Count - 1; i++)
            {
                links.Add(new Link(citiesEnRoute[i], citiesEnRoute[i + 1], citiesEnRoute[i].Location.Distance(citiesEnRoute[i + 1].Location)));
            }

            return links;
        }

        private IEnumerable<object> GetListOfAllOutgoingRoutes(City visitingCity, TransportMode _mode)
        {
            var outgoingRoutes = new List<Link>();

            foreach (var r in routes)
            {
                if (_mode.Equals(r.TransportMode))
                {
                    if (visitingCity.Equals(r.FromCity) || visitingCity.Equals(r.ToCity))
                        outgoingRoutes.Add(new Link(r.FromCity, r.ToCity, r.Distance));
                }
            }
            return outgoingRoutes;
        }

        private class DijkstraNode : IComparable<DijkstraNode>
        {
            public City VisitingCity;
            public double Distance;
            public City PreviousCity;

            public int CompareTo(DijkstraNode other)
            {
                return other.Distance.CompareTo(Distance);
            }
        }
        #endregion
    }
}
