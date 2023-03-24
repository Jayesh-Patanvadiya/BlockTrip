using BlockTrip.Model;
using System.Threading.Tasks;

namespace BlockTrip.Services
{
    public interface IBlockTripReportingService
    {
        Task<BlockTripReporting> CreateBlockTripReporting(BlockTripReporting blockTripReporting);
        Task<List<BlockTripReporting>> GetAllBlockTripReporting();
        Task<BlockTripReporting> UpdateBlockTripReporting(BlockTripReporting blockTripReporting);
        Task<BlockTripReporting> GetBlockTripReportingById(string blockTripReportingid);

        Task<string> DeleteBlockTripReporting(string blockTripReportingId);

        Task<List<BlockTripReportingPriceDto>> GetAllBlockTripReportingWithoutPrice();

    }
}
