using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Services;

namespace StockPriceViewer
{
    class Program
    {
        static readonly BackgroundWorker WorkerThread = new BackgroundWorker();
        static void Main(string[] args)
        {
            //Request the user to enter the stock symbols
            Console.WriteLine("Please enter the Stock(s) symbols to view the prices");
            Console.WriteLine("Enter one or many stocks For example AAPL for one stock or APPL,GOOG separated by , for more than one stock");
            Console.WriteLine("Type Exit to Close");
            //read the stock symbols entered by the user
            string consoleInput = Console.ReadLine();

            //instantiate the background worker thread
            WorkerThread.WorkerSupportsCancellation = true;
            WorkerThread.RunWorkerCompleted += WorkerThread_RunWorkerCompleted;
            WorkerThread.DoWork += GetStockPriceAsync;

            //use a while loop to continue taking inputs from console and process the same,
            while (true)
            {
                //if user enters Exit, break and close the console
                if (consoleInput.ToLower() == "exit")
                {
                    WorkerThread.CancelAsync();
                    break;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(consoleInput))
                    {
                        Console.WriteLine("Please enter a symbol to proceed");
                    }
                    else
                    {
                        //read the url from the app.config
                        var urlToFetchData = ConfigurationManager.AppSettings["WebUrl"];

                        //read the fields to be fetched from the app.config
                        var fieldsToFetch = ConfigurationManager.AppSettings["FieldsToFetch"];

                        //Create the instance of the stock service
                        var stockService = new StockServiceFacade(consoleInput, urlToFetchData, fieldsToFetch);

                        DoWorkEventArgs doworkArgs = new DoWorkEventArgs(stockService);
                        WorkerThread.RunWorkerAsync(doworkArgs);
                    }
                    consoleInput = Console.ReadLine();
                }
            }
        }


        private static void GetStockPriceAsync(object sender, DoWorkEventArgs e)
        {
            var stockService = ((DoWorkEventArgs)(e.Argument)).Argument as StockServiceFacade;

            //Get the url from the config service
            if (stockService != null)
            {
                e.Result = stockService.GetStockPrices();
            }
        }

        static void WorkerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //Get the results from the background worker completed args
                var stocks = ((List<StockPrice>)e.Result);

                if (stocks != null && stocks.Any())
                {
                    foreach (var stock in stocks)
                    {
                        //write the data to the console
                        Console.WriteLine("Name:{0} Symbol{1}  Bid:{2} Ask:{3}", stock.Name, stock.Symbol, stock.Bid,
                            stock.Ask);
                    }
                }
                else
                {
                    Console.WriteLine("No Results returned");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException);
            }

        }
    }
}
