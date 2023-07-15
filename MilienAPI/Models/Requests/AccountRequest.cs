namespace MilienAPI.Models.Requests
{
    public class AccountRequest
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? AboutMe { get; set; }
        public IFormFile? Avatar { get; set; }
    }
}
