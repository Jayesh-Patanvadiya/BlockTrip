using BlockTrip.Model;
using BlockTrip.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockTrip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockTripAutoVehicleController : ControllerBase
    {
        IBlockTripAutoVehicleService _blockTripAutoVehicleService;
        public BlockTripAutoVehicleController(IBlockTripAutoVehicleService blockTripAutoVehicleService)
        {
            _blockTripAutoVehicleService = blockTripAutoVehicleService;
        }
        [HttpPost]
        public async Task<BlockTripAutoVehicle> CreateBlockTripAutoVehicle([FromBody] BlockTripAutoVehicle blockTripAutoVehicle)
        {
            var createResult = await _blockTripAutoVehicleService.CreateBlockTripAutoVehicle(blockTripAutoVehicle);
            return createResult;

        }
        [HttpGet]
        public async Task<List<BlockTripAutoVehicle>> GetAllBlockTripAutoVehicles()
        {
            return await _blockTripAutoVehicleService.GetAllBlockTripAutoVehicles();
        }

        [HttpGet("blockTripAutoVehicleId")]
        public async Task<BlockTripAutoVehicle> GetBlockTripAutoVehicleId(string blockTripAutoVehicleId)
        {
            return await _blockTripAutoVehicleService.GetBlockTripAutoVehicleId(blockTripAutoVehicleId);
        }


        [HttpPut]
        public async Task<BlockTripAutoVehicle> UpdateBlockTripAutoVehicle([FromBody] BlockTripAutoVehicle blockTripAutoVehicle)
        {

            return await _blockTripAutoVehicleService.UpdateBlockTripAutoVehicle(blockTripAutoVehicle);
        }

        [HttpDelete]
        public async Task<string> DeleteBlockTripAutoVehicle(string blockTripAutoVehicleId)
        {
            return await _blockTripAutoVehicleService.DeleteBlockTripAutoVehicle(blockTripAutoVehicleId);
        }
    }
}
