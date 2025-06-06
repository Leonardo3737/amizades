namespace Amizades.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public int PublicationId { get; set; }
        public Publication Publication { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
