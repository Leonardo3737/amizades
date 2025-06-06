using Amizades.Data;
using Amizades.Models;
using Amizades.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Amizades.Services
{
    public class PublicationService
    {
        private AmizadesContext _context { get; set; }
        private PaginationService _paginationService { get; set; }
        private FriendshipService _friendshipService { get; set; }

        public PublicationService(
            AmizadesContext context,
            PaginationService paginationService,
            FriendshipService friendshipService
        )
        {
            _context = context;
            _paginationService = paginationService;
            _friendshipService = friendshipService;
        }

        public PaginationViewModel<PublicationViewModel> GetPublicationWithPagination(int userId, int pageSize, int page)
        {
            var friendUserIds = _friendshipService.GetFriends(userId).Select(f => f.UserId).ToList();

            _context.Publications.OrderBy(page => page.CreatedAt);

            var publications = _paginationService.Pagination(pageSize, page,
                _context.Publications,
                p => friendUserIds.Contains(p.AuthorId),
                p => new PublicationViewModel { 
                    Id = p.Id,
                    PublicationText = p.PublicationText,
                    AuthorId = p.AuthorId,
                    AuthorName = p.Author.Name,
                    AuthorUsername = p.Author.Username,
                    CreatedAt = p.CreatedAt
                },
                new List<Func<IQueryable<Publication>, IIncludableQueryable<Publication, object>>>
                {
                    q => q.Include(f => f.Author)
                },
                p => p.CreatedAt,
                true
            );
            return publications;
        }
    }
}
