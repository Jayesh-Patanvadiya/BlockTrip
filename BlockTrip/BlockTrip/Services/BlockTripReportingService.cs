using BlockTrip.Model;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlockTrip.Services
{
    public class BlockTripReportingService : IBlockTripReportingService
    {
        string projectId;
        FirestoreDb fireStoreDb;
        public BlockTripReportingService()
        {
            //_configuration = configuration;
            string filepath = @"\test-2a07f-4688daf8c712.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);

            projectId = "test-2a07f";
            fireStoreDb = FirestoreDb.Create(projectId);
        }

        public async Task<BlockTripReporting> CreateBlockTripReporting(BlockTripReporting blockTripReporting)
        {
            try
            {
                CollectionReference colRef = fireStoreDb.Collection("ezx.blockTripReportings");
                var result = await colRef.AddAsync(blockTripReporting);

                blockTripReporting.Id = result.Id;
                return blockTripReporting;

            }
            catch (Exception ex)
            {
                throw new Exception("Error " + ex.Message);
            }

        }

        public async Task<List<BlockTripReporting>> GetAllBlockTripReporting()
        {
            Query blockTripReportingQuery = fireStoreDb.Collection("ezx.blockTripReportings");
            QuerySnapshot blockTripReportingQuerySnapshot = await blockTripReportingQuery.GetSnapshotAsync();
            List<BlockTripReporting> blockTripReportings = new List<BlockTripReporting>();

            foreach (DocumentSnapshot documentSnapshot in blockTripReportingQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    BlockTripReporting blockTripReporting = documentSnapshot.ConvertTo<BlockTripReporting>();
                    blockTripReporting.Id = documentSnapshot.Id;
                    blockTripReportings.Add(blockTripReporting);
                }
            }
            return blockTripReportings.OrderByDescending(x=>x.RequestedDateTime).ToList();

        }

        
        public async Task<List<BlockTripReportingPriceDto>> GetAllBlockTripReportingWithoutPrice()
        {
            var result = await GetAllBlockTripReporting();
            List<BlockTripReportingPriceDto> blockTripReportingPriceDtosList = new List<BlockTripReportingPriceDto>();
            foreach (var reporting in result)
            {
                BlockTripReportingPriceDto blockTripReportingPriceDtos = new BlockTripReportingPriceDto()
                {
                    Hour = reporting.Hour,
                    Id = reporting.Id,
                    PriceListId = reporting.PriceListId,
                    RequestedDateTime = reporting.RequestedDateTime,
                    VehicleTypeId = reporting.VehicleTypeId,
                    CreateDT = reporting.CreateDT,
                };
                blockTripReportingPriceDtosList.Add(blockTripReportingPriceDtos);

            }

            return blockTripReportingPriceDtosList;

        }



        public async Task<BlockTripReporting> UpdateBlockTripReporting(BlockTripReporting blockTripReporting)
        {
            DocumentReference ezBlockTripReporting = fireStoreDb.Collection("ezx.blockTripReportings").Document(blockTripReporting.Id);
            await ezBlockTripReporting.SetAsync(blockTripReporting, SetOptions.Overwrite);
            return blockTripReporting;

        }
        public async Task<BlockTripReporting> GetBlockTripReportingById(string blockTripReportingid)
        {
            try
            {
                DocumentReference docRef = fireStoreDb.Collection("ezx.blockTripReportings").Document(blockTripReportingid);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    BlockTripReporting blockTripReportingss = snapshot.ConvertTo<BlockTripReporting>();
                    blockTripReportingss.Id = snapshot.Id;
                    return blockTripReportingss;
                }
                else
                {
                    return new BlockTripReporting();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error " + ex.Message);
            }

        }
        public async Task<string> DeleteBlockTripReporting(string blockTripReportingId)
        {
            try
            {
                DocumentReference ezBlockTripReporting = fireStoreDb.Collection("ezx.blockTripReportings").Document(blockTripReportingId);
                await ezBlockTripReporting.DeleteAsync();
                return "Deleted Successfully!";
            }
            catch (Exception ex)
            {
                throw new Exception("Error " + ex.Message);
            }
        }

    }
}
