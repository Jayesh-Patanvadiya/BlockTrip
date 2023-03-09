using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace BlockTrip.Model
{
    [FirestoreData]

    public class BlockTripAutoVehicle
    {

        public string Id { get; set; } // firebase unique id

        [FirestoreProperty]
        public int VehicleId { get; set; }


        [FirestoreProperty]
        public int MaxTripsPerHour { get; set; }
    }
}
