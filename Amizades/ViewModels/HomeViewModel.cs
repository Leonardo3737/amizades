using Amizades.Models;

namespace Amizades.ViewModels
{
    public class HomeViewModel
    {
        public HomeUserViewModel CurrentUser { get; set; }
        public PaginationViewModel<FriendshipViewModel> SuggestedUsers { get; set; }
        public PaginationViewModel<PublicationViewModel> Publications { get; set; }
        public CreatePublicationViewModel CreatePublicationViewModel { get; set; }
    }
}
