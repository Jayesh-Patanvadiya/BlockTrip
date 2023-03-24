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
        //public static async Task<List<Quote>> GetMaxTripsPerHour(int year, int equipmentVehicleTypeId)
        //{
        //    Console.WriteLine("Step 2: Query API to get maxtrips per hour for each vehicle type  \n");

        //    BlockTripServices blockTripServices = new BlockTripServices();

        //    // Used TripAnalyticsRaw api endpoint to Query against the trips table in BigQuery, using the query, to get trip count per hour for all hour periods in the next 1 year                

        //    string sqlQuery = $"SELECT  PickupDT as Date,EXTRACT(day FROM DATETIME(PickupDT)) as PickupDay, EXTRACT(hour FROM DATETIME(PickupDT)) as Hour ,count(EXTRACT(hour FROM DATETIME(PickupDT))) as Counts,EquipmentVehicleTypeId as EquipmentVehicleTypeId, FROM `ezshuttle2020.ezx.trips` WHERE EXTRACT(year FROM DATETIME(PickupDT))={year} and cast(equipmentVehicleTypeId as INT64) = {equipmentVehicleTypeId} group by Date,EquipmentVehicleTypeId order by Date asc";
        //    Console.WriteLine($"sqlQuery: {sqlQuery}  \n");

        //    var result = await blockTripServices.GetQuote(sqlQuery);
        //    if (result is not null)
        //    {
        //        var trips = result.ToObject<List<Quote>>();
        //        return trips;
        //    }
        //    else
        //    {
        //        return new List<Quote>();
        //    }
        //}


    }
}
