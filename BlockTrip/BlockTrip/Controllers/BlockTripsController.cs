using BlockTrip.Model;
using BlockTrip.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockTrip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockTripsController : ControllerBase
    {
        IBlockTripsService _blockTripService;
        IBlockTripReportingService _blockTripReportingService;
        public BlockTripsController(IBlockTripsService blockTripService, IBlockTripReportingService blockTripReportingService)
        {
            _blockTripService = blockTripService;
            _blockTripReportingService = blockTripReportingService;
        }
        [HttpPost]
        public async Task<BlockTrips> CreateBlockTrip([FromBody] BlockTrips blockTrip)
        {
            var createResult = await _blockTripService.CreateBlockTrip(blockTrip);
            return createResult;

        }
        [HttpGet]
        public async Task<List<BlockTrips>> GetAllBlockTrips()
        {
            return await _blockTripService.GetAllBlockTrips();
        }

        [HttpGet("blockTripId")]
        public async Task<BlockTrips> GetBlockTripById(string blockTripId)
        {
            return await _blockTripService.GetBlockTripById(blockTripId);
        }

        [HttpGet("blockTrip")]
        public async Task<bool> GetBlockTripByDate(int day, int month, int year, int vehcileTypeId)
        {
            var blockTrips = await _blockTripService.GetAllBlockTrips();
            DateTime requestedDate = new DateTime(year, month, day);
            DateTime dateTimeNow = DateTime.Now;
            var result = blockTrips.Where(x => x.StartDT >= dateTimeNow.Date  && x.VehicleId == vehcileTypeId).ToList();
            if (result.Count > 0)
            {
                return true;
            }
            else
            {
                BlockTripReporting blockTripReporting = new BlockTripReporting()
                {
                    CreateDT = dateTimeNow,
                    RequestedDateTime = requestedDate,
                    VehicleTypeId = vehcileTypeId
                };
                await _blockTripReportingService.CreateBlockTripReporting(blockTripReporting);
                return false;
            }

        }


        [HttpPut]
        public async Task<BlockTrips> UpdateBlockTrip([FromBody] BlockTrips blockTrip)
        {

            return await _blockTripService.UpdateBlockTrip(blockTrip);
        }

        [HttpDelete]
        public async Task<string> DeleteBlockTrip(string blockTripId)
        {
            return await _blockTripService.DeleteBlockTrip(blockTripId);
        }
    }
}
