using Animle.Classes;
using Microsoft.Extensions.Options;

namespace Animle.Helpers
{
    public class MyanimeListClientHttpService
    {
        private readonly ConfigSettings _appSettings;

        public MyanimeListClientHttpService(IOptions<ConfigSettings> options)
        {
            _appSettings = options.Value;
        }

        async public Task<string> ReturnAny(string subUrl, string apiUrl = "https://api.myanimelist.net/v2/")
        {
            var client = new HttpClient();


            apiUrl += subUrl;

            try
            {
                var malId = _appSettings.MalId;

                client.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", malId);

                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
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