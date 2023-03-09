using Google.Cloud.Firestore;

namespace BlockTrip.Model
{
    [FirestoreData]

    public class BlockTrips
    {
        public string Id { get; set; } // firebase unique id

        [FirestoreProperty]
        public int VehicleId { get; set; }


        [FirestoreProperty]
        public BlockTripType BlockTripType { get; set; }

        [FirestoreProperty]
        public DateTime StartDT { get; set; }

        [FirestoreProperty]
        public DateTime EndDT { get; set; }

        [FirestoreProperty]
        public int UserId { get; set; }
    }

    public enum BlockTripType
    {
        auto = 1,
        manual = 2
    }

}
