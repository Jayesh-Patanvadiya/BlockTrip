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
            var result =  await _blockTripReportingService.GetAllBlockTripReporting();
            return result.OrderByDescending(x=>x.RequestedDateTime).ToList();
        }

        [HttpGet("blockTripReportingId")]
        public async Task<BlockTripReporting> GetBlockTripReportingId(string blockTripReportingId)
        {
            return await _blockTripReportingService.GetBlockTripReportingById(blockTripReportingId);
        }


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
