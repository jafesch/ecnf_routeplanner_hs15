using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    class Cities
    {
        List<City> cities = new List<City>();
        public int ReadCities(string _filename)
        {
            try
            {
                using (TextReader reader = new StreamReader(_filename))
                {
                    IEnumerable<string[]> citiesAsStrings = reader. .GetSplittedLines('\t');
                    IEnumerable<City> c = citiesAsStrings.Select(city => new City(city[0].ToString(), city[1].ToString(), int.Parse(city[2]), double.Parse(city[3], CultureInfo.InvariantCulture), double.Parse(city[4], CultureInfo.InvariantCulture))).ToArray();
                    cities.AddRange(c);
                    return c.Count();
                }
            }
            catch (FileNotFoundException e)
            {
            }
            return -1;
        }
    }
}
