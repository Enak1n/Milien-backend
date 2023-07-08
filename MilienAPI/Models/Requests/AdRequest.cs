namespace MilienAPI.Models.Requests
{
    public class AdRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Adress { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public IFormFileCollection? Images { get; set; }
    }
}
