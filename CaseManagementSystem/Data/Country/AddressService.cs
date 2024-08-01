using System.Net.Http;using System.Threading.Tasks;

namespace CaseManagementSystem.Data.Country
{
    public class AddressService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "j4DD-Sgv1Uis3NtYnXW_CQ15746";

        public AddressService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAutocompleteSuggestions(string query)
        {          
            var apiUrl = $"https://api.getAddress.io/autocomplete?key={_apiKey}&text={query}";
            var response = await _httpClient.GetStringAsync(apiUrl);
            return response;
        }
    }
}