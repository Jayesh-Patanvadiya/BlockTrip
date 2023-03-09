using BlockTrip.Model;
using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace BlockTrip.Services
{

    public class BlockTripAutoVehicleService : IBlockTripAutoVehicleService
    {
        string projectId;
        FirestoreDb fireStoreDb;
        public BlockTripAutoVehicleService()
        {
            //_configuration = configuration;
            string filepath = @"\test-2a07f-4688daf8c712.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);

            projectId = "test-2a07f";
            fireStoreDb = FirestoreDb.Create(projectId);
        }

        public async Task<BlockTripAutoVehicle> CreateBlockTripAutoVehicle(BlockTripAutoVehicle blockTripAutoVehicle)
        {
            try
            {
                CollectionReference colRef = fireStoreDb.Collection("ezx.blockTripAutoVehicles");
                var result = await colRef.AddAsync(blockTripAutoVehicle);

                blockTripAutoVehicle.Id = result.Id;
                return blockTripAutoVehicle;

            }
            catch (Exception ex)
            {
                throw new Exception("Error " + ex.Message);
            }

        }

        public async Task<List<BlockTripAutoVehicle>> GetAllBlockTripAutoVehicles()
        {
            Query blockTripAutoVehiclesQuery = fireStoreDb.Collection("ezx.blockTripAutoVehicles");
            QuerySnapshot blockTripAutoVehiclesQuerySnapshot = await blockTripAutoVehiclesQuery.GetSnapshotAsync();
            List<BlockTripAutoVehicle> blockTripAutoVehicles = new List<BlockTripAutoVehicle>();

            foreach (DocumentSnapshot documentSnapshot in blockTripAutoVehiclesQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> ezPickupPointDic = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(ezPickupPointDic);
                    BlockTripAutoVehicle newBlockTripAutoVehicle = JsonConvert.DeserializeObject<BlockTripAutoVehicle>(json);
                    newBlockTripAutoVehicle.Id = documentSnapshot.Id;
                    blockTripAutoVehicles.Add(newBlockTripAutoVehicle);
                }
            }
            return blockTripAutoVehicles;

        }

        public async Task<BlockTripAutoVehicle> UpdateBlockTripAutoVehicle(BlockTripAutoVehicle blockTripAutoVehicles)
        {
            DocumentReference ezBlockTripAutoVehicle = fireStoreDb.Collection("ezx.blockTripAutoVehicles").Document(blockTripAutoVehicles.Id);
            await ezBlockTripAutoVehicle.SetAsync(blockTripAutoVehicles, SetOptions.Overwrite);
            return blockTripAutoVehicles;

        }
        public async Task<BlockTripAutoVehicle> GetBlockTripAutoVehicleId(string blockTripAutoVehicleid)
        {
            try
            {
                DocumentReference docRef = fireStoreDb.Collection("ezx.blockTripAutoVehicles").Document(blockTripAutoVehicleid);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    BlockTripAutoVehicle blockTripAutoVehicles = snapshot.ConvertTo<BlockTripAutoVehicle>();
                    blockTripAutoVehicles.Id = snapshot.Id;
                    return blockTripAutoVehicles;
                }
                else
                {
                    return new BlockTripAutoVehicle();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error " + ex.Message);
            }

        }
        public async Task<string> DeleteBlockTripAutoVehicle(string blockTripAutoVehicleId)
        {
            try
            {
                DocumentReference ezBlockTripAutoVehicle = fireStoreDb.Collection("ezx.blockTripAutoVehicles").Document(blockTripAutoVehicleId);
                await ezBlockTripAutoVehicle.DeleteAsync();
                return "Deleted Successfully!";
            }
            catch (Exception ex)
            {
                throw new Exception("Error " + ex.Message);
            }
        }
    }
}
