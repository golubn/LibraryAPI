namespace LibraryAPI.Domain.Models
{
    public class BookDTO
    {
        public int ISBN { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public DateTime GetBook { get; set; }
        public DateTime ReturnBook { get; set; }
    }
}
