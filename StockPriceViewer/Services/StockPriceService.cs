using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public class StockPriceService : IStockService
    {
        /// <summary>
        /// Get the Stock Prices from the Yahoo finance
        /// </summary>
        /// <param name="url"></param>
        /// <returns> CSV formatted string data of the stocks</returns>
        public string FetchStockPrices(string url)
        {
            string csvData = null;
            try
            {
                using (WebClient web = new WebClient())
                {
                    csvData = web.DownloadString(url);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return csvData;
        }

        /// <summary>
        /// Parse the CSV data received from the web and create the stock price collection
        /// </summary>
        /// <param name="csvData"> The CSV data format of the Stocks</param>
        /// <param name="fieldFormat"> The format of the field sent in the web request</param>
        /// <returns>List of StockPrices</returns>
        public List<StockPrice> ParseStockPrices(string csvData, string fieldFormat)
        {
            //Creates the stock price collection
            List<StockPrice> sotckPricesrices = new List<StockPrice>();

            if (!string.IsNullOrWhiteSpace(csvData) && !string.IsNullOrWhiteSpace(fieldFormat)
                && fieldFormat.IndexOfAny(new char[] { 's', 'n', 'b', 'a' }) >= 0)
            {
                //split the CSV data for each line as each line represents a stock
                string[] stockRows = csvData.Replace("\r", "").Split('\n');

                if (stockRows.Length > 0)
                {
                    foreach (string sotckRow in stockRows)
                    {
                        //Check if the stock is available
                        if (String.IsNullOrEmpty(sotckRow)) continue;

                        //Each stock row from CSV data is coma seperated and needs to be split
                        string[] stockColumns = sotckRow.Split(',');

                        //Create the Stockprice object
                        StockPrice stockPrice = new StockPrice();
                        //Map each column index to the index of the fieldFormat
                        stockPrice.Symbol = stockColumns[fieldFormat.IndexOf('s')];
                        stockPrice.Name = stockColumns[fieldFormat.IndexOf('n')];
                        stockPrice.Bid = stockColumns[fieldFormat.IndexOf('b')] == "N/A"
                            ? 0
                            : Convert.ToDecimal(stockColumns[fieldFormat.IndexOf('b')]);
                        stockPrice.Ask = stockColumns[fieldFormat.IndexOf('a')] == "N/A"
                            ? 0
                            : Convert.ToDecimal(stockColumns[fieldFormat.IndexOf('a')]);

                        sotckPricesrices.Add(stockPrice);
                    }
                }
            }

            return sotckPricesrices;
        }

    }
}
