using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace StockProjectTest
{
    public class StockAvailable
    {
        public string symbol;
        public float c = 0.0F; //Current Price
        public float d = 0.0F; //Change
        public float dp = 0.0F;//Percent Change
        public float h = 0.0F; //High Price of Day
        public float l = 0.0F; //Low Price of Day
        public float o = 0.0F; //Open Price of Day
        public float pc = 0.0F;//Previous Close Price
        string Response;

        public StockAvailable()
        {

        }
        
        

        
    }
    public class Stock
    {
        public string symbol;
        public float value;
        public float sharesOwned;
        public Stock(string syb, float val, float sha)
        {
            symbol = syb;
            value = val;
            sharesOwned = sha;
        }
        public Stock()
        {

        }
    }
}
