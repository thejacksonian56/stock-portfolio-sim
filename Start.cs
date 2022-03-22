using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Net.Http;
using Newtonsoft.Json;

namespace StockProjectTest
{
    class Program
    {
        public static Portfolio open = new Portfolio();
        public static string apiKey;
        public static StockAvailable temp = new StockAvailable();
        static void Main(string[] args) //Program start
        {
            //Console.WriteLine("Enter API Key from FinnHub.io: ");
            apiKey = "c8npoj2ad3iep4jec1lg"; //Temporary for testing, final product will ask user for own key
            Start();
            
            
        } 
        public static void Start() //The opening menu for the program once the API key is inserted
        {
            Console.Clear();
            Console.WriteLine("Welcome to Stock Simulator!");
            Console.WriteLine();
            Console.WriteLine("1. Open New Portfolio");
            Console.WriteLine("2. View Existing Portfolio");
            Console.WriteLine("3. Quit");
            Console.WriteLine();
            int selection = 0;
            try
            {
                selection = Convert.ToInt32(Console.ReadLine());    //Asks user for input
            }
            catch (Exception)
            {
                Console.WriteLine("Sorry, that's not a valid selection. Press enter to try again (start)");
                Console.ReadLine();
                Start();
            }
            switch (selection)  //Brings user to their deaired menu, or gives them a goodbye message if they wish to quit. Will loop back to method call if invalid selection is made
            {
                case 1:
                    newPortfolio();
                    break;
                case 2:
                    loadPortfolio();
                    break;
                case 3:
                    Console.WriteLine("Good Bye!");
                    Console.ReadLine();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Sorry, that's not a valid selection. Press enter to try again (default)");
                    Console.ReadLine();
                    Start();
                    break;
            }

        } 
        public static void newPortfolio() //Method that makes a new Portfolio Class and saves it
        {
            Console.Clear();
            string name = ""; //Starting string, later will be transfered to new class Portfolio
            double balance = 0.00; //Starting double, later will be transfered to new class Portfolio 
            bool validName=false; //Becomes true if making a name didn't cause an error
            bool validBalance=false; //Becomes true if making a starting balance didn't cause an error
            do //Loops here if either name or balance reached an error. Probably Redundant but haven't tested
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Portfolio Builder!");
                Console.WriteLine();
                do //Loops here if name had an error
                {
                    Console.WriteLine("Please type in a name for your Portfolio");
                    try
                    {
                        name = Console.ReadLine();
                        validName = true;

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Sorry, that's not a valid name");
                        Console.ReadLine();
                        Console.Clear();
                    } 
                } while (validName == false);
                do
                {
                    Console.WriteLine("Please type in a starting balance");
                    try
                    {
                        balance = Convert.ToDouble(Console.ReadLine());
                        validBalance = true;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Sorry, that's not a valid balance");
                        Console.ReadLine();
                        Console.Clear();
                    }
                } while (validBalance == false);
            } while (validName == false && validBalance == false); //Checks both name and balance for errors. Probably redundent but haven't tested yet
            open = new Portfolio(name, balance); //Creates new Portfolio Class. See Portfolio.cs
            open.Save(); //Saves new Portfolio class to a .xml document located in /MyDocuments/(name).xml -- See Portfolio.cs for more information
            Console.WriteLine("Congradulations! Your Portfolio has been created and saved!");
            Console.WriteLine("Press any key to continue to the dashboard");
            Console.ReadLine();
            portfolioOverview();
        }
        public static void loadPortfolio() //Method to load 
        {
            Console.Clear();
            Console.WriteLine("Please enter your portfolio name: ");
            try
            {
                string response = Console.ReadLine();
                XmlSerializer reader = new XmlSerializer(typeof(Portfolio));
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//StockSimulator" + response + ".xml"; //Sets path to default save location with filename given by user response
                System.IO.StreamReader file = new System.IO.StreamReader(path); //Reads save file
                open = (Portfolio)reader.Deserialize(file); //Loads the save file information into the portfolio 
                file.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Sorry, a save file with that name couldn't be found. Press enter to return to the menu.");
                Console.ReadLine();
                Start();
            }
            Console.WriteLine("File Loaded Successfully");
            Console.WriteLine("Press any key to return to the dashboard");
            Console.ReadLine();
            portfolioOverview();

        }
        public static void portfolioOverview() //Updates stock/portfolio information and displays it
        {
            updateStocks(); //See further down the file for functionality
            Console.Clear();
            Console.WriteLine("Name: " + open.Name);
            Console.WriteLine("Balance Available: " + Math.Round(open.Balance, 2));
            Console.WriteLine("Total Amount Invested: " + Math.Round(open.TotalInvested, 2));
            Console.WriteLine("Portfolio Worth: " + Math.Round(open.PortValue, 2));
            Console.Write("Portfolio Gain: ");
            if(open.PortGain > 0) //< -----Checks to see if growth is positive or negative
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(Math.Round(open.PortGain, 2));
                Console.ResetColor();
                Console.WriteLine();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(Math.Round(open.PortGain, 2));
                Console.ResetColor();
                Console.WriteLine();
            }
            Console.WriteLine("Stocks Owned: ");
            Console.WriteLine(" ");
            foreach (Stock x in open.stocksOwned)
            {
                Console.Write("Stock {0}, Shares: {1}, Value: {2}, Amount Invested: {3}, Gain: ",x.symbol, x.sharesOwned, Math.Round(x.value, 2), Math.Round(x.valueAtPurchase, 2));
                if(x.gain > 0) //< -----Checks to see if growth is positive or negative
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(Math.Round(x.gain, 2));
                    Console.ResetColor();
                    Console.WriteLine();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(Math.Round(x.gain, 2));
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }
            open.Save();
            Console.WriteLine(" ");
            Console.WriteLine("Select an option: 1. Buy Stock  2. Sell Stock  3. Add to Balance  4. Refresh 5. Return to Start");
            int response = 0;
            try
            {
                response = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Not a valid response! Press enter to refresh");
                Console.ReadLine();
            }
            switch (response){
                case 1:
                    search();
                    break;
                case 2:
                    sellStock();
                    break;
                case 3:
                    addBalance();
                    break;
                case 4:
                    portfolioOverview();
                    break;
                case 5:
                    Start();
                    break;
            }
            portfolioOverview();


        }
        public static void searchStock() //Allows the user to search for stock by it's symbol, and buy shares from that stock
        {
            StockGrabber finder = new StockGrabber(); //See StockGrabber.cs for functionality
            Console.WriteLine("Enter the Symbol of the Stock That You Would Like to Check");
            Console.WriteLine();
            string response = Console.ReadLine();
            finder.getInfo(response); //See StockGrabber.cs for functionality
            System.Threading.Thread.Sleep(1500); //Waits for 1.5 seconds, allows for above task to finish
            if (finder.stockFound == true) //Checks to see if the method returned in no stock information being pulled
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("What would you like to do next?");
                Console.WriteLine("1. Buy Shares");
                Console.WriteLine("2. Search Another Stock");
                Console.WriteLine("3. Go back");
            }
            else
            {
                Console.WriteLine("Sorry, no data for that symbol entry could be found. Press enter to return to the dashboard");
                Console.ReadLine();
                portfolioOverview();
            }
            int responseTwo = Convert.ToInt32(Console.ReadLine());
            switch (responseTwo)
            {
                case 1:
                    Console.WriteLine("How much money do you want to invest in this stock?");
                    float amount = float.Parse(Console.ReadLine());
                    float shares = amount / temp.c; //Finds the amount of shares user can buy with the amount they want to invest
                    Console.WriteLine("You are about to buy {0} shares of {1} worth ${2}, Procede? Y or N", shares, temp.symbol, amount);
                    Console.WriteLine();
                    char responseThree = Convert.ToChar(Console.ReadLine());
                    switch (responseThree)
                    {
                        case 'Y':
                            if(amount <= open.Balance) //Checks to see if user has enough money to complete transaction
                            {
                                open.Balance = open.Balance - amount; //Removes money
                                Stock test = new Stock(temp.symbol, amount, shares); //Adds new stock to list of stocks owned
                                test.sharePriceAtPurchase = temp.c;
                                open.stocksOwned.Add(test);                                
                                open.Save(); //Saves portfolio information
                                Console.WriteLine("Purchase successful! Returning to the dashboard.");
                                Console.ReadLine();
                                portfolioOverview();
                            }
                            else
                            {
                                Console.WriteLine("Sorry, you don't have enough money in your portfolio balance to buy this amount");
                                Console.WriteLine("Consider adding more to your balance, selling stock, or buying less amount");
                                Console.ReadLine();
                                portfolioOverview();
                            }
                            break;
                        case 'N':
                            Console.WriteLine("Canceling... Press enter to return to the dashboard");
                            Console.ReadLine();
                            portfolioOverview();
                            break;
                        default:
                            Console.WriteLine("Not a valid selection, press enter to return to dashboard");
                            Console.ReadLine();
                            portfolioOverview();
                            break;
                    }
                    
                    break; 
                case 2:
                    Console.Clear();
                    search();
                    break;
                case 3:
                    portfolioOverview();
                    break;
                default:
                    Console.WriteLine("Sorry, Not a valid Response!!");
                    Console.ReadLine();
                    searchStock();
                    break;

            }


        }
        public static void updateStocks() //Obtains updated portfolio information for stocks owned and updates them if changes are made, as well as clean up duplicates
        {
            StockGrabber finder = new StockGrabber(); //See StockGrabber.cs for functionality
            List<Stock> temp = open.stocksOwned; //Creates duplication of the portfolio's list of stocks owned
            Console.Clear();
            Console.WriteLine("Please wait... Updating stock information");

           
            foreach(Stock x in open.stocksOwned)
            {
                foreach(Stock y in temp) //THe following loop compares stocks from the portfolio and the duplicate list created above, and merges the value of shares if they are different and listed under the same symbol
                {
                    if(x.symbol == y.symbol & x.sharesOwned != y.sharesOwned)
                    {
                        x.sharesOwned = x.sharesOwned + y.sharesOwned;
                        x.sharePriceAtPurchase = (x.sharePriceAtPurchase + y.sharePriceAtPurchase) / 2;
                        y.sharePriceAtPurchase = x.sharePriceAtPurchase;
                        y.sharesOwned = x.sharesOwned;
                    }
                }
                if(x.sharesOwned == 0)  //This loop removes stocks if they have no shares owned
                {
                    open.stocksOwned.Remove(x);
                }
            }
            open.stocksOwned = temp.GroupBy(x => x.symbol).Select(x => x.First()).ToList(); //Removes duplicate entries of stock under the same symbol now that the correct shares owned is reflected acordingly
            finder.refreshInfo(open.stocksOwned); //See StockGrabber.cs for functionality
            System.Threading.Thread.Sleep(2000); //Waits 2 secons for the above method to complete
            open.PortValue = 0.00; //Sets the portfolio value to 0 to prevent the new value stacking ontop of the old one
            open.PortGain = 0.00;
            open.TotalInvested = 0.00;
            foreach(Stock x in open.stocksOwned)
            {
                open.PortValue = open.PortValue + x.value; //Adds up all the values of stocks owned for a total portfolio value
                x.gain = (x.sharePrice - x.sharePriceAtPurchase) * x.sharesOwned;
                open.PortGain = open.PortGain + x.gain;
                x.valueAtPurchase = x.sharesOwned * x.sharePriceAtPurchase;
                open.TotalInvested = open.TotalInvested + x.valueAtPurchase;
            }

        }
        public static void addBalance() //Method to add balance to the portfolio
        {
            Console.Clear();
            Console.WriteLine("How much would you like to invest?");
            Console.WriteLine(" ");
            double response = 0.0;
            try
            {
                response = Convert.ToDouble(Console.ReadLine()); //Asks user how much they want to invest
            }
            catch (Exception)
            {
                Console.WriteLine("Sorry, that's not a valid response. Press enter to return to the dashboard");
                Console.ReadLine();
                portfolioOverview();
            }
            open.Balance = open.Balance + response; //Adds new money to the balance amount
            Console.WriteLine("Successfully added {0} to your balance, your total balance is now {1}.", response, open.Balance);
            Console.WriteLine("Press enter to go back to the dashboard");
            open.Save(); //See portfilio.cs for functionality
            Console.ReadLine();
            portfolioOverview();


        }
        public static void sellStock()
        {
            Console.WriteLine("Enter the symbol of the stock you want to sell");
            string response = Console.ReadLine();
            foreach(Stock x in open.stocksOwned)
            {
                if (x.symbol == response)
                {
                    Console.WriteLine("How much would you like to sell? ($)");
                    float sellPrice = float.Parse(Console.ReadLine());
                    if (x.value >= sellPrice)
                    {
                        float sellRatio = sellPrice / x.value;
                        float sellShares = x.sharesOwned * sellRatio;
                        float remainingShares = x.sharesOwned - sellShares;
                        Console.WriteLine("You are about to sell {0}$ worth of {1} stock. You will have {2} shares left after this transaction. Continue? Y or N", sellPrice, x.symbol, remainingShares);
                        char resp = Convert.ToChar(Console.ReadLine());
                        switch (resp)
                        {
                            case 'Y':
                                Console.Clear();
                                open.Balance = open.Balance + sellPrice;
                                x.sharesOwned = remainingShares;
                                Console.WriteLine("Transaction complete! Press enter to return to dashboard");
                                Console.ReadLine();
                                portfolioOverview();
                                break;
                            case 'N':
                                Console.Clear();
                                Console.WriteLine("Transaction canceled: Press enter to return to dashboard");
                                Console.ReadLine();
                                portfolioOverview();
                                break;
                            default:
                                Console.Clear();
                                Console.WriteLine("Sorry, that's not a valid response. Press enter to return to dashboard");
                                Console.ReadLine();
                                portfolioOverview();
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Sorry, you don't have that much to sell... Press enter to continue");
                        Console.ReadLine();
                        portfolioOverview();
                    }

                }
            }
        } //Method to allow the user to sell stock for it's worth and add money back to balance
        public static void search()
        {
            Console.Clear();
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1. Search for symbol  2. Type in symbol manually  3. Go Back");
            int response = Convert.ToInt32(Console.ReadLine());
            switch (response){
                case 1:
                    Console.Clear();
                    StockGrabber grabber = new StockGrabber();
                    Console.WriteLine("Search: ");
                    string responseTwo = Console.ReadLine();
                    grabber.searchSyb(responseTwo);
                    System.Threading.Thread.Sleep(3000);
                    Console.WriteLine();
                    searchStock();
                    break;
                case 2:
                    Console.Clear();
                    searchStock();
                    break;
                case 3:
                    portfolioOverview();
                    break;
                default:
                    Console.WriteLine("Sorry, that's not a valid option! Press enter to try again");
                    Console.ReadLine();
                    search();
                    break;

            }

        }  //Method to search for stocks and stock symbols, currently the api returns weird results

    }
}
