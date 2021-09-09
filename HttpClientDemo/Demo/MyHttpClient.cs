using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientDemo.Demo
{
    public class MyHttpClient
    {
        private readonly HttpClient _client;

        public MyHttpClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> request()
        {
            return await _client.GetStringAsync("/helloWorld");
        }
    }
}