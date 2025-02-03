using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CurrencyTracker.Models;
using CurrencyTracker.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CurrencyTracker
{
    /// <summary>
    /// Запуск, инициализация и настройка
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            // Создание конфигурации из файла appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Настройка DI 
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)  
                .AddDbContext<CurrencyDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))) 
                .AddTransient<GetCurrency>()  
                .AddTransient<CurrencyRateLoader>()  
                .BuildServiceProvider();

            // Получение экземпляров зависимостей из DI контейнера
            var dbContext = serviceProvider.GetRequiredService<CurrencyDbContext>();
            var getCurrency = serviceProvider.GetRequiredService<GetCurrency>();
            var currencyRateLoader = serviceProvider.GetRequiredService<CurrencyRateLoader>();

            try
            {
                Console.WriteLine("Загрузка курсов валют за последний месяц...");
                await currencyRateLoader.LoadLastMonthRatesAsync();
                Console.WriteLine("Загрузка завершена");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.WriteLine($"Внутреннее исключение: {ex.InnerException?.Message}");
            }

        }
    }
}
