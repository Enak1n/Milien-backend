namespace MilienAPI.Models.Responses
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? AboutMe { get; set; }
        public string? Avatar { get; set; }
    }
}
