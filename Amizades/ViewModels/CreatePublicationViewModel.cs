using System.ComponentModel.DataAnnotations;

namespace Amizades.ViewModels
{
    public class CreatePublicationViewModel
    {
        [Required]
        [MinLength(1)]
        public string PublicationText { get; set; }
    }
}
