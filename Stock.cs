using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace StockProjectTest
{
    public class StockAvailable //A stock class that has all current information about the stock, usually pulled from the FinnHub api
    {
        public string symbol;
        public float c = 0.0F; //Current Price
        public float d = 0.0F; //Change
        public float dp = 0.0F;//Percent Change
        public float h = 0.0F; //High Price of Day
        public float l = 0.0F; //Low Price of Day
        public float o = 0.0F; //Open Price of Day
        public float pc = 0.0F;//Previous Close Price

        public StockAvailable()
        {

        }
        
        

        
    }
    public class Stock //Version of stock that only reflects the user's ownership of shares
    {
        public string symbol; //The stock symbol, acts as an identifier
        public float value; //Value of all the shares the user has in this stock
        public float sharesOwned; //Amount of shares the user has in this stock
        public float valueAtPurchase; //Value of the shares at purchase
        public Stock(string syb, float val, float sha)
        {
            symbol = syb;
            value = val;
            sharesOwned = sha;
            valueAtPurchase = val;
            
        }
        public Stock()
        {

        }
    }
}
