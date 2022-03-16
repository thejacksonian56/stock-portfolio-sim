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
        public List<Stock> stocksOwned = new List<Stock>();

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }
        
        public double Balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
            }
        }

        public double PortValue
        {
            get { return _portValue; }
            set
            {
                _portValue = value;
            }
        }

        public double PortGain
        {
            get { return _portGain; }
            set
            {
                _portGain = value;
            }
        }

        public double TotalInvested { get => _totalInvested; set => _totalInvested = value; }

        public Portfolio(string name, double balance)
        {
            Name = name;
            Balance = balance;
            PortValue = 0.00;
            PortGain = 0.00;
            TotalInvested = 0.00;
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
