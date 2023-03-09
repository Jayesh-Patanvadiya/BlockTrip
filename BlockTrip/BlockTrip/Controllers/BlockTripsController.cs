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
        public BlockTripsController(IBlockTripsService blockTripService)
        {
            _blockTripService = blockTripService;
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
