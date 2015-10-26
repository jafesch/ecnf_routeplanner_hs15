using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class Cities
    {
        List<City> cities = new List<City>();
        public int Count { get { return cities.Count; } }

        public int ReadCities(string _filename)
        {
            using (TextReader reader = new StreamReader(_filename))
            {
                IEnumerable<string[]> citiesAsStrings = reader.GetSplittedLines('\t');
                var citiesAdded = 0;

                foreach (var line in citiesAsStrings)
                {
                    int _tempPop = Int32.Parse(line[2], CultureInfo.InvariantCulture);
                    double _tempLati = Double.Parse(line[3], CultureInfo.InvariantCulture);
                    double _tempLong = Double.Parse(line[4], CultureInfo.InvariantCulture);
                    cities.Add(new City(line[0], line[1], _tempPop, _tempLati, _tempLong));
                    citiesAdded++;
                }

                return citiesAdded;
            }
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
            List<City> citiesTemp = new List<City>();
            for (int i = 0; i < cities.Count; i++)
            {
                if (_location.Distance(cities[i].Location) <= _distance)
                {
                    citiesTemp.Add(cities[i]);
                }
            }

            return citiesTemp;
        }
    }
}
