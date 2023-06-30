namespace MilienAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Role Role { get; set; }
        public string AboutMe { get; set; }
    }
}
