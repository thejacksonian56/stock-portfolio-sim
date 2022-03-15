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

        public StockAvailable temp;
        public async Task getInfo(string symb)
        {
            HttpClient reader = new HttpClient();
            string _response = await reader.GetStringAsync("https://finnhub.io/api/v1/quote?symbol=" + symb + "&token=" + Program.apiKey);
            StockAvailable current = JsonConvert.DeserializeObject<StockAvailable>(_response);
            temp = current;
            temp.symbol = symb;
            Console.WriteLine("Current Price: " + temp.c);
            Console.WriteLine("Change Today: " + temp.d);
            Console.WriteLine("Percent Change: " + temp.dp + "%");
            Console.WriteLine("High Price Today: " + temp.h);
            Console.WriteLine("Low Price Today: " + temp.l);
            Console.WriteLine("Open Price Today: " + temp.o);
            Console.WriteLine("Previous Close Price: " + temp.pc);
        }
        
        
        
    }

}
