using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class WayPoint
    {
        private const double EARTHRADIUS = 6371d;
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public WayPoint(string _name, double _latitude, double _longitude)
        {
            Name = _name;
            Latitude = _latitude;
            Longitude = _longitude;
        }

        public static WayPoint operator +(WayPoint lhs, WayPoint rhs)
        {
            return new WayPoint(lhs.Name, lhs.Latitude + rhs.Latitude, lhs.Longitude + rhs.Longitude);
        }

        public static WayPoint operator -(WayPoint lhs, WayPoint rhs)
        {
            return new WayPoint(lhs.Name, lhs.Latitude - rhs.Latitude, lhs.Longitude - rhs.Longitude);
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return String.Format(CultureInfo.InvariantCulture, "WayPoint: {0:N2}/{1:N2}", Latitude, Longitude);
            }
            else
            {
                return String.Format(CultureInfo.InvariantCulture, "WayPoint: {0} {1:N2}/{2:N2}", Name, Latitude, Longitude);
            }

            /*if (Name == null || Name == "")*/
        }

        public double Distance(WayPoint _target)
        {
            var phiA = DegRad(this.Latitude);
            var phiB = DegRad(_target.Latitude);
            var lambdaA = DegRad(this.Longitude);
            var lambdaB = DegRad(_target.Longitude);
            return EARTHRADIUS * Math.Acos(Math.Sin(phiA) * Math.Sin(phiB) + Math.Cos(phiA) * Math.Cos(phiB) * Math.Cos(lambdaA - lambdaB));
        }
        private static double DegRad(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}
