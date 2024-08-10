using System.Net.Http;
using System.Text.Json;

namespace TestAssignmentForDigitalCloud.Models
{
    /// <summary>
    /// Клас, який реалізовує запити до CoinCapAPI
    /// </summary>
    internal class CoinCapAPI
    {
        /// <summary>
        /// Апі ключ CoinCap
        /// </summary>
        private string apiKey;

        /// <summary>
        /// Екземпляр HttpClient
        /// </summary>
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Конструктор з ключем Апі
        /// </summary>
        /// <param name="apiKey">Апі ключ CoinCap</param>
        public CoinCapAPI(string apiKey)
        {
            this.apiKey = apiKey;
        }

        /// <summary>
        /// основна сторінка. Запит для отримання певної кількості найпопулярніших криптовалю
        /// </summary>
        /// <param name="numberCryptocurrency">Кількість криптовалют для запиту</param>
        /// <returns>List<CoinCapHistory>, який містить набор найпопулярніших криптовалют відсортованих по рангу</returns>
        public async Task<List<Coin>> GetPopularCryptocurrencies(int numberCryptocurrency)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.coincap.io/v2/assets?limit={numberCryptocurrency}");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return ConvertFromJsonToObjects<Coin>(content);
        }

        /// <summary>
        /// сторінка з деталями по конкретній криптовалюті. Запит для отримання priceUSD, volumeUsd24Hr, changePercent24Hr по певній крипті
        /// </summary>
        /// <param name="cryptocurrency">Назва або код криптовалюти</param>
        /// <returns>List<CoinCapHistory> криптовалюти, які відповідають вимогам пошуку</returns>
        public async Task<List<Coin>> GetCryptocurrencies(string cryptocurrency)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.coincap.io/v2/assets?search={cryptocurrency}");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return ConvertFromJsonToObjects<Coin>(content);
        }

        /// <summary>
        /// сторінка з деталями по конкретній криптовалюті. Запит для отримання магазинів по певній крипті
        /// </summary>
        /// <param name="cryptocurrency">Назва криптовалюти</param>
        /// <returns>List<CoinCapHistory> Магазини та їх пропозиції обміну</returns>
        public async Task<List<Market>> GetMarkets(string cryptocurrency)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.coincap.io/v2/assets/{cryptocurrency}/markets");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return ConvertFromJsonToObjects<Market>(content);
        }

        /// <summary>
        /// сторінка з деталями по конкретній криптовалюті. Запит для отримання посилання на конкретний магазин
        /// </summary>
        /// <param name="market"> Назва магазину </param>
        /// <returns>List<CoinCapHistory> рядок з деталями стосовно магазину </returns>
        public async Task<string> GetMarketURL(string market)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.coincap.io/v2/exchanges/{market}");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// конвертер з JSON у клас (Json серіалізація)
        /// </summary>
        /// <param name="content"> рядок з JSONом </param>
        /// <returns>List<CoinCapHistory> Ліст з деталями магазину </returns>
        public List<T> ConvertFromJsonToObjects<T>(string content)
        {
            List<T> objects = new List<T>();

            using (JsonDocument doc = JsonDocument.Parse(content))
            {
                JsonElement root = doc.RootElement;
                JsonElement dataElement = root.GetProperty("data");
                if (dataElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement item in dataElement.EnumerateArray())
                    {
                        T obj = JsonSerializer.Deserialize<T>(item.GetRawText());
                        objects.Add(obj);
                    }
                }
            }
            return objects;
        }

    }
}
