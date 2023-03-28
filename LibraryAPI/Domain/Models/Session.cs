
namespace LibraryAPI.Domain.Models
{
    public class Session
    {
        public int SessionId { get; set; }
        public string Token { get; set; } = string.Empty;
        public SessionStatus? Status { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = null;
        public DateTime? DeletedAt { get; set; } = null;

        public int UserId { get; set; }
        public User? User { get; set; }
    }

    public enum SessionStatus
    {
        Open,
        Closed
    }
}
