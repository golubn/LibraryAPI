using System.Data;

namespace LibraryAPI.Domain.Models
{
    public class Book
    {
        public int Id { get; set; }
        public int ISBN { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public DateTime GetBook { get; set; } = DateTime.MinValue;
        public DateTime ReturnBook { get; set; } = DateTime.MinValue;
        public User? User { get; set; } = null;

    }
}
