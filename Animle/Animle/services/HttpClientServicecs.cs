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
                var configuration = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                    .Build();

                client.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", configuration.GetSection("MalId").Value);

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
                client.Dispose();
            }
        }
    }
}
