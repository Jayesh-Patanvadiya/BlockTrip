using BlockTrip.Model;
using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace BlockTrip.Services
{
    public class BlockTripsService : IBlockTripsService
    {
        string projectId;
        FirestoreDb fireStoreDb;
        public BlockTripsService()
        {
            //_configuration = configuration;
            string filepath = @"\test-2a07f-4688daf8c712.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);

            projectId = "test-2a07f";
            fireStoreDb = FirestoreDb.Create(projectId);
        }

        public async Task<BlockTrips> CreateBlockTrip(BlockTrips blockTrip)
        {
            try
            {
                CollectionReference colRef = fireStoreDb.Collection("ezx.blockTrips");
                var result = await colRef.AddAsync(blockTrip);

                blockTrip.Id = result.Id;
                return blockTrip;

            }
            catch (Exception ex)
            {
                throw new Exception("Error " + ex.Message);
            }

        }

        public async Task<List<BlockTrips>> GetAllBlockTrips()
        {
            Query blockTripQuery = fireStoreDb.Collection("ezx.blockTrips");
            QuerySnapshot blockTripQuerySnapshot = await blockTripQuery.GetSnapshotAsync();
            List<BlockTrips> blockTrips = new List<BlockTrips>();

            foreach (DocumentSnapshot documentSnapshot in blockTripQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {

                    BlockTrips blockTrip = documentSnapshot.ConvertTo<BlockTrips>();
                    blockTrip.Id = documentSnapshot.Id;
                    blockTrips.Add(blockTrip);
                }
            }
            
            return blockTrips;

        }

        public async Task<BlockTrips> UpdateBlockTrip(BlockTrips blockTrip)
        {
            DocumentReference ezBlockTrips = fireStoreDb.Collection("ezx.blockTrips").Document(blockTrip.Id);
            await ezBlockTrips.SetAsync(blockTrip, SetOptions.Overwrite);
            return blockTrip;

        }
        public async Task<BlockTrips> GetBlockTripById(string blockTripid)
        {
            try
            {
                DocumentReference docRef = fireStoreDb.Collection("ezx.blockTrips").Document(blockTripid);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    BlockTrips blockTripss = snapshot.ConvertTo<BlockTrips>();
                    blockTripss.Id = snapshot.Id;
                    return blockTripss;
                }
                else
                {
                    return new BlockTrips();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error " + ex.Message);
            }

        }
        public async Task<string> DeleteBlockTrip(string blockTripId)
        {
            try
            {
                DocumentReference ezBlockTrips = fireStoreDb.Collection("ezx.blockTrips").Document(blockTripId);
                await ezBlockTrips.DeleteAsync();
                return "Deleted Successfully!";
            }
            catch (Exception ex)
            {
                throw new Exception("Error " + ex.Message);
            }
        }

    }
}
