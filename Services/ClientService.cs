using Newtonsoft.Json;
using SWM.TechnicalAssessment.Handlers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SWM.TechnicalAssessment.Services
{
    public interface IClientService
    {
        Task<IList<T>> GetAsync<T>(string path);
    }
    public class ClientService: IClientService
    {
        public async Task<IList<T>> GetAsync<T>(string path)
        {
            IList<T> result = null;
            using (var client = new HttpClient(new RetryHandler(new HttpClientHandler())))
            {
                client.BaseAddress = new Uri("https://f43qgubfhf.execute-api.ap-southeast-2.amazonaws.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    var resultString = await response.Content.ReadAsStringAsync();
                    try
                    {
                        result = JsonConvert.DeserializeObject<List<T>>(resultString);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Data error. Unable to deserialize users. Exception Message: {ex.Message}");
                    }
                }
                return result;
            }
        }
    }
}
