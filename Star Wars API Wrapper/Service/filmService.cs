using System.Net;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Star_Wars_API_Wrapper.Models;

namespace Star_Wars_API_Wrapper.Service
{
    public class filmService
    {
        private readonly HttpClient _httpClient;

        public filmService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //Performs an asynchronous HTTP GET request to the url specified in the controller using and instance of HttpClient
        public async Task<string> GetSpecifiedJson(string Url)
        {
            var response = await _httpClient.GetAsync(Url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // Handle 404 Not Found
                return null; // or throw a specific exception, log, etc.
            }
            else
            {
                // Log the error and throw an exception
                response.EnsureSuccessStatusCode();
                return null; // This line should never be reached
            }
        }
    }
}
