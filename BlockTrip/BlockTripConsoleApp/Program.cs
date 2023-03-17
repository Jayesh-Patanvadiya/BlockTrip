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

                //Console application  shutdown
                Environment.Exit(1);// exit

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
        }
        //Pull all records from BlockTripAutoVehicle
        public static async Task<List<BlockTripAutoVehicle>> GetAllRecordsBlockTripAutoVehicles()
        {
            Console.WriteLine("Pull all records from BlockTripAutoVehicle \n");

            BlockTripAutoVehicleService blockTripAutoVehicleService = new BlockTripAutoVehicleService();
            var result = await blockTripAutoVehicleService.GetAllBlockTripAutoVehicles();
            return result;
        }
        //pull all records from BlockTrips
        public static async Task<List<BlockTripReporting>> GetAllRecordsBlockTripReporting()
        {
            Console.WriteLine("pull all records from BlockTrips \n");

            BlockTripReportingService blockTripReportingService = new BlockTripReportingService();
            var result = await blockTripReportingService.GetAllBlockTripReporting();
            return result;
        }
        public static async Task CreateBlockTrip()
        {
            try
            {
                BlockTripServices blockTripServices = new BlockTripServices();

                Console.WriteLine("step 1 : Get All Records BlockTripAutoVehicles \n");
                var maxTripAllowDetails = await GetAllRecordsBlockTripAutoVehicles();

                //Use the api to create block trip records for all periods in step 2 where max trip count is greater than the maxtrips allowed obtained in step 1
                Console.WriteLine("Get All Records MaxTripsPerHour \n");
                var maxTrips = await GetMaxTripsPerHour(DateTime.Now.Year, 1);

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

                                Console.WriteLine(string.Format("max trip count is greater than the maxtrips allowed obtained in step 1 trip Count :{0} and tripPerHourAllowed {1}  \n", trip.Counts, tripPerHourAllowed.MaxTripsPerHour));

                                BlockTripReporting blockTripReporting = new BlockTripReporting()
                                {
                                    Id = "",
                                    CreateDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTimeKind.Utc),
                                    RequestedDateTime = new DateTime(trip.Date.Year, trip.Date.Month, trip.Date.Day, trip.Date.Hour, trip.Date.Minute, trip.Date.Second, DateTimeKind.Utc),
                                    VehicleTypeId = trip.EquipmentVehicleTypeId
                                };
                                Console.WriteLine(string.Format("Creating block trip records for vehicle id: {0} and RequestedDateTime {1}  \n", blockTripReporting.VehicleTypeId, blockTripReporting.RequestedDateTime));

                                //Used the api to create block trip records for all periods in step 2 where max trip count is greater than the maxtrips allowed obtained in step 1
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

                Console.WriteLine("step 1 : Get All Records BlockTripAutoVehicles for maxTripAllowed \n");
                var maxTripAllowDetails = await GetAllRecordsBlockTripAutoVehicles();

                //Get records from BlockTripReporting, created on step 3 : CreateBlockTrip()
                var blockTripReporting = await GetAllRecordsBlockTripReporting();
                
                //Use the api to create block trip records for all periods in step 2 where max trip count is greater than the maxtrips allowed obtained in step 1
                var maxTrips = await GetMaxTripsPerHour(DateTime.Now.Year, 1);
               
                foreach (var tripPerHourAllowed in maxTripAllowDetails)
                {
                    foreach (var trip in maxTrips)
                    {
                        //checking  vehicle id is match 
                        if (tripPerHourAllowed.VehicleId == trip.EquipmentVehicleTypeId)
                        {
                            Console.WriteLine("max trip count is LESS than the maxtrips allowed obtained in step 1");
                            //where max trip count is LESS than the maxtrips allowed obtained
                            if (trip.Counts < tripPerHourAllowed.MaxTripsPerHour)
                            {
                                //found records that are match with blockTripReporting based on Time
                                var deleteTrip = blockTripReporting.Where(x => x.RequestedDateTime == trip.Date).ToList();

                                //Used the API to REMOVE block trip records for all periods in step 2 where max trip count is LESS than the maxtrips allowed obtained in step 1
                                foreach (var removeTrip in deleteTrip)
                                {
                                    Console.WriteLine(string.Format("Removing block trip records for RequestedDateTime: {0}  \n", removeTrip.RequestedDateTime));

                                    await blockTripServices.RemoveBlockTripReporting(removeTrip.Id);
                                }
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
            Console.WriteLine("Step 2: Query API to get maxtrips per hour for each vehicle type  \n");

            BlockTripServices blockTripServices = new BlockTripServices();

            // Used TripAnalyticsRaw api endpoint to Query against the trips table in BigQuery, using the query, to get trip count per hour for all hour periods in the next 1 year                

            string sqlQuery = $"SELECT  PickupDT as Date,EXTRACT(day FROM DATETIME(PickupDT)) as PickupDay, EXTRACT(hour FROM DATETIME(PickupDT)) as Hour ,count(EXTRACT(hour FROM DATETIME(PickupDT))) as Counts,EquipmentVehicleTypeId as EquipmentVehicleTypeId, FROM `ezshuttle2020.ezx.trips` WHERE EXTRACT(year FROM DATETIME(PickupDT))={year} and cast(equipmentVehicleTypeId as INT64) = {equipmentVehicleTypeId} group by Date,EquipmentVehicleTypeId order by Date asc";
            Console.WriteLine($"sqlQuery: {sqlQuery}  \n");

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
