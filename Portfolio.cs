using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StockProjectTest
{

    [Serializable]  //Attribute to allow Serializing data
    public class Portfolio
    {


        private string _name;
        private double _balance;
        private double _portValue;
        private double _portGain;
        private double _totalInvested;
        public List<Stock> stocksOwned = new List<Stock>(); //List of stocks that the user owns

        public string Name //An identifier for a unique portfolio
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }
        
        public double Balance //Amount the user has available to invest
        {
            get { return _balance; }
            set
            {
                _balance = value;
            }
        }

        public double PortValue //The current value of the users portfolio 
        {
            get { return _portValue; }
            set
            {
                _portValue = value;
            }
        }

        public double PortGain //The net gain/loss of the users portfolio
        {
            get { return _portGain; }
            set
            {
                _portGain = value;
            }
        }

        public double TotalInvested { get => _totalInvested; set => _totalInvested = value; } //The total amount of money the user has invested in stock

        public string apiKey; //Finnhub API key for calling stock information. 

        public Portfolio(string name, double balance, string apiKey_)
        {
            Name = name;
            Balance = balance;
            PortValue = 0.00;
            PortGain = 0.00;
            TotalInvested = 0.00;
            apiKey = apiKey_;
        }
        public Portfolio()
        {

        }
        public void Save()  //Method for saving class data to a .xml file. Currently can only be saved to %appdata%/Roaming/
        {
            XmlSerializer writer = new XmlSerializer(typeof(Portfolio)); //Creates new XmlSerializer class from System.Xml.Serialization;
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//StockSimulator" + this.Name + ".xml";
            System.IO.FileStream file = System.IO.File.Create(path);
            writer.Serialize(file, this);
            file.Close();

        }

    }
}
