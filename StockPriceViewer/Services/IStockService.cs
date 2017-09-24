using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface IStockService
    {
        /// <summary>
        /// Get the Stock Prices from the web
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        string FetchStockPrices(string url);

        /// <summary>
        /// Parse the stock price string to a list of stock prices
        /// </summary>
        /// <param name="csvData"></param>
        /// <param name="fieldFormat"></param>
        /// <returns></returns>
        List<StockPrice> ParseStockPrices(string csvData, string fieldFormat);
    }
}
