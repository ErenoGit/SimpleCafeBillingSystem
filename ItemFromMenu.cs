using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeBillingSystem
{
    public class ItemFromMenu
    {
        public ItemFromMenu(string name, decimal price, string type)
        {
            Name = name;
            Price = price;
            Type = type;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }

    }
}
