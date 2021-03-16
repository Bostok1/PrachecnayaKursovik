using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prachecnaya
{
    class order
    {
        public uint id;
        public List<serivce> serivces= new List<serivce>();
        private uint price;
        private string dateIn ;
       private string dateOut;
        public bool isReady=false;
        public bool isTake;
        
        public order(uint id,List<serivce> pampam  )
        {
            this.id = id;
           
           this.dateIn = DateTime.Now.ToString();
            this.isReady = false;
            this.isTake = false;
            this.serivces = pampam;
        }
        public void addService(serivce ADD)
        {
            serivces.Add(ADD);
        }
        public void toTakePrice()
        {
            price = 0;
            foreach (serivce Serv in serivces)

            {
                price += Serv.getPrice();
            }
        }
        public void setDataOut()
        {
           dateOut= DateTime.Now.ToString(); 
        }
        public uint getPrice()
        {
            toTakePrice();
            return price;
        }
        public string getDateIn()
        {
            return dateIn;
        }
        public string getDateOut()
        {
            return dateOut;
        }
    }
}
