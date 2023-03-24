using Google.Cloud.Firestore;

namespace BlockTrip.Model
{
    public class BlockTripReportingPriceDto
    {
       
            public string Id { get; set; } // firebase unique id      

            public DateTime CreateDT { get; set; }

            public int VehicleTypeId { get; set; }

            public DateTime RequestedDateTime { get; set; }

            // Newly Added

            //Add Price field to BlockTripReporting
            //[FirestoreProperty]
            //public long Price { get; set; }
            //Add Hour field to BlockTripReporting
            public int Hour { get; set; }
            //Add PriceListId int field to BlockTripReporting
            public int PriceListId { get; set; }


        

    }
}
