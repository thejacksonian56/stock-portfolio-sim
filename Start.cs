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
        static void Main(string[] args)
        {
            //Console.WriteLine("Enter API Key from FinnHub.io: ");
            apiKey = "c8npoj2ad3iep4jec1lg"; //Temporary for testing, final product will ask user for own key
            Start();
            
            
        }
        public static void Start()
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
                Console.WriteLine("Sorry, that's not a valid selection. Press enter to try again");
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
                    Console.WriteLine("Good bye!");
                    break;
                default:
                    Console.WriteLine("Sorry, that's not a valid selection. Press enter to try again");
                    Console.ReadLine();
                    Start();
                    break;
            }

        }
        static void newPortfolio() //Method that makes a new Portfolio Class and saves it
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
            Menu();
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
            Menu();

        }
        public static void Menu() //Dashboard, the main hub for the program after a portfolio has been loaded
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Dashboard!");
            Console.WriteLine("1. Find Stocks");
            Console.WriteLine("2. Add Money to Balance");
            Console.WriteLine("3. View Portfolio");
            Console.WriteLine("4. Save Portfolio");
            Console.WriteLine("5. Return to Start");
            try
            {
                int response = Convert.ToInt32(Console.ReadLine());
                switch (response)
                {
                    case 1:
                        searchStock(); //See further down the file for functionality
                        break;
                    case 2:
                        addBalance(); //See further down the file for functionality
                        break;

                    case 3:
                        portfolioOverview(); //See further down the file for functionality
                        break;
                    case 4:
                        open.Save(); //See Portfolio.cs for functionality
                        Console.WriteLine("Your file has been saved! Press any key to continue.");
                        Console.ReadLine();
                        Menu();
                        break;
                    case 5:
                        Start();
                        break;
                    default:
                        Console.WriteLine("Sorry, that's not a valid selection! Press any key to continue");
                        Console.ReadLine();
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Sorry, that's not a vald selection!! Press any key to continue");
                Console.ReadLine();
                Menu();
            }
        }
        public static void portfolioOverview() //Updates stock/portfolio information and displays it
        {
            updateStocks(); //See further down the file for functionality
            Console.Clear();
            Console.WriteLine("Name: " + open.Name);
            Console.WriteLine("Balance Available: " + open.Balance);
            Console.WriteLine("Total Amount Invested: " + open.TotalInvested);
            Console.WriteLine("Portfolio Worth: " + open.PortValue);
            Console.WriteLine("Portfolio Gain: " + open.PortGain);
            Console.WriteLine("Stocks Owned: ");
            Console.WriteLine(" ");
            foreach (Stock x in open.stocksOwned)
            {
                Console.WriteLine("Stock {0}, Shares: {1}, Value: {2}",x.symbol, x.sharesOwned, x.value);
            }
            Console.WriteLine(" ");
            Console.WriteLine("Press any key to return to the dashboard");
            open.Save();
            Console.ReadLine();
            Menu();


        }
        public static void searchStock() //Allows the user to search for stock by it's symbol, and buy shares from that stock
        {
            Console.Clear();
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
                Menu();
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
                                open.stocksOwned.Add(test);                                 //
                                open.TotalInvested = open.TotalInvested + amount; //Updates total amount invested
                                open.Save(); //Saves portfolio information
                                Console.WriteLine("Purchase successful! Returning to the dashboard.");
                                Console.ReadLine();
                                Menu();
                            }
                            else
                            {
                                Console.WriteLine("Sorry, you don't have enough money in your portfolio balance to buy this amount");
                                Console.WriteLine("Consider adding more to your balance, selling stock, or buying less amount");
                                Console.ReadLine();
                                Menu();
                            }
                            break;
                        case 'N':
                            Console.WriteLine("Canceling... Press enter to return to the dashboard");
                            Console.ReadLine();
                            Menu();
                            break;
                        default:
                            Console.WriteLine("Not a valid selection, press enter to return to dashboard");
                            Console.ReadLine();
                            Menu();
                            break;
                    }
                    
                    break; 
                case 2:
                    searchStock();
                    break;
                case 3:
                    Menu();
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

            //THe following loop compares stocks from the portfolio and the duplicate list created above, and merges the value of shares if they are different and listed under the same symbol
            foreach(Stock x in open.stocksOwned)
            {
                foreach(Stock y in temp)
                {
                    if(x.symbol == y.symbol & x.sharesOwned != y.sharesOwned)
                    {
                        x.sharesOwned = x.sharesOwned + y.sharesOwned;
                        y.sharesOwned = x.sharesOwned;
                    }
                }
            }
            open.stocksOwned = temp.GroupBy(x => x.symbol).Select(x => x.First()).ToList(); //Removes duplicate entries of stock under the same symbol now that the correct shares owned is reflected acordingly
            finder.refreshInfo(open.stocksOwned); //See StockGrabber.cs for functionality
            System.Threading.Thread.Sleep(5000); //Waits 5 secons for the above method to complete
            open.PortValue = 0.00; //Sets the portfolio value to 0 to prevent the new value stacking ontop of the old one
            foreach(Stock x in open.stocksOwned)
            {
                open.PortValue = open.PortValue + x.value; //Adds up all the values of stocks owned for a total portfolio value
            }
            open.PortGain = open.PortValue - open.TotalInvested; //Shows the difference between portfolio balance and amount invested

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
                Menu();
            }
            open.Balance = open.Balance + response; //Adds new money to the balance amount
            Console.WriteLine("Successfully added {0} to your balance, your total balance is now {1}.", response, open.Balance);
            Console.WriteLine("Press enter to go back to the dashboard");
            open.Save(); //See portfilio.cs for functionality
            Console.ReadLine();
            Menu();


        }

    }
}
