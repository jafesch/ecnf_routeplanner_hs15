using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class Cities
    {
        private static readonly TraceSource traceSource = new TraceSource("Cities");
        private static readonly TraceSource traceSourceErrors = new TraceSource("CitiesErrors");

        List<City> cities = new List<City>();
        public int Count { get { return cities.Count; } }

        public int ReadCities(string _filename)
        {
            traceSource.TraceInformation("ReadCities started");
            traceSource.Flush();
            try
            {
                using (TextReader reader = new StreamReader(_filename))
                {
                    var citiesAsStrings = reader.GetSplittedLines('\t');
                    var c = citiesAsStrings.Select(city => new City(city[0].ToString(), city[1].ToString(), int.Parse(city[2]),
                        double.Parse(city[3], CultureInfo.InvariantCulture), double.Parse(city[4], CultureInfo.InvariantCulture))).ToArray();
                    cities.AddRange(c);
                    traceSource.TraceInformation("ReadCities ended");
                    traceSource.Flush();
                    traceSource.Close();
                    return c.Count();
                }
            }
            catch (FileNotFoundException e)
            {
                traceSourceErrors.TraceData(TraceEventType.Critical, 1, e.StackTrace);
                traceSource.Flush();
            }
            return -1;
        }

        public City this[int i]
        {
            get
            {
                if (i >= 0 && i < Count) {
                    return cities[i];
                } else {
                    throw new IndexOutOfRangeException("Index not available!");
                }
            }
        }

        public City this[string _cityName]
        {
            get
            {
                var foundCity = cities.Find(delegate(City c)
                {
                    return c.Name.Equals(_cityName, StringComparison.InvariantCultureIgnoreCase);
                });

                if (foundCity != null)
                {
                    return foundCity;
                }
                else
                {
                    throw new KeyNotFoundException("City not found");
                }
            }
        }

        public IEnumerable<City> FindNeighbours(WayPoint _location, double _distance)
        {
            return cities.Where(c => _location.Distance(c.Location) <= _distance).ToList();
        }
    }
}
