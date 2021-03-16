using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prachecnaya
{
    class info
    {
        public uint countOfMachine ;
        private uint countOfOrders ;
        public List<order> orders = new List<order>();
        public List<serivce> serivces=new List<serivce>();


        public info(uint machine) {
            countOfMachine = machine;
            countOfOrders = 0;
            serivces.Add(new serivce(1, "Стрика до 6 кг", 300));
            serivces.Add(new serivce(2, "Стрика до 10 кг", 450));
            serivces.Add(new serivce(3, "Стрика до 15 кг", 550));
            serivces.Add(new serivce(4, "Обслуживание верхней одежды", 200));
            serivces.Add(new serivce(5, "Чистка обуви", 250));
            serivces.Add(new serivce(6, "Чистка верхней одежды", 300));
        }
        public void addOrder(uint id, List<serivce> serivces)
        {
            orders.Add(new order(id,serivces));

        }
        public void plusOrder()
        {
            countOfOrders++;
        }
        public uint getCountOfOrders()
        {
            return countOfOrders;
        }
        
                
    }
}
