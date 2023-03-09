using BlockTrip.Model;

namespace BlockTrip.Services
{
    public interface IBlockTripAutoVehicleService
    {
        Task<BlockTripAutoVehicle> CreateBlockTripAutoVehicle(BlockTripAutoVehicle blockTripAutoVehicle);
        Task<List<BlockTripAutoVehicle>> GetAllBlockTripAutoVehicles();
        Task<BlockTripAutoVehicle> UpdateBlockTripAutoVehicle(BlockTripAutoVehicle blockTripAutoVehicles);
        Task<BlockTripAutoVehicle> GetBlockTripAutoVehicleId(string blockTripAutoVehicleid);
        Task<string> DeleteBlockTripAutoVehicle(string blockTripAutoVehicleId);

    }
}
