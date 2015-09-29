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
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(_filename);
            while ((line = file.ReadLine()) != null)    // Read the file and display it line by line.
            {
                //Console.WriteLine(line);
                string[] splited = line.Split();
                cities.Add(new City(splited[0], splited[1], Int32.Parse(splited[2]), Double.Parse(splited[3]), Double.Parse(splited[4])));
            }

            file.Close();
            return cities.Count;
        }
    }
}
