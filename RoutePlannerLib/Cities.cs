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
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(_filename);
            var citiesAdded = 0;
            while ((line = file.ReadLine()) != null)    // Read the file and display it line by line.
            {
                //Console.WriteLine(line);
                string[] splited = line.Split('\t');
                int _tempPop = Int32.Parse(splited[2], CultureInfo.InvariantCulture);
                double _tempLati = Double.Parse(splited[3], CultureInfo.InvariantCulture);
                double _tempLong = Double.Parse(splited[4], CultureInfo.InvariantCulture);
                cities.Add(new City(splited[0], splited[1], _tempPop, _tempLati, _tempLong));
                citiesAdded++;
            }

            file.Close();
            return citiesAdded;
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
