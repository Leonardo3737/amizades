using Amizades.Models.Enums;

namespace Amizades.ViewModels
{
    public class FriendshipViewModel
    {
        public int FriendshipId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public FriendshipStatus? Status { get; set; }
    }
}
