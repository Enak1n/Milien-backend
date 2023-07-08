namespace MilienAPI.Models
{
    public class PaidAd
    {
        public int Id { get; set; }
        public int AdId { get; set; }
        public DateTime ExpiryTime { get; set; }

        public PaidAd(int adId, DateTime expiryTime)
        {
            AdId = adId;
            ExpiryTime = expiryTime;
        }
    }
}
