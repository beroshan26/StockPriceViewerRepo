using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class StockPrice
    {
        //Symbol of the Stock
        public string Symbol { get; set; }

        //Name of the Stock
        public string Name { get; set; }
        
        //Bid price of the Stock
        public decimal Bid { get; set; }
        
        //Ask price for the Stock
        public decimal Ask { get; set; }
        
    }
}
