using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Dynamic
{
    public class World : DynamicObject
    {
        private Cities _cities;

        public World(Cities cities)
        {
            this._cities = cities;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            try
            {
                result = _cities[binder.Name];
            } catch
            {
                result = string.Format("The city \"{0}\" does not exist!", binder.Name);
            }
            return true;
        }
    }
}
