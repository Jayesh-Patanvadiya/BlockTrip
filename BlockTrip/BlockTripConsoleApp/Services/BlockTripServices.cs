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

        public async Task<List<Quote>> GetQuote(string pickupPlaceId, string dropOffPlaceId, int day, int month, int year, int hour, int minute, int clientId, int vehicleTypeIdCsvList, int pricePlanId)
        {
            try
            {
                var client = new HttpClient();

                string url = string.Format("https://api.ezshuttle.co.za/ezx/quote/api/quote?pickupPlaceId={0}&dropOffPlaceId={1}&day={2}&month={3}&year={4}&hour={5}&minute={6}&clientId={7}&vehicleTypeIdCsvList={8}&pricePlanId={9}", pickupPlaceId, dropOffPlaceId, day, month, year, hour, minute, clientId, vehicleTypeIdCsvList, pricePlanId);
                var request = new HttpRequestMessage
                {

                    Method = HttpMethod.Get,

                    RequestUri = new Uri(url),
                    Headers =
    {
        { "Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjpbImRldmVsb3BlciIsIkRFVkVMT1BFUiJdLCJyb2xlIjoiQURNSU5JU1RSQVRPUiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL3NpZCI6IjI5NDA1IiwibmJmIjoxNjgwMTU3NDUwLCJleHAiOjE2ODAyNDU2NTAsImlhdCI6MTY4MDE1OTI1MCwiaXNzIjoiZXpzaHV0dGxlLmNvLnphIiwiYXVkIjoiZXpzaHV0dGxlLmNvLnphIn0.eJHqruIe8PWXg8qpyWpAmWgAlYRIYdqOeNU6GS3YnHY" },
    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(body);
                    var deserialized = JsonConvert.DeserializeObject<List<Quote>>(body);
                    return deserialized;
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
            Console.WriteLine(string.Format("CreateBlockTripReporting StatusCode :{0} and Content : {1} and response.ErrorMessage :{2}  \n", response.StatusCode, response.Content, response.ErrorMessage));

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

        public async Task UpdateBlockTripReporting(BlockTripReporting blockTripReporting)
        {

            var request = new RestRequest(_baseUrl, Method.Put);
            request.AddHeader("Content-Type", "application/json");

            var blockTripReportingSerialize = JsonConvert.SerializeObject(blockTripReporting);
            request.AddParameter("application/json", blockTripReportingSerialize, ParameterType.RequestBody);


            //execute request
            var response = await _httpClient.ExecuteAsync(request);
            Console.WriteLine(string.Format("UpdateBlockTripReporting StatusCode :{0} and Content : {1} and response.ErrorMessage :{2}  \n", response.StatusCode, response.Content, response.ErrorMessage));

        }
    }
}
