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
        static void Main(string[] args)
        {
            //Console.WriteLine("Enter API Key from FinnHub.io: ");
            apiKey = "c8npoj2ad3iep4jec1lg"; //Temporary for testing, final product will ask user for own key
            Start();
            
            
        }
        public static void Start()
        {
            bool done = false; //Becomes true once a successful choice was made
            do //Loops here if choice was not successful
            {
                Console.Clear();
                Console.WriteLine("Welcome to Stock Simulator!");
                Console.WriteLine();
                Console.WriteLine("1. Open New Portfolio");
                Console.WriteLine("2. View Existing Portfolio");
                Console.WriteLine("3. Quit");
                Console.WriteLine();
                try //Prevents a choice outside of selections
                {
                    int slection = Convert.ToInt32(Console.ReadLine());
                    if (slection == 1)
                    {
                        done = true;
                        Console.Clear();
                        newPortfolio(); //see further down in current file
                    }
                    else if (slection == 2)
                    {
                        done = true;
                        Console.Clear();
                        loadPortfolio(); //See further down in current file
                    }
                    else if (slection == 3)
                    {
                        done = true;
                        Console.Clear();
                        Console.WriteLine("Goodbye!");
                        Console.ReadLine();
                    }
                    else //Occurs if response is an int bty
                    {
                        Console.WriteLine("Sorry, that's not a valid selection!");
                        Console.ReadLine();

                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Sorrry, that's not a valid selection!");
                    Console.ReadLine();
                }
            } while (done == false); //Checks to see if a valid choice was made or not
        }
        static void newPortfolio() //Method that makes a new Portfolio Class and saves it
        {
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
            Portfolio open = new Portfolio(name, balance); //Creates new Portfolio Class. See Portfolio.cs
            open.Save(); //Saves new Portfolio class to a .xml document located in /MyDocuments/(name).xml -- See Portfolio.cs for more information
            Console.WriteLine("Congradulations! Your Portfolio has been created and saved!");
            Console.WriteLine("Press any key to continue to the dashboard");
            Console.ReadLine();
            Menu();
        }
        public static void loadPortfolio() //Method to load 
        {
            Console.WriteLine("Please enter your portfolio name: ");
            string response = Console.ReadLine();
            XmlSerializer reader = new XmlSerializer(typeof(Portfolio));
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//StockSimulator" + response + ".xml"; //Sets path to default save location with filename given by user response
            System.IO.StreamReader file = new System.IO.StreamReader(path); //Reads save file
            open = (Portfolio)reader.Deserialize(file); //Loads the save file information into the portfolio 
            file.Close();
            Console.WriteLine("File Loaded Successfully");
            Console.WriteLine("Press any key to return to the dashboard");
            Console.ReadLine();
            Menu();

        }
        public static void Menu()
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
                        searchStock();
                        break;
                    case 2:
                        addBalance();
                        break;

                    case 3:
                        portfolioOverview();
                        break;
                    case 4:
                        open.Save();
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
        public static void portfolioOverview()
        {
            updateStocks();
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
        public static void searchStock()
        {
            Console.Clear();
            StockGrabber finder = new StockGrabber();
            Console.WriteLine("Enter the Symbol of the Stock That You Would Like to Check");
            Console.WriteLine();
            string response = Console.ReadLine();
            finder.getInfo(response);
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("1. Buy Shares");
            Console.WriteLine("2. Search Another Stock");
            Console.WriteLine("3. Go back");
            int responseTwo = Convert.ToInt32(Console.ReadLine());
            switch (responseTwo)
            {
                case 1:
                    Console.WriteLine("How much money do you want to invest in this stock?");
                    float amount = float.Parse(Console.ReadLine());
                    float shares = amount / finder.temp.c;
                    Console.WriteLine("You are about to buy {0} shares of {1} worth ${2}, Procede? Y or N", shares, finder.temp.symbol, amount);
                    Console.WriteLine();
                    char responseThree = Convert.ToChar(Console.ReadLine());
                    switch (responseThree)
                    {
                        case 'Y':
                            if(amount <= open.Balance)
                            {
                                open.Balance = open.Balance - amount;
                                Stock test = new Stock(finder.temp.symbol, amount, shares);
                                open.stocksOwned.Add(test);
                                open.TotalInvested = open.TotalInvested + amount;
                                open.Save();
                                Console.WriteLine("Purchase successful! Returning to the dashboard.");
                                Console.ReadLine();
                                Menu();
                            }
                            else
                            {
                                Console.WriteLine("Sorry, you don't have enough money in your portfolio balance to buy this amount");
                                Console.WriteLine("Consider adding more to your balance, selling stock, or buying less amount");
                                Console.ReadLine();
                                searchStock();
                            }
                            break;
                        case 'N':
                            Console.WriteLine("Canceling... Press any key to return to the dashboard");
                            Console.ReadLine();
                            Menu();
                            break;
                        default:
                            Console.WriteLine("Not a valid selection, returning to stock search");
                            Console.ReadLine();
                            searchStock();
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
        public static void updateStocks()
        {
            StockGrabber finder = new StockGrabber();
            Console.Clear();
            Console.WriteLine("Please wait... Updating stock information");
            finder.refreshInfo(open.stocksOwned);
            System.Threading.Thread.Sleep(5000);
            open.PortValue = 0.00;
            foreach(Stock x in open.stocksOwned)
            {
                open.PortValue = open.PortValue + x.value;
            }
            open.PortGain = open.PortValue - open.TotalInvested;

        }
        public static void addBalance()
        {
            Console.Clear();
            Console.WriteLine("How much would you like to invest?");
            Console.WriteLine(" ");
            double response = 0.0;
            try
            {
                response = Convert.ToDouble(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Sorry, that's not a valid response. Press enter to try again");
                Console.ReadLine();
                addBalance();
            }
            open.Balance = open.Balance + response;
            Console.WriteLine("Successfully added {0} to your balance, your total balance is now {1}.", response, open.Balance);
            Console.WriteLine("Press enter to go back to the dashboard");
            open.Save();
            Console.ReadLine();
            Menu();


        }

    }
}
