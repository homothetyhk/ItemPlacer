using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemPlacer
{
    internal class ILP
    {
        public readonly string item;
        public readonly string location;
        public readonly int cost;

        public ILP(string _item, string _location, int _cost)
        {
            item = _item;
            location = _location;
            cost = _cost;
        }

        public override string ToString()
        {
            return $"Item {item} at location {location}" + (cost != 0 ? $" with cost {cost}" : string.Empty);
        }
    }
}
