using BlockTrip.Model;
using BlockTrip.Services;

namespace BlockTripConsoleApp
{
    internal class Program
    {
        public Program()
        {

        }
        public static async void Main()
        {
            try
            {
               var AllRecords =  await GetAllRecordsBlockTripAutoVehicles();
                await FilterVehicleType(AllRecords);
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
        public static async Task<List<BlockTripAutoVehicle>> FilterVehicleType(List<BlockTripAutoVehicle> blockTripAutoVehicles)
        {

            BlockTripAutoVehicleService blockTripAutoVehicleService = new BlockTripAutoVehicleService();
            var result = await blockTripAutoVehicleService.GetAllBlockTripAutoVehicles();
            return result;
        }


    }
}
