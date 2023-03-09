using Google.Cloud.Firestore;

namespace BlockTrip.Model
{
    [FirestoreData]
    public class BlockTripReporting
    {
        public string Id { get; set; } // firebase unique id      

        [FirestoreProperty]
        public DateTime CreateDT { get; set; }

        [FirestoreProperty]
        public int VehicleTypeId { get; set; }

        [FirestoreProperty]
        public DateTime RequestedDateTime { get; set; }
    }
}
