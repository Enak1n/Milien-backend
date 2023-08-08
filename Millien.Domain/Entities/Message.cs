namespace Millien.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Text { get; set; }
        public DateTime DateOfDispatch { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

        public Message(int senderId, int recpientId, string meessage)
        {
            SenderId = senderId;
            RecipientId = recpientId;
            Text = meessage;
        }

        public Message()
        {
        }
    }
}
