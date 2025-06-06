namespace Amizades.ViewModels
{
    public class PublicationViewModel
    {
        public int Id { get; set; }
        public string PublicationText { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorUsername { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
