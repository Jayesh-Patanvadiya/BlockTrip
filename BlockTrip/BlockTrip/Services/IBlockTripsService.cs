using BlockTrip.Model;

namespace BlockTrip.Services
{
    public interface IBlockTripsService
    {
        Task<BlockTrips> CreateBlockTrip(BlockTrips blockTrip);
        Task<List<BlockTrips>> GetAllBlockTrips();
        Task<BlockTrips> UpdateBlockTrip(BlockTrips blockTrip);
        Task<BlockTrips> GetBlockTripById(string blockTripid);
        Task<string> DeleteBlockTrip(string blockTripId);

    }
}
