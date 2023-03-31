using BlockTrip.Model;
using BlockTrip.Services;
using BlockTripConsoleApp.Model;
using BlockTripConsoleApp.Services;

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
                //for count execution time
                var watch = System.Diagnostics.Stopwatch.StartNew();

                //Do a GET to obtain ALL BlockTripReports with missing pricing
                await GetAllBlockTripReportingWithoutPrice();

                //Iterate through all records and obtain a price using the quote endpoint.
                // authentication auth token is required
                await GetQuote("EZKnYwMDEqMnwtMzMuOTgyNzg2NXwyNS41NDA2NTI3fC0xfDQ5MnwyIERvcmNoZXN0ZXIgUmQsIFNwcmluZ2ZpZWxkLCBHcWViZXJoYSwgNjA3MCwgU291dGggQWZyaWNhfDEwMDd8MXwyOS45NjExfA==", "EZKnYwMDEqM3wtMzMuOTg3MjcxOXwyNS42MTQyOTQ3fC0xfHxQb3J0IEVsaXphYmV0aCBJbnRlcm5hdGlvbmFsIEFpcnBvcnR8MTAwN3wtMXwzMS42MDc5fFBMWg==", 18, 5, 2023, 15, 18, 6981, 1, 21);

                //Use the PUT method on the api to update the price
                BlockTripReporting blockTripReporting = new BlockTripReporting()
                {
                    CreateDT = DateTime.UtcNow,
                    Hour = 12,
                    Id = "vrIPLFJnS8orWSH5HM13",
                    Price = 0,
                    PriceListId = 0,
                    RequestedDateTime = DateTime.UtcNow,
                    VehicleTypeId = 1
                };
                await UpdateBlockTripReporting(blockTripReporting);

                //sync all BlockTripReporting to BigQuery
                await GetMaxTripsPerHour(2022, 1);   // approx. 5.337 SECOND to execute method

                watch.Stop();
                Console.WriteLine(string.Format("Total time for execute all 4 methods : {0}  \n", watch.ElapsedMilliseconds));

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

        //Use the PUT method on the api to update the price
        public static async Task UpdateBlockTripReporting(BlockTripReporting blockTripReporting)
        {
            BlockTripServices blockTripServices = new BlockTripServices();

            await blockTripServices.UpdateBlockTripReporting(blockTripReporting);
        }

        // sync all BlockTripReporting to BigQuery
        public static async Task<List<MaxtripsPerHour>> GetMaxTripsPerHour(int year, int equipmentVehicleTypeId)
        {

            // the code that you want to measure comes here
            Console.WriteLine("Query API to get maxtrips per hour for each vehicle type  \n");


            // Used TripAnalyticsRaw api endpoint to Query against the trips table in BigQuery, using the query, to get trip count per hour for all hour periods in the next 1 year                

            string sqlQuery = $"SELECT  PickupDT as Date,EXTRACT(day FROM DATETIME(PickupDT)) as PickupDay, EXTRACT(hour FROM DATETIME(PickupDT)) as Hour ,count(EXTRACT(hour FROM DATETIME(PickupDT))) as Counts,EquipmentVehicleTypeId as EquipmentVehicleTypeId, FROM `ezshuttle2020.ezx.trips` WHERE EXTRACT(year FROM DATETIME(PickupDT))={year} and cast(equipmentVehicleTypeId as INT64) = {equipmentVehicleTypeId} group by Date,EquipmentVehicleTypeId order by Date asc";
            Console.WriteLine($"sqlQuery: {sqlQuery}  \n");


            BlockTripServices blockTripServices = new BlockTripServices();

            return await blockTripServices.GetMaxTripsPerHour(sqlQuery);


        }

    }
}
