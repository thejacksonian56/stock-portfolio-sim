using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;


namespace StockProjectTest
{
    public class StockGrabber
    {

        public StockAvailable temp; //Temporary instance of StockAvailable for the use of methods below. See Stock.cs for attributes
        public async Task getInfo(string symb) //Task for pulling updated information on a specific stock through a stock symbol and the FinnHub API
        {
            HttpClient reader = new HttpClient(); 
            string _response = await reader.GetStringAsync("https://finnhub.io/api/v1/quote?symbol=" + symb + "&token=" + Program.apiKey); //Grabs data from the API using the symbol given at method called and key given at program startup
            StockAvailable temp = JsonConvert.DeserializeObject<StockAvailable>(_response); //Deserializes the data from the API and inserts it in the temp instance of StockAvailable. See Stock.cs for attributes
            temp.symbol = symb; //Sets the symbol of temp to the symb string set at the calling of the method
            Console.WriteLine("Current Price: " + temp.c);          //
            Console.WriteLine("Change Today: " + temp.d);           //
            Console.WriteLine("Percent Change: " + temp.dp + "%");  //
            Console.WriteLine("High Price Today: " + temp.h);       //Prints out all the data pulled from the temp instance of StockAvailable.
            Console.WriteLine("Low Price Today: " + temp.l);        //
            Console.WriteLine("Open Price Today: " + temp.o);       //
            Console.WriteLine("Previous Close Price: " + temp.pc);  //
        }
        public async Task refreshInfo(List<Stock> stocks) //Task for pulling information on all the stocks in a list of stocks and updating each stock's value of shares
        {
            HttpClient reader = new HttpClient();
            foreach(Stock x in stocks) //Loop to go through each stock in the given list
            {
                string response = await reader.GetStringAsync("https://finnhub.io/api/v1/quote?symbol=" + x.symbol + "&token=" + Program.apiKey); //Grabs data from the API using the symbol of the stock the loop is on and key given at program startup
                temp = JsonConvert.DeserializeObject<StockAvailable>(response); //Deserializes the data from the API and inserts it in the temp instance of StockAvailable. See Stock.cs for attributes
                x.value = temp.c * x.sharesOwned; //Modifies the value of shares to reflect its updated value
            }
            

        }
        
        
        
    }

}
