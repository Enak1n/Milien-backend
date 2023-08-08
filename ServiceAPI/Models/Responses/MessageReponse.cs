using Millien.Domain.Entities;

namespace ServiceAPI.Models.Responses
{
    public class MessageReponse
    {
        public Customer Customer { get; set; }
        public string Message { get; set; }
        public DateTime DateOfDispatch { get; set; }
        public bool IsRead { get; set; }

        public MessageReponse(Customer customer, string message, DateTime dateOfDispatch, bool isRead)
        {
            Customer = customer;
            Message = message;
            DateOfDispatch = dateOfDispatch;
            IsRead = isRead;
        }
    }
}
