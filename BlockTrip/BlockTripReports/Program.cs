using BlockTrip.Model;
using BlockTrip.Services;
using BlockTripConsoleApp.Model;
using BlockTripConsoleApp.Services;
using System;
namespace BlockTripReports
{
    public class Program
    {
        public Program()
        {

        }
        public static async Task Main(string[] args)
        {
            try
            {

                //Do a GET to obtain ALL BlockTripReports with missing pricing
                await GetAllBlockTripReportingWithoutPrice();
                await GetQuote("EZKnYwMDEqMnwtMzMuOTgyNzg2NXwyNS41NDA2NTI3fC0xfDQ5MnwyIERvcmNoZXN0ZXIgUmQsIFNwcmluZ2ZpZWxkLCBHcWViZXJoYSwgNjA3MCwgU291dGggQWZyaWNhfDEwMDd8MXwyOS45NjExfA==", "EZKnYwMDEqM3wtMzMuOTg3MjcxOXwyNS42MTQyOTQ3fC0xfHxQb3J0IEVsaXphYmV0aCBJbnRlcm5hdGlvbmFsIEFpcnBvcnR8MTAwN3wtMXwzMS42MDc5fFBMWg==", 18, 5, 2023, 15, 18, 6981, 1, 21);

                //Console application  shutdown
                Environment.Exit(1);// exit

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
        }

        //Do a GET to obtain ALL BlockTripReports with missing pricing
        public static async Task<List<BlockTripReportingPriceDto>> GetAllBlockTripReportingWithoutPrice()
        {
            BlockTripReportingService blockTripReportingServices = new BlockTripReportingService();

            return await blockTripReportingServices.GetAllBlockTripReportingWithoutPrice();
        }
        // Iterate through all records and obtain a price using the quote endpoint.
        public static async Task<List<Quote>> GetQuote(string pickupPlaceId, string dropOffPlaceId, int day, int month, int year, int hour, int minute, int clientId, int vehicleTypeIdCsvList, int pricePlanId)
        {
            BlockTripServices blockTripServices = new BlockTripServices();

            var result = await blockTripServices.GetQuote(pickupPlaceId, dropOffPlaceId, day, month, year, hour, minute, clientId, vehicleTypeIdCsvList, pricePlanId);
            return result;
        }

        // sync all BlockTripReporting to BigQuery


    }
}
