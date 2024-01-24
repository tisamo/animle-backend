using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

namespace Animle.services
{
    public class MyanimeListClientHttpService
    {
        async public Task<string> ReturnAny(string subUrl, string apiUrl = "https://api.myanimelist.net/v2/")
        {
            HttpClient client = new HttpClient();

            apiUrl += subUrl;

            try
            {
                client.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", "5ab79100e2772855f94a8372f5863c36");

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                   
                    return responseBody;

                }
                else
                {
                    return null;

                }
            }
            catch (HttpRequestException e)
            {
                return null;
            }
            finally
            {
                // Dispose the HttpClient instance to free up resources
                client.Dispose();
            }
        }
    }
}
