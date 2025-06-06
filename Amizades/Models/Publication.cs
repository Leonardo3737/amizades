namespace Amizades.Models
{
    public class Publication
    {
        public int Id { get; set; }
        public string PublicationText { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public List<Comment> Comments { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
