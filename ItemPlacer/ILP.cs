using ItemChanger;
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
        public readonly bool editCost;
        public readonly int cost;
        public readonly Location.CostType costType;

        public ILP(string _item, string _location, int _cost = 0, string _costType = null)
        {
            item = _item;
            location = _location;
            editCost = !string.IsNullOrEmpty(_costType);

            if (editCost && Enum.TryParse(_costType, out Location.CostType _type))
            {
                if (_type != Location.CostType.None)
                {
                    cost = _cost;
                    costType = _type;
                }
            }
            else editCost = false;
        }

        public override string ToString()
        {
            return $"Item {item} at location {location}" + (editCost ? $" with new cost {cost} of type {costType}" : string.Empty);
        }
    }
}
