using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class City
    {
        public string Country { get; set; }
        public WayPoint Location { get; set; }
        public string Name { get; set; }
        public int Population { get; set; }

        public City() { }

        public City(string _name, string _country, int _population, double _latitude, double _longitude)
        {
            Name = _name;
            Country = _country;
            Population = _population;
            Location = new WayPoint(_name, _latitude, _longitude);
        }

        public override bool Equals(object obj)
        {
            var city = obj as City;
            if (city == null) return false;
            return Name.Equals(city.Name, StringComparison.InvariantCultureIgnoreCase) && Country.Equals(city.Country, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
