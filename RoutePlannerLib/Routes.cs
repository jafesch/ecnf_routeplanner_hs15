using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
	///	<summary>
	///	Manages	a routes from a	city to	another	city.
	///	</summary>
	public class Routes : IRoutes
	{
        public delegate void RouteRequestHandler(object _sender, RouteRequestEventArgs _e);
        public event RouteRequestHandler RouteRequested;

        private List<Link> routes = new List<Link>();
		private Cities cities;
        private static readonly TraceSource traceSource = new TraceSource("Routes");
        private static readonly TraceSource traceSourceErrors = new TraceSource("RoutesErrors");

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
            try { 
			    using (var reader = new StreamReader(_filename))
			    {
                    traceSource.TraceInformation("ReadRoutes started");
                    traceSource.Flush();

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
                traceSource.TraceInformation("ReadRoutes ended");
                traceSource.Flush();
                return Count;
            }
            catch (FileNotFoundException e)
            {
                traceSourceErrors.TraceData(TraceEventType.Critical, 1, e.StackTrace);
                traceSource.Flush();
                throw (e);
            }
        }

        public City[] FindCities(TransportMode transportMode)
        {
            return routes.Where(r1 => r1.TransportMode == transportMode)
                .SelectMany(r2 => new[] { r2.FromCity, r2.ToCity })
                .Distinct()
                .ToArray();
        }

        public IEnumerable<TransportMode> AllTransportModes()
        {
            return Enum.GetValues(typeof(TransportMode)).Cast<TransportMode>();
        }

        public List<List<Link>> FindAllShortestRoutes()
        {
            return AllTransportModes()
                 .SelectMany(tm => Enumerable.Range(0, cities.Count)
                     .SelectMany(n => Enumerable.Range(0, cities.Count)
                         .Select(k => FindShortestRouteBetween(cities[n].Name, cities[k].Name, tm))
                     )
                 ).ToList();
        }

        public List<List<Link>> FindAllShortestRoutesParallel()
        {
            return AllTransportModes()
                .SelectMany(tm => Enumerable.Range(0, cities.Count)
                    .SelectMany(n => Enumerable.Range(0, cities.Count)
                        .Select(k => FindShortestRouteBetween(cities[n].Name, cities[k].Name, tm)).AsParallel()
                    )
                ).ToList();
        }

        #region Lab04: Dijkstra implementation
        public List<Link> FindShortestRouteBetween(string _fromCity, string _toCity, TransportMode _mode, IProgress<string> _reportProgress)
        {
            //inform listeners
            RouteRequested?.Invoke(this, new RouteRequestEventArgs(cities[_fromCity], cities[_toCity], _mode));
            if (_reportProgress != null) { _reportProgress.Report("Route request done"); }

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
            if (_reportProgress != null) { _reportProgress.Report("Creating dijkstra nodes done"); }

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
            if (_reportProgress != null) { _reportProgress.Report("Finding route done"); }

            //create a list of cities that we passed along the way
            var citiesEnRoute = new List<City>();
            for (var c = cities[_toCity]; c != null; c = visited[c].PreviousCity)
                citiesEnRoute.Add(c);
            citiesEnRoute.Reverse();
            if (_reportProgress != null) { _reportProgress.Report("Creating city list done"); }

            //convert that city-list into a list of links
            IEnumerable<Link> paths = ConvertListOfCitiesToListOfLinks(citiesEnRoute);
            if (_reportProgress != null) { _reportProgress.Report("Converting city list to link list done"); }
            return paths.ToList();
        }

        public List<Link> FindShortestRouteBetween(string _fromCity, string _toCity, TransportMode _mode)
        {
            return FindShortestRouteBetween(_fromCity, _toCity, _mode, null);
        }

        public async Task<List<Link>> FindShortestRouteBetweenAsync(string _fromCity, string _toCity, TransportMode _mode, IProgress<string> _reportProgress)
        {
            return await Task.Run(() => FindShortestRouteBetween(_fromCity, _toCity, _mode, _reportProgress));
        }

        public async Task<List<Link>> FindShortestRouteBetweenAsync(string _fromCity, string _toCity, TransportMode _mode)
        {
            return await FindShortestRouteBetweenAsync(_fromCity, _toCity, _mode, null);
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
