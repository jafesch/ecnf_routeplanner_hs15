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
					
					var city1 = cities[tokens[0]];
					var city2 = cities[tokens[1]];
					
					// only add links, where both cities are found 
					if ((city1 != null)	&& (city2 != null))
						routes.Add(new Link(city1, city2, city1.Location.Distance(city2.Location), TransportMode.Rail));
				}
			}
			
			return Count;
		}

		public List<Link> FindShortestRouteBetween(string _fromCity, string _toCity, TransportMode _mode)
		{
            RouteRequested?.Invoke(this, new RouteRequestEventArgs(cities[_fromCity], cities[_toCity], _mode));
            return new List<Link>();
		}
	}
}
