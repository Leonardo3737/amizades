using Amizades.Data;
using Amizades.Models;
using Amizades.Models.Enums;
using Amizades.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Amizades.Services
{
    public class FriendshipService
    {
        private PaginationService _paginationService { get; set; }
        private AmizadesContext _context { get; set; }

        public FriendshipService(
            AmizadesContext context,
            PaginationService paginationService
        ) 
        {
            _context = context;
            _paginationService = paginationService;
        }

        public PaginationViewModel<FriendshipViewModel> GetSuggestionsWithPagination(int userId, int pageSize, int page)
        {
            var suggestedUsers = _paginationService.Pagination(pageSize, page, _context.Users,
                u => (
                 u.Id != userId &&
                 !_context.Friendships.Any(f =>
                    (f.RequesterId == userId && f.AddresseeId == u.Id ||
                     f.RequesterId == u.Id && f.AddresseeId == userId) &&
                    (f.Status == FriendshipStatus.Pending || f.Status == FriendshipStatus.Accepted))
                ),
                user => new FriendshipViewModel
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Username = user.Username,
                    Status = _context.Friendships
                        .Where(f =>
                            (f.RequesterId == userId && f.AddresseeId == user.Id) &&
                            f.Status != FriendshipStatus.Accepted
                        )
                        .Select(f => (FriendshipStatus?)f.Status)
                        .FirstOrDefault() ?? null
                });
            return suggestedUsers;
        }

        public PaginationViewModel<FriendshipViewModel> GetFriendRequestsWithPagination(int userId, int pageSize, int page)
        {
            var friendRequests = _paginationService.Pagination(pageSize, page, _context.Friendships,
                f => f.AddresseeId == userId && f.Status == FriendshipStatus.Pending,
                f => new FriendshipViewModel
                {
                    FriendshipId = f.Id,
                    UserId = f.RequesterId,
                    Name = f.Requester.Name,
                    Username = f.Requester.Username,
                    Status = f.Status
                },
                new List<Func<IQueryable<Friendship>, IIncludableQueryable<Friendship, object>>>
                {
                    q => q.Include(f => f.Requester)
                }
            );

            return friendRequests;
        }

        public PaginationViewModel<FriendshipViewModel> GetFriendsWithPagination(int userId, int pageSize, int page)
        {
            var friends = _paginationService.Pagination(pageSize, page, _context.Friendships,
                f => (f.AddresseeId == userId || f.RequesterId == userId) && f.Status == FriendshipStatus.Accepted,
                f => new FriendshipViewModel
                {
                    FriendshipId = f.Id,
                    UserId = f.AddresseeId == userId ? f.RequesterId : f.AddresseeId,
                    Name = f.AddresseeId == userId ? f.Requester.Name : f.Addressee.Name,
                    Username = f.AddresseeId == userId ? f.Requester.Username : f.Addressee.Username,
                    Status = f.Status
                },
                new List<Func<IQueryable<Friendship>, IIncludableQueryable<Friendship, object>>>
                {
                    q => q.Include(f => f.Addressee),
                    q => q.Include(f => f.Requester)
                }
            );

            return friends;
        }

        public List<FriendshipViewModel> GetFriends(int userId)
        {
            var friends = _context.Friendships
                .Include(f => f.Addressee)
                .Include(f => f.Requester)
                .Where(f => (f.AddresseeId == userId || f.RequesterId == userId) && f.Status == FriendshipStatus.Accepted)
                .Select(
                f => new FriendshipViewModel
                {
                    FriendshipId = f.Id,
                    UserId = f.AddresseeId == userId ? f.RequesterId : f.AddresseeId,
                    Name = f.AddresseeId == userId ? f.Requester.Name : f.Addressee.Name,
                    Username = f.AddresseeId == userId ? f.Requester.Username : f.Addressee.Username,
                    Status = f.Status
                }).ToList();

            return friends;
        }
    }
}
