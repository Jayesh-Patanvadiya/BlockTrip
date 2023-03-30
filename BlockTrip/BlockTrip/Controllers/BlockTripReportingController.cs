using BlockTrip.Model;
using BlockTrip.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockTrip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockTripReportingController : ControllerBase
    {
        IBlockTripReportingService _blockTripReportingService;
        public BlockTripReportingController(IBlockTripReportingService blockTripReportingService)
        {
            _blockTripReportingService = blockTripReportingService;
        }
        [HttpPost]
        public async Task<BlockTripReporting> CreateBlockTripReporting([FromBody] BlockTripReporting blockTripReporting)
        {
            var createResult = await _blockTripReportingService.CreateBlockTripReporting(blockTripReporting);
            return createResult;

        }
        [HttpGet]
        public async Task<List<BlockTripReporting>> GetAllBlockTripReporting()
        {
            return await _blockTripReportingService.GetAllBlockTripReporting();
        }

        [HttpGet("blockTripReportingWithoutPrice")]
        public async Task<List<BlockTripReportingPriceDto>> GetAllBlockTripReportingWithoutPrice(int currentPage, int items)
        {
            var allrecords = await _blockTripReportingService.GetAllBlockTripReportingWithoutPrice();
            //paged list logic
            var result = allrecords.Skip((currentPage - 1) * items).Take(items).ToList();
            return result;
        }

        [HttpGet("blockTripReportingId")]
        public async Task<BlockTripReporting> GetBlockTripReportingId(string blockTripReportingId)
        {
            return await _blockTripReportingService.GetBlockTripReportingById(blockTripReportingId);
        }


        //Add Hour and PriceListId to existing endpoint GET (QueryString int day, int month, int year, int vehcileTypeId)
        [HttpGet("blockTripReportingByDate")]
        public async Task<List<BlockTripReporting>> GetBlockTripReportingByDateTime(int hour, int day, int month, int year, int vehcileTypeId, int priceListId)
        {

            var blockTripsReporting = await _blockTripReportingService.GetAllBlockTripReporting();

            DateTime requestedDate = new DateTime(year, month, day, hour, 0, 0, DateTimeKind.Utc);

            var result = blockTripsReporting.Where(x => x.RequestedDateTime >= requestedDate && x.VehicleTypeId == vehcileTypeId && x.PriceListId == priceListId).ToList();
            return result;
        }

        // Use the PUT method on the api to update the price
        [HttpPut]
        public async Task<BlockTripReporting> UpdateBlockTripReporting([FromBody] BlockTripReporting blockTripReporting)
        {

            return await _blockTripReportingService.UpdateBlockTripReporting(blockTripReporting);
        }

        [HttpDelete]
        public async Task<string> DeleteBlockTrip(string blockTripReportingId)
        {
            return await _blockTripReportingService.DeleteBlockTripReporting(blockTripReportingId);
        }
    }
}
