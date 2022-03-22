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

        public bool stockFound; //Becomes false if no information was able to be pulled
        public StockAvailable temp; //Temporary instance of StockAvailable for the use of methods below. See Stock.cs for attributes


        public async Task getInfo(string symb) //Task for pulling updated information on a specific stock through a stock symbol and the FinnHub API
        {
            HttpClient reader = new HttpClient(); 
            string _response = await reader.GetStringAsync("https://finnhub.io/api/v1/quote?symbol=" + symb + "&token=" + Program.apiKey); //Grabs data from the API using the symbol given at method called and key given at program startup
            StockAvailable temp = JsonConvert.DeserializeObject<StockAvailable>(_response); //Deserializes the data from the API and inserts it in the temp instance of StockAvailable. See Stock.cs for attributes
            if (temp != null)
            {
                stockFound = true;
                temp.symbol = symb; //Sets the symbol of temp to the symb string set at the calling of the method
                Console.WriteLine("Current Price: " + Math.Round(temp.c, 2));          ////
                Console.WriteLine("Change Today: " + Math.Round(temp.d, 2));             //
                Console.Write("Percent Change: ");                                       //
                if(temp.dp > 0)                                                          //<-----Checks to see if growth is positive or negative
                {                                                                        //
                    Console.ForegroundColor = ConsoleColor.Green;                        //
                    Console.Write(temp.dp + "%");                                        //
                    Console.ResetColor();                                                //
                    Console.WriteLine();                                                 //
                }                                                                        //  
                else                                                                     //-Prints out all the data pulled from the temp instance of StockAvailable.
                {                                                                        //
                    Console.ForegroundColor = ConsoleColor.Red;                          //
                    Console.Write(temp.dp + "%");                                        //
                    Console.ResetColor();                                                //  
                    Console.WriteLine();                                                 //
                }                                                                        //
                                                                                         //
                Console.WriteLine("High Price Today: " + Math.Round(temp.h, 2));         //
                Console.WriteLine("Low Price Today: " + Math.Round(temp.l, 2));          //
                Console.WriteLine("Open Price Today: " + Math.Round(temp.o, 2));         //
                Console.WriteLine("Previous Close Price: " + Math.Round(temp.pc));     //// 
                Program.temp = temp;
            }
            else
            {
                stockFound = false;
            }
        }
        public async Task refreshInfo(List<Stock> stocks) //Task for pulling information on all the stocks in a list of stocks and updating each stock's value of shares
        {
            HttpClient reader = new HttpClient();
            foreach(Stock x in stocks) //Loop to go through each stock in the given list
            {
                string response = await reader.GetStringAsync("https://finnhub.io/api/v1/quote?symbol=" + x.symbol + "&token=" + Program.apiKey); //Grabs data from the API using the symbol of the stock the loop is on and key given at program startup
                temp = JsonConvert.DeserializeObject<StockAvailable>(response); //Deserializes the data from the API and inserts it in the temp instance of StockAvailable. See Stock.cs for attributes
                x.sharePrice = temp.c;                  //Sets the current stock price per share
                x.value = x.sharePrice * x.sharesOwned; //Modifies the value of shares to reflect its updated value
            }
            

        }
        public async Task searchSyb(string query)  //Task for pulling information on a stock query and listing the results
        {
            int x = 0;
            HttpClient reader = new HttpClient();
            string response = await reader.GetStringAsync("https://finnhub.io/api/v1/search?q=" + query + "&token=" + Program.apiKey);
            StockSymbolQueryResults info = JsonConvert.DeserializeObject<StockSymbolQueryResults>(response);
            Console.WriteLine("Count: " + info.count);
            Console.WriteLine("Results: ");
            Console.WriteLine();
            while (x < Convert.ToInt32(info.count) & x <= 5) //Prints out the top 5 query results
            {
                Console.WriteLine(info.result[x].description);
                Console.WriteLine(info.result[x].displaySymbol);
                Console.WriteLine(info.result[x].type);
                x++;
                Console.WriteLine();
            }
        } 
        
        
        
    }

}
