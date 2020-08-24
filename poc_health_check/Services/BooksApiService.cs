using books_api.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace poc_health_check.Services
{
    public class BooksApiService
    {
        private readonly IConfiguration configuration;

        public BooksApiService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                var uri = configuration.GetValue<string>("BookApi");

                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                var books = JsonConvert.DeserializeObject<List<Book>>(jsonString);
                return books;
            }
        }
    }
}
