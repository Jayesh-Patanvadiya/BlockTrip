using BlockTrip.Model;
using BlockTrip.Services;
using BlockTripConsoleApp.Model;
using BlockTripConsoleApp.Services;
using System;
namespace BlockTripConsoleApp
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
                //Query API to get maxtrips per hour for each vehicle type
                await CreateBlockTrip();
                await RemoveBlockTrip();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
        }
        //Pull all records from BlockTripAutoVehicle
        public static async Task<List<BlockTripAutoVehicle>> GetAllRecordsBlockTripAutoVehicles()
        {
            BlockTripAutoVehicleService blockTripAutoVehicleService = new BlockTripAutoVehicleService();
            var result = await blockTripAutoVehicleService.GetAllBlockTripAutoVehicles();
            return result;
        }
        public static async Task CreateBlockTrip()
        {
            try
            {
                BlockTripServices blockTripServices = new BlockTripServices();

                Console.WriteLine("step 1 : Get All Records BlockTripAutoVehicles");
                var maxTripAllowDetails = await GetAllRecordsBlockTripAutoVehicles();
                //Use the api to create block trip records for all periods in step 2 where max trip count is greater than the maxtrips allowed obtained in step 1
                var maxTrips = await GetMaxTripsPerHour(2023, 1);
                foreach (var tripPerHourAllowed in maxTripAllowDetails)
                {
                    foreach (var trip in maxTrips)
                    {
                        //checking  vehicle id is match 
                        if (tripPerHourAllowed.VehicleId == trip.EquipmentVehicleTypeId)
                        {
                            //where max trip count is greater than the maxtrips allowed obtained
                            if (trip.Counts > tripPerHourAllowed.MaxTripsPerHour)
                            {

                                Console.WriteLine("max trip count is greater than the maxtrips allowed obtained in step 1");

                                BlockTripReporting blockTripReporting = new BlockTripReporting()
                                {
                                    Id = "",
                                    CreateDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTimeKind.Utc),
                                    RequestedDateTime = new DateTime(trip.Date.Year, trip.Date.Month, trip.Date.Day, trip.Date.Hour, trip.Date.Minute, trip.Date.Second, DateTimeKind.Utc),
                                    VehicleTypeId = trip.EquipmentVehicleTypeId
                                };
                                //Use the api to create block trip records for all periods in step 2 where max trip count is greater than the maxtrips allowed obtained in step 1

                                await blockTripServices.CreateBlockTripReporting(blockTripReporting);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public static async Task RemoveBlockTrip()
        {
            try
            {
                BlockTripServices blockTripServices = new BlockTripServices();

                Console.WriteLine("step 1 : Get All Records BlockTripAutoVehicles");
                var maxTripAllowDetails = await GetAllRecordsBlockTripAutoVehicles();
                //Use the api to create block trip records for all periods in step 2 where max trip count is greater than the maxtrips allowed obtained in step 1
                var maxTrips = await GetMaxTripsPerHour(2023, 1);
                foreach (var tripPerHourAllowed in maxTripAllowDetails)
                {
                    foreach (var trip in maxTrips)
                    {
                        //checking  vehicle id is match 
                        if (tripPerHourAllowed.VehicleId == trip.EquipmentVehicleTypeId)
                        {
                            //where max trip count is greater than the maxtrips allowed obtained
                            if (trip.Counts < tripPerHourAllowed.MaxTripsPerHour)
                            {

                                Console.WriteLine("max trip count is greater than the maxtrips allowed obtained in step 1");

                                //Use the api to create block trip records for all periods in step 2 where max trip count is greater than the maxtrips allowed obtained in step 1

                                await blockTripServices.RemoveBlockTripReporting(tripPerHourAllowed.Id);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }



        //Query API to get maxtrips per hour for each vehicle type

        public static async Task<List<MaxtripsPerHour>> GetMaxTripsPerHour(int year, int equipmentVehicleTypeId)
        {
            Console.WriteLine("Step 2: Query API to get maxtrips per hour for each vehicle type");

            BlockTripServices blockTripServices = new BlockTripServices();

            // Used TripAnalyticsRaw api endpoint to Query against the trips table in BigQuery, using the query, to get trip count per hour for all hour periods in the next 1 year                

            string sqlQuery = $"SELECT  PickupDT as Date,EXTRACT(day FROM DATETIME(PickupDT)) as PickupDay, EXTRACT(hour FROM DATETIME(PickupDT)) as Hour ,count(EXTRACT(hour FROM DATETIME(PickupDT))) as Counts,EquipmentVehicleTypeId as EquipmentVehicleTypeId, FROM `ezshuttle2020.ezx.trips` WHERE EXTRACT(year FROM DATETIME(PickupDT))={year} and cast(equipmentVehicleTypeId as INT64) = {equipmentVehicleTypeId} group by Date,EquipmentVehicleTypeId order by Date asc";
            Console.WriteLine($"sqlQuery: {sqlQuery}");

            var result = await blockTripServices.GetMaxTripsPerHour(sqlQuery);
            if (result is not null)
            {
                var trips = result.ToObject<List<MaxtripsPerHour>>();
                return trips;
            }
            else
            {
                return new List<MaxtripsPerHour>();
            }
        }


    }
}
