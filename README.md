# stock-portfolio-sim
A simulated stock portfolio manager made for an MSSA project

This program allows the user to create and manage local stock portfolios in a sandbox enviornment. In it's current state, it doesn't accuratly reflect the rules of the stock market, and shouldn't be used for anything other than personal use. This program was developed in C# and with the Finnhub.io stock API.

## Navigating Code
This project is made up of 4 main files: 
#### Start.cs
The main file containing most of the methods for the program as well as Main()
#### Portfolio.cs
Contains code for the Portfolio class, which stores information on the users overall portfolio
#### Stock.cs
Contains code for various stock classes used throughout the program to provide information on stocks available to buy and stocks owned
#### StockGrabber.cs
Contains code for the StockGrabber class, which contains tasks that use the Finnhub.io API to grab stock information from the web

## Running the program
Once the project has been downloaded, it can be executed by navigating to the /bin/Debug/net5.0/ and running StockProjectTest.exe
