using BlockTrip.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockTripConsoleApp.Services
{
    public interface IBlockTripServices
    {
        Task<JArray> GetMaxTripsPerHour(string sqlQuery);

        Task CreateBlockTripReporting(BlockTripReporting blockTripReporting);

        Task RemoveBlockTripReporting(string blockTripReportingId);
    }
}
