using BlockTrip.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;

namespace BlockTripConsoleApp.Services
{
    public class BlockTripServices : IBlockTripServices
    {
        private readonly HttpClient _blockTripApiClient = new HttpClient();
        private readonly RestClient _httpClient;
        public string _baseUrl { get; set; }

        public BlockTripServices()
        {
            _blockTripApiClient.BaseAddress = new Uri("https://api.ezshuttle.co.za/");
            _baseUrl = "http://localhost:5032/api/BlockTripReporting/";
            _httpClient = new RestClient(new Uri(_baseUrl));
        }
        public async Task<JArray> GetMaxTripsPerHour(string sqlQuery)
        {
            try
            {
                Console.WriteLine("Used TripAnalyticsRaw api endpoint to Query against the trips table in BigQuery, using the query, to get trip count per hour for all hour periods in the next 1 year  \n");

                // Used TripAnalyticsRaw api endpoint to Query against the trips table in BigQuery, using the query, to get trip count per hour for all hour periods in the next 1 year
                var response = await _blockTripApiClient.GetAsync($"ezx/reporting/api/TripAnalyticsRaw?query={sqlQuery}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var propertiesToRemove = new[] { "__key__", "__error__", "__has_error__" }.ToList();
                    var responseAsJArray = JArray.Parse(responseContent);
                    responseAsJArray.Descendants()
                        .OfType<JProperty>()
                        .Where(p => propertiesToRemove.Contains(p.Name))
                        .ToList()
                        .ForEach(p => p.Remove());

                    return responseAsJArray;
                }
                else
                {
                    throw new Exception($"GetMaxTripsPerHour API call returned status {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        //Use the api to create block trip records for all periods in step 2 where max trip count is greater than the maxtrips allowed obtained in step 1

        public async Task CreateBlockTripReporting(BlockTripReporting blockTripReporting)
        {
            
            var request = new RestRequest(_baseUrl, Method.Post);
            request.AddHeader("Content-Type", "application/json");

            var blockTripReportingSerialize = JsonConvert.SerializeObject(blockTripReporting);
            request.AddParameter("application/json", blockTripReportingSerialize, ParameterType.RequestBody);


            //execute request
            var response = await _httpClient.ExecuteAsync(request);
            Console.WriteLine(string.Format("CreateBlockTripReporting StatusCode :{0} and Content : {1} and response.ErrorMessage :{2}  \n", response.StatusCode,response.Content,response.ErrorMessage));

        }
        public async Task RemoveBlockTripReporting(string blockTripReportingId)
        {

            var request = new RestRequest(_baseUrl, Method.Delete);
            request.AddHeader("Content-Type", "application/json");
            request.AddQueryParameter("blockTripReportingId", blockTripReportingId); 


            //execute request
            var response = await _httpClient.ExecuteAsync(request);
            Console.WriteLine(string.Format("RemoveBlockTripReporting StatusCode :{0} and Content {1}  \n", response.StatusCode, response.Content));

        }
    }
}
