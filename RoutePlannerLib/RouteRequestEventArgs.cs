using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RouteRequestEventArgs : EventArgs
    {
        public City FromCity { get; }
        public City ToCity { get; }
        public TransportMode Mode;

        public RouteRequestEventArgs(City fromCity, City toCity, TransportMode mode)
        {
            this.FromCity = fromCity;
            this.ToCity = toCity;
            this.Mode = mode;
        }
    }
}
