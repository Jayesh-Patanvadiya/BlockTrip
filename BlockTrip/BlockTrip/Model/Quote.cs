namespace BlockTrip.Model
{
    public class Quote
    {
        public string Id { get; set; }
        public string LinkedQuoteId { get; set; }
        public long TotalAmountInCents { get; set; }
        public long BaseAmountInCents { get; set; }
        public string PickupPlaceId { get; set; }
        public string DropoffPlaceId { get; set; }
        public int VehicleTypeId { get; set; }
        public string VehicleTypeName { get; set; }
        public string VehicleTypeThumbnailUrl { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int ClientId { get; set; }
        public string CouponCode { get; set; }
        public int CouponDiscountAmountInCents { get; set; }
        public int AfterHoursSurcharge { get; set; }
        public int BabySeatSurcharge { get; set; }
        public int TrailerSurcharge { get; set; }
        public int PricePlanId { get; set; }
        public bool IncludeTrailer { get; set; }
        public bool IncludeBabySeat { get; set; }
        public bool IncludeSanitize { get; set; }
        public DateTime CreateDateTimeUtc { get; set; }
    }


}
