using CurrencyTracker.XmlModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CurrencyTracker.Services
{
    public class GetCurrency
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<ValCurs> GetCurrencyRatesAsync(DateTime date)
        {
            // Добавление поддержки кодировки для корректной работы с цбр

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string url = $"https://www.cbr.ru/scripts/XML_daily.asp?date_req={date:dd/MM/yyyy}";

            using var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Чтение и десериализация XML-ответа от источника

            using var stream = await response.Content.ReadAsStreamAsync();

            using var reader = new StreamReader(stream, Encoding.GetEncoding("windows-1251"));

            string xmlContent = await reader.ReadToEndAsync();

            XmlSerializer serializer = new XmlSerializer(typeof(ValCurs));
            using StringReader stringReader = new StringReader(xmlContent);

            // Десериализация xml в объект ValCurs
            return (ValCurs)serializer.Deserialize(stringReader);
        }
    }
}