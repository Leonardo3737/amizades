using Amizades.Controllers;
using Amizades.Data;
using Amizades.ViewModels;

namespace Amizades.Services
{
    public class HomeService
    {
        private int pageSize = 3;
        private AmizadesContext _context { get; set; }
        private FriendshipService _friendshipService { get; set; }
        private PublicationService _publicationService { get; set; }

        public HomeService(
            AmizadesContext context,
            FriendshipService homeService,
            PublicationService publicationService
        )
        {
            _context = context;
            _friendshipService = homeService;
            _publicationService = publicationService;
        }

        public HomeViewModel GetHomeViewModelData(int userId)
        {
            var user = _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new HomeUserViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    Username = u.Username,
                    Email = u.Email,
                })
                .FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            user.Friends = _friendshipService.GetFriendsWithPagination(userId, pageSize, 0);

            user.ReceivedFriendRequests = _friendshipService.GetFriendRequestsWithPagination(userId, pageSize, 0);

            var suggestions = _friendshipService.GetSuggestionsWithPagination(userId, pageSize, 0);

            var publications = _publicationService.GetPublicationWithPagination(userId, pageSize, 0);

            var data = new HomeViewModel()
            {
                CurrentUser = user,
                SuggestedUsers = suggestions,
                Publications = publications,
                CreatePublicationViewModel = new CreatePublicationViewModel()
            };
            return data;
        }
    }
}
