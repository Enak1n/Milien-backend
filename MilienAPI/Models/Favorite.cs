namespace MilienAPI.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int AdId { get; set; }

        public Favorite(int customerId, int adId)
        {
            CustomerId = customerId;
            AdId = adId;
        }
    }
}
