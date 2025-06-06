using Amizades.Data;
using Amizades.Models;
using Amizades.Models.Enums;
using Amizades.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Amizades.Controllers.Api
{
    [Route("api/[controller]")]
    public class FriendshipController : Controller
    {
        private int pageSize = 3;
        private AmizadesContext _context { get; set; }
        private FriendshipService _friendshipService { get; set; }

        public FriendshipController(
            AmizadesContext context,
            FriendshipService homeService
            )
        {
            _context = context;
            _friendshipService = homeService;
        }

        [HttpGet("suggestion")]
        public IActionResult GetSuggestions(int page = 1)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var suggestions = _friendshipService.GetSuggestionsWithPagination(userId, pageSize, page);

            ViewData["HasNextPage"] = suggestions.HasNextPage;

            ViewData["type"] = "SUGESTION";

            return PartialView("~/Views/Home/Shared/_UserList.cshtml", suggestions.Items);
        }

        [HttpGet("friend-request")]
        public IActionResult GetFriendRequests(int page = 1)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var friendRequests = _friendshipService.GetFriendRequestsWithPagination(userId, pageSize, page);

            ViewData["HasNextPage"] = friendRequests.HasNextPage;

            ViewData["type"] = "REQUEST";

            return PartialView("~/Views/Home/Shared/_UserList.cshtml", friendRequests.Items);
        }

        [HttpGet("friend")]
        public IActionResult GetFriends(int page = 1)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var friendRequests = _friendshipService.GetFriendsWithPagination(userId, pageSize, page);

            ViewData["HasNextPage"] = friendRequests.HasNextPage;

            ViewData["type"] = "FRIEND";

            return PartialView("~/Views/Home/Shared/_UserList.cshtml", friendRequests.Items);
        }


        [HttpPost("invite/{addresseeId}")]
        public async Task<IActionResult> Invite(int addresseeId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var addresseeUser = _context.Users.Find(addresseeId);

            if (addresseeUser == null)
            {
                return NotFound();
            }

            // Verificar se já existe amizade ou convite pendente
            bool jaExiste = _context.Friendships.Any(f =>
                f.RequesterId == userId && f.AddresseeId == addresseeId ||
                f.RequesterId == addresseeId && f.AddresseeId == userId);

            if (jaExiste)
                return Conflict("Já existe uma solicitação ou amizade.");

            var newRequestFriendship = new Friendship()
            {
                RequesterId = userId,
                AddresseeId = addresseeId,
            };
            _context.Friendships.Add(newRequestFriendship);
            await _context.SaveChangesAsync();
            return Created(string.Empty, new { message = "Solicitação enviada." }); ;
        }

        [HttpPatch("accept/{friendshipId}")]
        public async Task<IActionResult> Accept(int friendshipId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var invite = _context.Friendships.Find(friendshipId);

            if (invite == null)
            {
                return NotFound();
            }


            invite.Status = FriendshipStatus.Accepted;
            invite.AcceptedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Solicitação aceita com sucesso." });
        }

        [HttpDelete("remove/{friendshipId}")]
        public async Task<IActionResult> Remove(int friendshipId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var invite = _context.Friendships.Find(friendshipId);

            if (invite == null)
            {
                return NotFound();
            }
            _context.Friendships.Remove(invite);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Amizade removida com sucesso." });
        }
    }
}
