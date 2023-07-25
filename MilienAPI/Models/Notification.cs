namespace MilienAPI.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int CustomerId { get; set; }
        public int OwnerId { get; set; }    
        public DateTime DateOfCreation { get; set; } = DateTime.Now;

        public Notification(string message, int customerId, int ownerId)
        {
            Message = message;
            CustomerId = customerId;
            OwnerId = ownerId;
        }
    }
}
