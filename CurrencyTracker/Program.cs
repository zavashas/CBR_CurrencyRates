using CurrencyTracker;
using CurrencyTracker.Models;
using System;
using System.Threading.Tasks;
using CurrencyTracker.Services;

namespace CurrencyTracker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var dbContext = new CurrencyDbContext(); 
            var getCurrency = new GetCurrency();

            var currencyRateLoader = new CurrencyRateLoader(dbContext, getCurrency);

            try
            {
                Console.WriteLine("Загрузка курсов валют за последний месяц...");
                await currencyRateLoader.LoadLastMonthRatesAsync();
                Console.WriteLine("Загрузка завершена");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
