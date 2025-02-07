﻿using CurrencyTracker.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyTracker.Services
{
    /// <summary>
    /// Загрузка данных из цбр в бд
    /// </summary>
    public class CurrencyRateLoader
    {
        private readonly CurrencyDbContext _dbContext;
        private readonly GetCurrency _getCurrency;

        public CurrencyRateLoader(CurrencyDbContext context, GetCurrency getCurrency)
        {
            _dbContext = context;
            _getCurrency = getCurrency;
        }

        public async Task LoadLastMonthRatesAsync()
        {
            DateTime today = DateTime.Today;
            DateTime monthAgo = today.AddDays(-30);

            // Удаление устаревших курсов валют
            var outdatedRates = _dbContext.CurrencyRates.Where(r => r.DateCurrencyRate < monthAgo);
            _dbContext.CurrencyRates.RemoveRange(outdatedRates);
            await _dbContext.SaveChangesAsync();

            // Получаем все уникальные даты, для которых уже есть данные
            var existingDates = _dbContext.CurrencyRates
                .Where(r => r.DateCurrencyRate >= monthAgo && r.DateCurrencyRate <= today)
                .Select(r => r.DateCurrencyRate)
                .Distinct()
                .ToList();

            // Загрузка курсов валют за последние 30 дней, если данных еще нет
            for (int i = 0; i < 30; i++)
            {
                DateTime date = today.AddDays(-i);

                // Если данные за эту дату уже есть, пропускаем запрос
                if (existingDates.Contains(date))
                {
                    continue;
                }

                // Запрашиваем данные, если их нет
                var rates = await _getCurrency.GetCurrencyRatesAsync(date);

                foreach (var valute in rates.Valutes)
                {
                    var currency = _dbContext.Currencies.FirstOrDefault(c => c.CharCode == valute.CharCode);

                    // Если валюты нет в базе данных, добавляем её
                    if (currency == null)
                    {
                        currency = new Currency
                        {
                            NumCode = valute.NumCode,
                            CharCode = valute.CharCode,
                            NameCurrency = valute.Name
                        };
                        _dbContext.Currencies.Add(currency);
                        await _dbContext.SaveChangesAsync();
                    }

                    // Проверка, есть ли уже курс валюты для текущей даты
                    var existingRate = _dbContext.CurrencyRates
                        .FirstOrDefault(r => r.CurrencyId == currency.IdCurrency && r.DateCurrencyRate == date);

                    if (existingRate == null)
                    {
                        var exchangeRate = new CurrencyRate
                        {
                            CurrencyId = currency.IdCurrency,
                            DateCurrencyRate = date,
                            Nominal = valute.Nominal,
                            RateValue = decimal.Parse(valute.Value),
                            VunitRate = decimal.Parse(valute.VunitRate)
                        };
                        _dbContext.CurrencyRates.Add(exchangeRate);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
