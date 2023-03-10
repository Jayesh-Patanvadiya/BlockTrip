using BlockTrip.Services;

namespace BlockTripConsoleApp
{
    internal class Program
    {

        // need to config dependency injection 
        public Program()
        {

        }
        public static async void Main()
        {
            try
            {
                await GetAllBlockTripAutoVehicles();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
        }
        public static async Task GetAllBlockTripAutoVehicles()
        {
            BlockTripAutoVehicleService blockTripAutoVehicleService = new BlockTripAutoVehicleService();
            var result = await blockTripAutoVehicleService.GetAllBlockTripAutoVehicles();
        }


    }
}
