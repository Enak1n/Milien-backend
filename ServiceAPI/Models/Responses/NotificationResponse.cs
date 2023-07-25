namespace ServiceAPI.Models.Responses
{
    public class NotificationResponse
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int CustomerId { get; set; }
        public int OwnerId { get; set; }
        public DateTime DateOfCreation { get; set; } = DateTime.Now;
        public Customer Customer { get; set; }
    }
}
