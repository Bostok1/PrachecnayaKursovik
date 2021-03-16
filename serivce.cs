using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prachecnaya
{
    class serivce
    {
        private uint id; 
        private string name;
        private uint price;
        public serivce(uint id, string name, uint price)
        {
            this.id = id;
            this.name = name;
            this.price = price;
        }
        public string getName()
        {
            return name;
        }
        public uint getPrice()
        {
            return price;
        }
    }

}
