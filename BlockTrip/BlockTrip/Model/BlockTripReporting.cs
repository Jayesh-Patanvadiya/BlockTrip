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

        // Newly Added
        //Add Price field to BlockTripReporting
        [FirestoreProperty]
        public long Price { get; set;}
        //Add Hour field to BlockTripReporting
        [FirestoreProperty]
        public int Hour { get; set; }
        //Add PriceListId int field to BlockTripReporting
        [FirestoreProperty]
        public int PriceListId { get; set; }


    }
}
